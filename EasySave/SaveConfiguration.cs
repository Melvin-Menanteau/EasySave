using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
        private List<Save> GetConfigurations()
        {
            return new List<Save>() {
                new Save(5, "Sauvegarde 1", "C:\\Users\\vpetit\\Desktop\\test\\source", "C:\\Users\\vpetit\\Desktop\\test\\destination", SaveType.DIFFERENTIAL),
                //new Save(1, "Sauvegarde 2", "C:/Users/Utilisateur/Images", "D:/Sauvegardes", SaveType.DIFFERENTIAL),
                //new Save(3, "Sauvegarde 3", "C:/Users/Utilisateur/Vid�os", "D:/Sauvegardes", SaveType.COMPLETE)
            };
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
            if (ListeConfiguration.Count >= 5)
            {
                throw new Exception("Le nombre maximum de sauvegardes est atteint");
            }

            ListeConfiguration.Add(new Save(null, nom, inputFolder, outputFolder, saveType));
        }

        /// <summary>
        /// Supprimer une configuration de sauvegarde
        /// </summary>
        /// <param name="id">L'identifiant de la configuration a supprimer</param>
        public bool RemoveConfiguration(int id)
        {
            return ListeConfiguration.Remove(GetConfiguration(id));
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
        public void UpdateConfiguration(int id, [Optional] string nom, [Optional]  string inputFolder, [Optional] string outputFolder, SaveType? saveType = null)
        {
            Save save = GetConfiguration(id) ?? throw new Exception("Aucune sauvegarde ne correspond a cet identifiant");

            if (nom != null)
                save.Name = nom;

            if (inputFolder != null)
                save.InputFolder = inputFolder;
            
            if (outputFolder != null)
                save.OutputFolder = outputFolder;

            if (saveType != null)
                save.SaveType = saveType.Value;
        }
    }
}