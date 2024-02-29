using System.Diagnostics;

namespace EasySaveUI.Services
{
    public class SaveManager
    {
        private static SaveManager _instance;
        private readonly Dictionary<int, Thread> _runningSaves;
        private readonly Dictionary<int, ManualResetEvent> _runningSavesState;
        private readonly Dictionary<int, CancellationTokenSource> _runningSavesCancellation;
        private readonly object _lockRunningSave = new object();
        private readonly object _lockLargeFile = new object();
        private readonly Parameters _parameters = Parameters.GetInstance();
        private Barrier _barrier = new Barrier(0);
        private Dictionary<string, Thread> _BusinessObserversThreads;
        private readonly LoggerJournalier _loggerJournalier = LoggerJournalier.GetInstance();
        private readonly LoggerEtat _loggerEtat = LoggerEtat.GetInstance();

        private SaveManager()
        {
            _runningSaves = new Dictionary<int, Thread>();
            _runningSavesState = new Dictionary<int, ManualResetEvent>();
            _runningSavesCancellation = new Dictionary<int, CancellationTokenSource>();
            _BusinessObserversThreads = new Dictionary<string, Thread>();
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
                List<string> list_buisness_apps = _parameters.BusinessApplicationsList;
                for (int i = 0; i < list_buisness_apps.Count; i++)
                {
                    Debug.WriteLine(list_buisness_apps[i]);
                    Debug.WriteLine(i);
                    string appname = list_buisness_apps[i];
                    if (!_BusinessObserversThreads.ContainsKey(appname) && appname != null)
                    {
                        Thread thread = new Thread(new ThreadStart(() => BusinessObserver.Observer(appname)));
                        thread.Start();
                        _BusinessObserversThreads.Add(list_buisness_apps[i], thread);
                    }
                }
                if (_runningSaves.ContainsKey(save.Id))
                {
                    Debug.WriteLine($"La sauvegarde \"{save.Name}\" est déjà en cours d'exécution");
                    // TODO: Throw exception ?

                    return;
                }

                CancellationTokenSource cancellationToken = new CancellationTokenSource();

                _barrier.AddParticipant();

                _runningSaves.Add(save.Id, new Thread(() => SaveThread(save, cancellationToken.Token)));
                _runningSavesState.Add(save.Id, new ManualResetEvent(true));
                _runningSavesCancellation.Add(save.Id, cancellationToken);
                _runningSaves[save.Id].Start();
            }
        }

        /// <summary>
        /// Arrête une sauvegarde et supprime le thread associé
        /// </summary>
        /// <param name="save">La sauvegarde à arrêter</param>
        /// <param name="resetProgress">Indique si la progression de la sauvegarde doit être réinitialisée</param>
        public void StopSave(Save save, bool resetProgress = true)
        {
            lock (_lockRunningSave)
            {
                if (_runningSavesCancellation.TryGetValue(save.Id, out CancellationTokenSource c))
                {
                    c.Cancel();
                    c.Dispose();

                    _runningSaves.Remove(save.Id);
                    _runningSavesState.Remove(save.Id);
                    _runningSavesCancellation.Remove(save.Id);

                    _barrier.RemoveParticipant();
                    UpdateSaveState(save, SaveState.FINISHED);


                    if (resetProgress)
                    {
                        save.NbFilesLeftToDo = save.TotalFilesToCopy;
                        save.Progress = 0;
                    }
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
                _runningSavesCancellation.Values.ToList().ForEach(c => {
                    c.Cancel();
                    c.Dispose();
                });

                _runningSaves.Clear();
                _runningSavesState.Clear();
                _runningSavesCancellation.Clear();

                _barrier.RemoveParticipants(_runningSaves.Count);
            }
        }

        public void PauseAllSaves()
        {
            lock (_lockRunningSave)
            {
                SaveConfiguration saveConfiguration = SaveConfiguration.GetInstance();

                _runningSaves.Keys.ToList().ForEach((id) => PauseSave(saveConfiguration.GetConfiguration(id)));
            }
        }

        public void ResumeAllSaves()
        {
            lock (_lockRunningSave)
            {
                SaveConfiguration saveConfiguration = SaveConfiguration.GetInstance();

                _runningSaves.Keys.ToList().ForEach((id) => ResumeSave(saveConfiguration.GetConfiguration(id)));
            }
        }

        /// <summary>
        /// Fonction exécutée par un thread pour effectuer une sauvegarde
        /// </summary>
        /// <param name="save">La sauvegarde à effectuer</param>
        private void SaveThread(Save save, CancellationToken cancellationToken)
        {
            UpdateSaveState(save, SaveState.IN_PROGRESS);
            save.Progress = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                List<string> filesToCopy = GetFilesToCopy(save.SaveType, save.InputFolder, save.OutputFolder);

                save.TotalFilesToCopy = filesToCopy.Count;
                save.NbFilesLeftToDo = filesToCopy.Count;

                List<string> priority = filesToCopy.Where((file) => _parameters.PriorityExtensionsList.Contains(Path.GetExtension(file).TrimStart('.'))).ToList();
                List<string> nonPriority = filesToCopy.Except(priority).ToList();

                // Copier les fichiers prioritaires en premier
                foreach (string file in priority)
                {
                    HandleCopy(save, file);
                }

                _barrier.SignalAndWait();

                foreach (string file in nonPriority)
                {
                    HandleCopy(save, file);
                }

                // Si la sauvegarde est différentielle et qu'il n'y a pas de fichier à copier, la sauvegarde est terminée
                if (filesToCopy.Count == 0 && save.SaveType == SaveType.DIFFERENTIAL)
                {
                    save.Progress = 1;
                }

                StopSave(save, false);
            }
        }

        private void HandleCopy(Save save, string file)
        {
            Broker broker = Broker.GetInstance();

            try
            {
                // Il est possible que la sauvegarde est été annulée (stop), dans ce cas l'index ne sera plus disponible
                _runningSavesState[save.Id].WaitOne();
            } catch (Exception e)
            {
                return;
            }

            // Si l'utilisateur ne renseigne pas de taille maximale, on ne bloque pas la copie
            bool islargeFile = _parameters.MaxFileSize > 0 && new FileInfo(file).Length > _parameters.MaxFileSize;

            if (islargeFile)
            {
                Monitor.Enter(_lockLargeFile);
            }

            List<int?> times = [];

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(file.Replace(save.InputFolder, save.OutputFolder))))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(file.Replace(save.InputFolder, save.OutputFolder)));
                }

                if (_parameters.EncryptionExstensionsList.Contains(Path.GetExtension(file).TrimStart('.')))
                    times = EncryptFile(file, file.Replace(save.InputFolder, save.OutputFolder));
                else
                    times = CopyFile(file, file.Replace(save.InputFolder, save.OutputFolder));
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error while copying file {file}: {e.Message}");
            }
            finally
            {
                save.NbFilesLeftToDo--;
                save.Progress = ((save.TotalFilesToCopy - save.NbFilesLeftToDo) / (float)save.TotalFilesToCopy);

                int length = (int)new FileInfo(file).Length;
                using (Mutex m = new Mutex(false, "WriteProgress"))
                {
                    m.WaitOne();
                    broker.SendProgressToClient(save.Name, save.TotalFilesToCopy - save.NbFilesLeftToDo, save.TotalFilesToCopy);

                    _loggerJournalier.Log(save.Name, file, file.Replace(save.InputFolder, save.OutputFolder), length, times[0], times[1] != null ? times[1] : null);

                    m.ReleaseMutex();
                }

                // Liberer le lock si c'est un gros fichier
                if (islargeFile)
                {
                    Monitor.PulseAll(_lockLargeFile);
                    Monitor.Exit(_lockLargeFile);
                }
            }
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
        private static List<int?> CopyFile(string inputFullPath, string outputFullPath)
        {
            float transferTime = -1;
            int? encryptionTime = null;

            try
            {
                DateTime startTime = DateTime.Now;

                File.Copy(inputFullPath, outputFullPath, true);

                DateTime endTime = DateTime.Now;

                transferTime = (float)(endTime - startTime).TotalMilliseconds;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error while copying file {inputFullPath} to {outputFullPath}: {e.Message}");
                transferTime = -1;
            }
            return [(int)transferTime, encryptionTime];
        }

        /// <summary>
        /// Chiffre le fichier et le sauvegarde à l'emplacement spécifié.
        /// Le logiciel 'CryptoSoft' est utilisé pour chiffrer le fichier.
        /// </summary>
        /// <param name="inputFullPath">Le chemin complet du fichier source</param>
        /// <param name="outputFullPath">Le chemin complet du fichier de destination</param>
        private List<int?> EncryptFile(string inputFullPath, string outputFullPath)
        {
            DateTime StartTime = DateTime.Now;
            float transferTime = -1;
            int encryptionTime = -1;

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cryptosoft", "cryptosoft.exe"),
                    Arguments = $"\"{inputFullPath}\" \"{outputFullPath}\"", // Commande à exécuter
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process process = Process.Start(startInfo);

                int.TryParse(process.StandardOutput.ReadToEnd(), out encryptionTime);

                process.WaitForExit();
                process.Close();

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error while encrypting file {inputFullPath} to {outputFullPath}: {e.Message}");
                transferTime = -1;
                encryptionTime = -1;
            }

            TimeSpan DurationTotal = DateTime.Now - StartTime;
            return [(int)DurationTotal.TotalMilliseconds, encryptionTime];
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
            
            string state_string = "";
            if (state == SaveState.IN_PROGRESS)
                state_string = "En cours";
            else if (state == SaveState.PAUSED)
                state_string = "En pause";
            else if (state == SaveState.FINISHED)
                state_string = "Terminée";
            else if (state == SaveState.STOPPED)
                state_string = "Arrêtée";

            _loggerEtat.WriteStatesToFile();
            Broker broker = Broker.GetInstance();
            broker.SendStatusToClient(save.Name, state_string);
            Debug.WriteLine($"La sauvegarde \"{save.Name}\" est maintenant {state_string}");
        }

        /// <summary>
        /// Met en pause une sauvegarde
        /// </summary>
        /// <param name="save">La sauvegarde a mettre en pause</param>
        public void PauseSave(Save save)
        {
            if (_runningSavesState.TryGetValue(save.Id, out ManualResetEvent mre))
            {
                mre.Reset();

                // Il est possible de mettre en pause une sauvegarde de plusieurs manière
                // notamment par appui sur le bouton ou présence d'un logiciel métier.
                // Il se pourrait donc que le nombre de participants soit négatif.
                if (_barrier.ParticipantCount > 0)
                    _barrier.RemoveParticipant();

                UpdateSaveState(save, SaveState.PAUSED);
            }
        }

        /// <summary>
        /// Relance une sauvegarde en pause
        /// </summary>
        /// <param name="save">La sauvegarde a relancer</param>
        public void ResumeSave(Save save)
        {
            
            if (_runningSavesState.TryGetValue(save.Id, out ManualResetEvent mre))
            {
                mre.Set();
                _barrier.AddParticipant();
                UpdateSaveState(save, SaveState.IN_PROGRESS);
            }
        }
    }
}
