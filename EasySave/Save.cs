using System;
using System.Runtime.InteropServices;

namespace EasySave
{
	public enum SaveType
	{
		COMPLETE,
		DIFFERENTIAL
	}

	public class Save
	{
		private static int idCounter = 0;
		public int Id { get; }
		public string Name { get; set; }
		public string InputFolder { get; set; }
		public string OutputFolder { get; set; }
		public SaveType SaveType { get; set; }

		public Save(string name, string inputFolder, string outputFolder, SaveType saveType, [Optional] int id)
		{
			if (id == null)
			{
				idCounter++;
				Id = idCounter;
			}
			else
			{
				Id = id;

				if (id > idCounter)
					idCounter = id;
			}

			Name = name;
			InputFolder = inputFolder;
			OutputFolder = outputFolder;
			SaveType = saveType;
		}

        public override string ToString()
        {
			return $"Save {Id} : {Name} - {InputFolder} -> {OutputFolder} - {SaveType}";
		}
    }
}