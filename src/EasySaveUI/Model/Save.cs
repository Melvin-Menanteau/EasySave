using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        FINISHED = 2
    }

    public class Save
    {
        private static int _idCounter = 0;
        public int Id { get; set; }
        public string Name { get; set; }
        public string InputFolder { get; set; }
        public string OutputFolder { get; set; }
        public SaveType SaveType { get; set; }
        [JsonIgnore]
        public int TotalFilesToCopy { get; set; }
        [JsonIgnore]
        public int TotalFilesSize { get; set; }
        [JsonIgnore]
        public int NbFilesLeftToDo { get; set; }
        [JsonIgnore]
        public SaveState State { get; set; }
        [JsonIgnore]
        public bool IsSelected = false;

        /// <summary>
        /// Constructeur de la classe Save
        /// </summary>
        /// <param name="id">Identifiant de la sauvegarde</param>
        /// <param name="name">Nom de la sauvegarde</param>
        /// <param name="inputFolder">Dossier source</param>
        /// <param name="outputFolder">Dossier de destination</param>
        /// <param name="saveType">Type de sauvegarde</param>
        public Save(int? id, string name, string inputFolder, string outputFolder, SaveType saveType = SaveType.COMPLETE)
        {
            if (id == null)
            {
                _idCounter++;
            }
            else if (id > _idCounter)
            {
                _idCounter = (int)id;
            }

            Id = id ?? _idCounter;
            Name = name ?? $"Save_{Id}";
            InputFolder = inputFolder ?? "";
            OutputFolder = outputFolder ?? "";
            SaveType = saveType;
        }

        public Save()
        {
        }

        public override string ToString()
        {
            return $"\"{Name}\" (id: {Id}): \"{InputFolder}\" -> \"{OutputFolder}\" - {SaveType}";
        }
    }
}
