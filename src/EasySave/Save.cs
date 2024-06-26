﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EasySave
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
            Id = id;
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