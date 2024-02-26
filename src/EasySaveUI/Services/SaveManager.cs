using System.Diagnostics;

namespace EasySaveUI.Services
{
    public class SaveManager
    {
        private static SaveManager _instance;
        private readonly Dictionary<int, Thread> _runningSaves;
        private readonly object _lockRunningSave = new object();
        private readonly object _lockLargeFile = new object();
        //private readonly LoggerJournalier _loggerJournalier = new();
        //private readonly LoggerEtat _loggerEtat = new();

        private SaveManager()
        {
            _runningSaves = new Dictionary<int, Thread>();
        }


        /// <summary>
        /// Récupère l'instance de la classe
        /// </summary>
        public static SaveManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SaveManager();
            }

            return _instance;
        }

        /// <summary>
        /// Lance une sauvegarde dans un nouveau thread
        /// </summary>
        /// <param name="save">La sauvegarde à lancer</param>
        public void RunSave(Save save)
        {
            lock (_lockRunningSave)
            {
                if (_runningSaves.ContainsKey(save.Id))
                {
                    Debug.WriteLine($"La sauvegarde \"{save.Name}\" est déjà en cours d'exécution");
                    // TODO: Throw exception ?

                    return;
                }

                _runningSaves.Add(save.Id, new Thread(() => SaveThread(save)));
                _runningSaves[save.Id].Start();
            }
        }

        /// <summary>
        /// Arrête une sauvegarde et supprime le thread associé
        /// </summary>
        /// <param name="save">La sauvegarde à arrêter</param>
        public void StopSave(Save save)
        {
            lock (_lockRunningSave)
            {
                if (_runningSaves.ContainsKey(save.Id))
                {
                    _runningSaves.Remove(save.Id);
                }
            }
        }

        /// <summary>
        /// Arrête toutes les sauvegardes en cours
        /// </summary>
        public void StopAllSaves()
        {
            lock (_lockRunningSave)
            {
                foreach (var save in _runningSaves)
                {
                    save.Value.Abort();
                }
                _runningSaves.Clear();
            }
        }

        /// <summary>
        /// Fonction exécutée par un thread pour effectuer une sauvegarde
        /// </summary>
        /// <param name="save">La sauvegarde à effectuer</param>
        private void SaveThread(Save save)
        {
            UpdateSaveState(save, SaveState.IN_PROGRESS);

            List<string> filesToCopy = GetFilesToCopy(save.SaveType, save.InputFolder, save.OutputFolder);

            // Trier les fichiers par taille dans l'ordre croissant
            filesToCopy.Sort((string a, string b) => new FileInfo(a).Length.CompareTo(new FileInfo(b).Length));

            // Trier les fichiers par priorité
            List<string> ext = new List<string>() { "vue", "txt" };

            filesToCopy.Sort((string a, string b) =>
            {
                string extA = Path.GetExtension(a).TrimStart('.');
                string extB = Path.GetExtension(b).TrimStart('.');

                if (ext.Contains(extA) && !ext.Contains(extB))
                    return -1;
                else if (!ext.Contains(extA) && ext.Contains(extB))
                    return 1;
                else
                    return 0;
            });

            filesToCopy.ForEach((file) =>
            {
                bool islargeFile = new FileInfo(file).Length > 500_000;

                if (islargeFile)
                {
                    Monitor.Enter(_lockLargeFile);
                }

                try
                {
                    // TODO: Chiffrer le fichier si nécessaire
                    if (false)
                        EncryptFile(file, file.Replace(save.InputFolder, save.OutputFolder));
                    else
                        CopyFile(file, file.Replace(save.InputFolder, save.OutputFolder));
                } catch (Exception e)
                {
                    Debug.WriteLine($"Error while copying file {file}: {e.Message}");
                }
                finally {
                    // Liberer le lock si c'est un gros fichier
                    if (islargeFile)
                    {
                        Monitor.PulseAll(_lockLargeFile);
                        Monitor.Exit(_lockLargeFile);
                    }
                }
            });

            StopSave(save);
        }

        /// <summary>
        /// Récupère la liste des fichiers à copier pour une sauvegarde
        /// </summary>
        /// <param name="saveType">Le type de sauvegarde à effectuer</param>
        /// <param name="inputFolder">Le dossier source</param>
        /// <param name="outputFolder">Le dossier de destination</param>
        private List<string> GetFilesToCopy(SaveType saveType, string inputFolder, string? outputFolder)
        {
            List<string> filesToCopy = [];

            if (saveType == SaveType.COMPLETE)
            {
                Directory.GetFiles(inputFolder, "*", SearchOption.AllDirectories).ToList().ForEach(filesToCopy.Add);
            }
            else
            {
                Directory.GetFiles(inputFolder, "*", SearchOption.AllDirectories).ToList().ForEach((string file) => {
                    string outputLocation = file.Replace(inputFolder, outputFolder);

                    if (!File.Exists(file) || File.GetLastWriteTimeUtc(file) > File.GetLastWriteTimeUtc(outputLocation))
                    {
                        filesToCopy.Add(file);
                    }
                });
            }

            return filesToCopy;
        }

        /// <summary>
        /// Copie un fichier d'un emplacement à un autre
        /// </summary>
        /// <param name="inputFullPath">Le chemin complet du fichier source</param>
        /// <param name="outputFullPath">Le chemin complet du fichier de destination</param>
        private static void CopyFile(string inputFullPath, string outputFullPath)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(outputFullPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFullPath));
                }

                DateTime startTime = DateTime.Now;

                File.Copy(inputFullPath, outputFullPath, true);

                DateTime endTime = DateTime.Now;

                float transferTime = (float)(endTime - startTime).TotalMilliseconds;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error while copying file {inputFullPath} to {outputFullPath}: {e.Message}");
            }
        }

        /// <summary>
        /// Chiffre le fichier et le sauvegarde à l'emplacement spécifié.
        /// Le logiciel 'CryptoSoft' est utilisé pour chiffrer le fichier.
        /// </summary>
        /// <param name="inputFullPath">Le chemin complet du fichier source</param>
        /// <param name="outputFullPath">Le chemin complet du fichier de destination</param>
        private void EncryptFile(string inputFullPath, string outputFullPath)
        {
            DateTime startTime = DateTime.Now;

            // float encryptTime = appel cryptosoft

            DateTime endTime = DateTime.Now;
        }

        /// <summary>
        /// Change l'état d'une sauvegarde
        /// </summary>
        /// <param name="save">La sauvegarde à mettre à jour</param>
        /// <param name="state">Le nouvel état de la sauvegarde</param>
        private void UpdateSaveState(Save save, SaveState state)
        {
            // TODO: Update statut save dans le fichier JSON
            save.State = state;

            //_loggerEtat.WriteStatesToFile();
        }
    }
}
