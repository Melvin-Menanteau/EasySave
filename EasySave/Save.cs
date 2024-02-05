using System;

public enum SaveType
{
    Complete,
    Differential
}

public class Save
{
	private static int idCounter = 0;
	private int id { get; };
	private string name { get; set; };
	private string inputFolder { get; set; };
	private string outputFolder { get; set; };
	private SaveType saveType { get; set; };

	public Save(int id = null, string name, string inputFolder, string outputFolder, SaveType saveType)
	{
		if (id == null)
		{
            idCounter++;
            id = idCounter;
        }
        else
		{
            id = id;

			if (id > idCounter)
                idCounter = id;
        }

		name = name;
		inputFolder = inputFolder;
		outputFolder = outputFolder;
		saveType = saveType;
	}
}
