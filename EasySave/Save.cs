﻿namespace EasySave
{
	public enum SaveType
	{
		COMPLETE,
		DIFFERENTIAL
	}

	public enum SaveState
	{
        NOT_STARTED,
        IN_PROGRESS,
        FINISHED,
		ERROR
    }

	public class Save
	{
		private static int _idCounter = 0;
		public int Id { get; }
		public string Name { get; set; }
		public string InputFolder { get; set; }
		public string OutputFolder { get; set; }
		public SaveType SaveType { get; set; }
		public int TotalFilesToCopy { get; set; }
		public int TotalFilesSize { get; set; }
		public int NbFilesLeftToDo { get; set; }
		public SaveState State { get; set; }

        /// <summary>
        /// Constructeur de la classe Save
        /// </summary>
        /// <param name="id">Identifiant de la sauvegarde</param>
        /// <param name="name">Nom de la sauvegarde</param>
        /// <param name="inputFolder">Dossier source</param>
        /// <param name="outputFolder">Dossier de destination</param>
        /// <param name="saveType">Type de sauvegarde</param>
        public Save(int? id, string name, string inputFolder, string outputFolder, SaveType saveType)
		{
			if (id == null)
			{
				_idCounter++;
			} else if (id > _idCounter)
			{
                _idCounter = (int)id;
            }

			Id = id ?? _idCounter;
			Name = name;
			InputFolder = inputFolder;
			OutputFolder = outputFolder;
			SaveType = saveType;
		}

        public override string ToString()
        {
			return $"\"{Name}\" (id: {Id}): \"{InputFolder}\" -> \"{OutputFolder}\" - {SaveType}";
		}
    }
}