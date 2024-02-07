using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EasySave
{
    /// <summary>
    /// Classe principale de l'application
    /// Elle est chargee de faire l'interface entre l'utilisateur et les travaux de sauvegarde, en permettant
    /// par exemple de lancer une sauvegarde, de creer une sauvegarde, ou de lister les sauvegardes disponibles.
    /// </summary>
    public class EasySave
    {
        private readonly SaveConfiguration _saveConfiguration;
        public EasySave()
        {
            _saveConfiguration = SaveConfiguration.GetInstance();
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
        /// Lancer une sauvegarde complète
        /// </summary>
        /// <param name="save">La sauvegarde à effectuer</param>
        public void EffectuerSauvegardeComplete(Save save)
        {
            try
            {
                // Vérifier si le dossier source existe
                if (!Directory.Exists(save.InputFolder))
                {
                    throw new DirectoryNotFoundException($"Le dossier source '{save.InputFolder}' n'existe pas.");
                }

                // Créer le dossier cible s'il n'existe pas
                if (!Directory.Exists(save.OutputFolder))
                {
                    Directory.CreateDirectory(save.OutputFolder);
                }

                // Copier le contenu du dossier source vers le dossier cible
                CopyDirectory(save.InputFolder, save.OutputFolder);

                Console.WriteLine("La sauvegarde est terminée.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la sauvegarde : {ex.Message}");
            }
        }

        /// <summary>
        /// Copier le contenu d'un répertoire vers un autre répertoire
        /// </summary>
        /// <param name="sourceDir">Répertoire source</param>
        /// <param name="targetDir">Répertoire cible</param>
        private void CopyDirectory(string sourceDir, string targetDir)
        {
            sourceDir = Path.GetFullPath(new Uri(sourceDir).LocalPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            targetDir = Path.GetFullPath(new Uri(targetDir).LocalPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Si le répertoire de destination n'existe pas, le créer
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Copier les fichiers du dossier source vers le dossier cible
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(targetDir, fileName);
                File.Copy(file, destFile, true);
            }

            // Copier les sous-dossiers récursivement
            foreach (string subDirectory in Directory.GetDirectories(sourceDir))
            {
                string subDirectoryName = new DirectoryInfo(subDirectory).Name;
                string destSubDirectory = Path.Combine(targetDir, subDirectoryName);
                CopyDirectory(subDirectory, destSubDirectory);
            }
        }

        public void EffectuerSauvegardeDifferentielle(Save save)
        {
            try
            {
                if (!Directory.Exists(save.InputFolder))
                {
                    throw new DirectoryNotFoundException($"Le dossier source '{save.InputFolder}' n'existe pas.");
                }

                if (!Directory.Exists(save.OutputFolder))
                {
                    Directory.CreateDirectory(save.OutputFolder);
                }

                // Dictionnaire pour stocker les hachages MD5 des fichiers du dossier cible
                Dictionary<string, string> targetFileHashes = new Dictionary<string, string>();

                // Récupérer les fichiers et leurs hachages dans le dossier cible
                string[] targetFiles = Directory.GetFiles(save.OutputFolder, "*.*", SearchOption.AllDirectories);
                foreach (string targetFile in targetFiles)
                {
                    using (var hash = MD5.Create())
                    using (var stream = File.OpenRead(targetFile))
                    {
                        byte[] hashBytes = hash.ComputeHash(stream);
                        targetFileHashes[targetFile] = BitConverter.ToString(hashBytes).Replace("-", "");
                    }
                }

                // Copier les fichiers du dossier source vers le dossier cible si nécessaire
                string[] sourceFiles = Directory.GetFiles(save.InputFolder, "*.*", SearchOption.AllDirectories);
                foreach (string sourceFile in sourceFiles)
                {
                    string relativePath = sourceFile.Substring(save.InputFolder.Length + 1);
                    string targetFile = Path.Combine(save.OutputFolder, relativePath);

                    // Vérifie si le fichier existe dans le dossier cible avant la sauvegarde
                    bool targetFileExists = targetFileHashes.ContainsKey(targetFile);

                    // Si le fichier du dossier source existe déjà dans le dossier de destination et est identique
                    if (targetFileExists && IsSameContent(sourceFile, targetFile))
                    {
                        // Ne rien faire, le fichier est déjà présent et identique
                        continue;
                    }

                    // Le fichier est soit nouveau, soit différent, donc le copier dans le dossier cible
                    string targetDirName = Path.GetDirectoryName(targetFile);
                    Directory.CreateDirectory(targetDirName);

                    File.Copy(sourceFile, targetFile, true);
                    Console.WriteLine($"Fichier copié : {sourceFile}");
                }

                Console.WriteLine("La sauvegarde différentielle est terminée.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la sauvegarde différentielle : {ex.Message}");
            }
        }

        private static bool IsSameContent(string file1, string file2)
        {
            using (var hash1 = MD5.Create())
            using (var hash2 = MD5.Create())
            using (var stream1 = File.OpenRead(file1))
            using (var stream2 = File.OpenRead(file2))
            {
                byte[] hashBytes1 = hash1.ComputeHash(stream1);
                byte[] hashBytes2 = hash2.ComputeHash(stream2);

                return StructuralComparisons.StructuralEqualityComparer.Equals(hashBytes1, hashBytes2);
            }
        }
    }
}
