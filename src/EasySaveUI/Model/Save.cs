﻿using System.Diagnostics;
using System.Text.Json.Serialization;

namespace EasySaveUI.Model
{
    public enum SaveType
    {
        COMPLETE,
        DIFFERENTIAL
    }

    public enum SaveState
    {
        ERROR = -1,
        NOT_STARTED = 0,
        IN_PROGRESS = 1,
        FINISHED = 2,
        STOPPED = 3,
        PAUSED = 4
    }

    public class Save : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InputFolder { get; set; }
        public string OutputFolder { get; set; }
        public SaveType SaveType { get; set; }
        [JsonIgnore]
        private int _totalFilesToCopy { get; set; } = 0;
        [JsonIgnore]
        public int TotalFilesSize { get; set; }
        [JsonIgnore]
        public int NbFilesLeftToDo { get; set; }
        // Progression comprise entre 0 et 1
        [JsonIgnore]
        private double _progress { get; set; }
        [JsonIgnore]
        private SaveState _state { get; set; }
        [JsonIgnore]
        public string _imageSource { get; set; } = "pause.png";
        [JsonIgnore]
        private bool _isSelected = false;

        /// <summary>
        /// Constructeur de la classe Save
        /// </summary>
        /// <param name="id">Identifiant de la sauvegarde</param>
        /// <param name="name">Nom de la sauvegarde</param>
        /// <param name="inputFolder">Dossier source</param>
        /// <param name="outputFolder">Dossier de destination</param>
        /// <param name="saveType">Type de sauvegarde</param>
        public Save(int id, string name, string inputFolder, string outputFolder, SaveType saveType = SaveType.COMPLETE)
        {

            Id = id ;
            Name = name ?? $"Save_{Id}";
            InputFolder = inputFolder ?? "";
            OutputFolder = outputFolder ?? "";
            SaveType = saveType;
        }

        public Save()
        {
        }

        /// <summary>
        /// Getter et Setter de l'attribut _totalFilesToCopy
        /// Permet également de notifier le changement de valeur de l'attribut à la vue
        /// </summary>
        public int TotalFilesToCopy
        {
            get => _totalFilesToCopy;
            set => SetProperty(_totalFilesToCopy, value, this, (save, val) => save._totalFilesToCopy = val);
        }

        /// <summary>
        /// Getter et Setter de l'attribut _progress
        /// Permet également de notifier le changement de valeur de l'attribut à la vue
        /// </summary>
        public double Progress
        {
            get => _progress;
            set => SetProperty(_progress, value, this, (save, val) => save._progress = val);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(_isSelected, value, this, (save, val) => save._isSelected = val);
        }

        public SaveState State
        {
            get => _state;
            set {
                SetProperty(_state, value, this, (save, val) => save._state = val);

                if (value == SaveState.PAUSED)
                    ImageSource = "play.png";
                else
                    ImageSource = "pause.png";
            }
        }

        public string ImageSource
        {
            get => _imageSource;
            set => SetProperty(_imageSource, value, this, (save, val) => save._imageSource = val);
        }

        public override string ToString()
        {
            return $"\"{Name}\" (id: {Id}): \"{InputFolder}\" -> \"{OutputFolder}\" - {SaveType}";
        }
    }
}
