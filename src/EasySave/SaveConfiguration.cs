using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EasySave
{
    public class SaveConfiguration
    {
        private static SaveConfiguration _instance;

        private readonly List<Save> ListeConfiguration;

        /// <summary>
        /// Rendre le constructeur prive afin d'empecher l'instanciation de la classe
        /// </summary>
        private SaveConfiguration()
        {
            ListeConfiguration = GetConfigurations();
        }

        /// <summary>
        /// Méthode pour recuperer l'instance de la classe
        /// </summary>
        /// <returns>Instance de la classe</returns>
        public static SaveConfiguration GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SaveConfiguration();
            }

            return _instance;
        }

        /// <summary>
        /// Recuperation des configurations de sauvegarde depuis un fichier de configuration
        /// </summary>
        /// <returns>Liste des configurations de sauvegarde</returns>
        private static List<Save> GetConfigurations()
        {
            if (!File.Exists("config.json"))
            {
                FileStream file = new FileStream("config.json", FileMode.Create);
                StreamWriter writer = new StreamWriter(file);
                writer.Write("[]");
                writer.Close();
                file.Close();
              
                return new List<Save>();
            }
          
            string jsonString = File.ReadAllText("config.json");
            // spit the string into an array of strings using the }, as a delimiter
            List<Save> list = JsonSerializer.Deserialize<List<Save>>(jsonString);
          
            return list;
        }

        /// <summary>
        /// Recuperer l'ensemble des configurations de sauvegarde disponible
        /// </summary>
        /// <returns>Liste des configurations de sauvegarde</returns>
        public List<Save> GetConfiguration()
        {
            return ListeConfiguration;
        }

        /// <summary>
        /// Recuperer une configuration de sauvegarde par son identifiant
        /// </summary>
        /// <param name="id">L'identifiant de la configuration</param>
        /// <returns>La premiere configuration de sauvegarde trouvee avec cette id ou null si aucune n'existe</returns>
        public Save GetConfiguration(int id)
        {
            return ListeConfiguration.Find(save => save.Id == id) ?? null;
        }

        /// <summary>
        /// Recuperer une configuration de sauvegarde par son nom
        /// </summary>
        /// <param name="name">Le nom de la sauvegarde</param>
        /// <returns>La premiere configuration de sauvegarde trouvee avec ce nom ou null si aucune n'existe</returns>
        public Save GetConfiguration(string name)
        {
            return ListeConfiguration.Find(save => save.Name == name) ?? null;
        }

        private static int GetMaximumIdFromSaves()
        {
            int maximumId = 0;

            List<Save> saves = GetConfigurations();

            if (saves.Count > 0)
            {
                foreach (Save save in saves)
                {
                    if (save.Id > maximumId)
                    {
                        maximumId = save.Id;
                    }
                }
            }

            return maximumId;
        }

        /// <summary>
        /// Ajouter une configuration de sauvegarde
        /// </summary>
        /// <param name="nom">Le nom de la sauvegarde</param>
        /// <param name="inputFolder">Le dossier source de la sauvegarde</param>
        /// <param name="outputFolder">Le dossier de destination de la sauvegarde</param>
        /// <param name="saveType">Le type de sauvegarde</param>
        /// <exception cref="Exception">Il existe deja 5 configurations enregistr�es</exception>
        public void AddConfiguration(string nom, string inputFolder, string outputFolder, SaveType saveType)
        {
            int id = GetMaximumIdFromSaves() + 1;

            ListeConfiguration.Add(new Save(id, nom, inputFolder, outputFolder, saveType));

            SaveConfigToFile();
        }

        /// <summary>
        /// Supprimer une configuration de sauvegarde
        /// </summary>
        /// <param name="id">L'identifiant de la configuration a supprimer</param>
        public bool RemoveConfiguration(int id)
        {
            bool deleted = ListeConfiguration.Remove(GetConfiguration(id));

            SaveConfigToFile();

            return deleted;
        }

        /// <summary>
        /// Mettre a jour une configuration de sauvegarde
        /// </summary>
        /// <param name="id">L'identifiant de la configuration a mettre a jour</param>
        /// <param name="nom">Le nom de la sauvegarde</param>
        /// <param name="inputFolder">Le dossier source de la sauvegarde</param>
        /// <param name="outputFolder">Le dossier de destination de la sauvegarde</param>
        /// <param name="saveType">Le type de sauvegarde</param>
        /// <exception cref="Exception">Aucune sauvegarde ne correspond a cet identifiant</exception>
        public void UpdateConfiguration(int id, [Optional] string nom, [Optional] string inputFolder, [Optional] string outputFolder, SaveType? saveType = null)
        {
            Save save = GetConfiguration(id) ?? throw new ArgumentException("Aucune sauvegarde ne correspond a cet identifiant");

            if (nom != null)
                save.Name = nom;

            if (inputFolder != null)
                save.InputFolder = inputFolder;

            if (outputFolder != null)
                save.OutputFolder = outputFolder;

            if (saveType != null)
                save.SaveType = saveType.Value;

            SaveConfigToFile();
        }

        /// <summary>
        /// Enregistrer les configurations de sauvegarde dans un fichier de configuration
        /// </summary>
        private void SaveConfigToFile()
        {
            string save_json = JsonSerializer.Serialize(ListeConfiguration);
            FileStream file = new FileStream("config.json", FileMode.Create);
            StreamWriter writer = new StreamWriter(file);

            writer.Write(save_json);
            writer.Close();
            file.Close();
        }
    }
}