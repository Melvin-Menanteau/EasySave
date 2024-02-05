using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace EasySave
{
    public class SaveConfiguration
    {
        private static SaveConfiguration _instance;

        List<Save> saveConfiguration;

        /* R��criture du constructeur pour le rendre priv� */
        private SaveConfiguration()
        {
            saveConfiguration = GetConfigurations();
            Console.WriteLine(saveConfiguration.Count);
            saveConfiguration.ForEach(save => Console.WriteLine(save.ToString()));

            UpdateConfiguration(0, "Sauvegarde 1", "C:/Users/Utilisateur/Documents", "D:/Sauvegardes", SaveType.COMPLETE);

            saveConfiguration.ForEach(save => Console.WriteLine(save.ToString()));
        }

        /* M�thode pour r�cup�rer l'instance de la classe */
        public static SaveConfiguration GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SaveConfiguration();
            }

            return _instance;
        }

        /* R�cup�ration des configurations enregistr�es depuis un fichier */
        private List<Save> GetConfigurations()
        {
            return new List<Save>() {
                new Save("Sauvegarde 1", "C:/Users/Utilisateur/Documents", "D:/Sauvegardes", SaveType.COMPLETE, 5),
                new Save("Sauvegarde 2", "C:/Users/Utilisateur/Images", "D:/Sauvegardes", SaveType.DIFFERENTIAL),
                new Save("Sauvegarde 3", "C:/Users/Utilisateur/Vid�os", "D:/Sauvegardes", SaveType.COMPLETE, 3)
            };
        }

        /// <summary>
        /// R�cup�rer l'ensemble des configurations de sauvegarde disponible
        /// </summary>
        /// <returns>Liste des configurations de sauvegarde</returns>
        public List<Save> GetConfiguration()
        {
            return saveConfiguration;
        }

        /// <summary>
        /// R�cup�rer une configuration de sauvegarde par son identifiant
        /// </summary>
        /// <param name="id">L'identifiant de la configuration</param>
        /// <returns>La premi�re configuration de sauvegarde trouv�e avec cette id ou null si aucune n'existe</returns>
        public Save GetConfiguration(int id)
        {
            return saveConfiguration.Find(save => save.Id == id) ?? null;
        }

        /// <summary>
        /// R�cup�rer une configuration de sauvegarde par son nom
        /// </summary>
        /// <param name="name">Le nom de la sauvegarde</param>
        /// <returns>La premi�re configuration de sauvegarde trouv�e avec ce nom ou null si aucune n'existe</returns>
        public Save GetConfiguration(string name)
        {
            return saveConfiguration.Find(save => save.Name == name) ?? null;
        }

        /// <summary>
        /// Ajouter une configuration de sauvegarde
        /// </summary>
        /// <param name="nom">Le nom de la sauvegarde</param>
        /// <param name="inputFolder">Le dossier source de la sauvegarde</param>
        /// <param name="outputFolder">Le dossier de destination de la sauvegarde</param>
        /// <param name="saveType">Le type de sauvegarde</param>
        /// <exception cref="Exception">Il existe d�j� 5 configurations enregistr�es</exception>
        public void AddConfiguration(string nom, string inputFolder, string outputFolder, SaveType saveType)
        {
            if (saveConfiguration.Count >= 5)
            {
                throw new Exception("Le nombre maximum de sauvegardes est atteint");
            }

            saveConfiguration.Add(new Save(nom, inputFolder, outputFolder, saveType));
        }

        /// <summary>
        /// Supprimer une configuration de sauvegarde
        /// </summary>
        /// <param name="id">L'identifiant de la configuration � supprimer</param>
        public bool RemoveConfiguration(int id)
        {
            return saveConfiguration.Remove(GetConfiguration(id));
        }

        /// <summary>
        /// Mettre � jour une configuration de sauvegarde
        /// </summary>
        /// <param name="id">L'identifiant de la configuration � mettre � jour</param>
        /// <param name="nom">Le nom de la sauvegarde</param>
        /// <param name="inputFolder">Le dossier source de la sauvegarde</param>
        /// <param name="outputFolder">Le dossier de destination de la sauvegarde</param>
        /// <param name="saveType">Le type de sauvegarde</param>
        /// <exception cref="Exception">Aucune sauvegarde ne correspond � cet identifiant</exception>
        public void UpdateConfiguration(int id, [Optional] string nom, [Optional]  string inputFolder, [Optional] string outputFolder, SaveType? saveType)
        {
            Save save = GetConfiguration(id) ?? throw new Exception("Aucune sauvegarde ne correspond � cet identifiant");

            if (nom != null)
                save.Name = nom;

            if (inputFolder != null)
                save.InputFolder = inputFolder;
            
            if (outputFolder != null)
                save.OutputFolder = outputFolder;

            //if (saveType != null)
            //    save.SaveType = saveType;
        }
    }
}