using System;
using System.Collections.Generic;

namespace EasySave
{
    /// <summary>
    /// Classe principale de l'application
    /// Elle est chargee de faire l'interface entre l'utilisateur et les travaux de sauvegarde, en permettant
    /// par exemple de lancer une sauvegarde, de creer une sauvegarde, ou de lister les sauvegardes disponibles.
    /// </summary>
    public class EasySaveC
    {
        private readonly SaveConfiguration _saveConfiguration;
        public EasySaveC()
        {
            _saveConfiguration = SaveConfiguration.GetInstance();
        }

        /// <summary>
        /// TODO: @Faugnell (Victor)
        /// Copier un fichier d'un emplacement source a un emplacement destination
        /// </summary>
        /// <param name="source">Emplacement source du fichier</param>
        /// <param name="destination">Emplacement de destination du fichier</param>
        /// <returns>True si la copie s'est effectuee, false sinon</returns>
        /// <exception cref="Exception">Lance une exception si une erreur survient lors de la copie</exception>"
        private bool CopierFichier(string source, string destination)
        {
            // TODO
            throw new Exception("TODO: CopierFichier");

            // Retourner true / false si la copier s'est effectuee ou non?
            // Exception lors d'erreur?
            return true;
        }

        /// <summary>
        /// Lancer un ou plusieurs travaux de sauvegarde
        /// </summary>
        /// <param name="listeId">Liste des identifiants des sauvegardes a lancer, Si aucun id n'est specifie, lance toutes les sauvegardes.</param>
        public void LancerSauvegarde(List<int> listeId)
        {
            // Si la liste est vide, on lance toutes les sauvegardes
            if (listeId.Count == 0)
            {
                listeId = _saveConfiguration.GetConfiguration().ConvertAll(save => save.Id);
            }

            Console.WriteLine(listeId.Count);
            listeId.ForEach(id => Console.WriteLine(id));

            foreach (int id in listeId)
            {
                Save save = _saveConfiguration.GetConfiguration(id) ?? throw new ArgumentException($"Il n'existe pas de configuration de sauvegarde pour cet identifiant: {id}");

                if (save.SaveType == SaveType.COMPLETE)
                {
                    EffectuerSauvegardeComplete(save);
                }
                else if (save.SaveType == SaveType.DIFFERENTIAL)
                {
                    EffectuerSauvegardeDifferentielle(save);
                }
            }
        }

        /// <summary>
        /// TODO: @Faugnell (Victor)
        /// Lancer une sauvegarde complete
        /// </summary>
        /// <param name="save">La sauvegarde a effectuer</param>
        private void EffectuerSauvegardeComplete(Save save)
        {
            throw new Exception("TODO: EffectuerSauvegardeComplete");
        }

        private void EffectuerSauvegardeDifferentielle(Save save)
        {

        }
    }
}
