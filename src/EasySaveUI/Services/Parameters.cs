using System.Text.Json;

namespace EasySaveUI.Services
{
    public class Parameters
    {
        private static Parameters _instance;

        public List<string> ExtensionsList = [];

        private static string _extensionFileFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extensionFile.json");

        private Parameters()
        {
            ReadJsonExtensionFile();
        }

        public static Parameters GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Parameters();
            }

            return _instance;
        }

        public void ReadJsonExtensionFile()
        {
            if (!File.Exists(_extensionFileFilePath))
            {
                FileStream file = new FileStream(_extensionFileFilePath, FileMode.Create);
                StreamWriter writer = new StreamWriter(file);
                writer.Write("[]");
                writer.Close();
                file.Close();

                return;
            }

            string jsonContent = File.ReadAllText(_extensionFileFilePath);
            ExtensionsList = JsonSerializer.Deserialize<List<string>>(jsonContent);
        }

        public void WriteJsonExtensionFile()
        {
            string jsonString = JsonSerializer.Serialize(ExtensionsList);
            FileStream file = new(_extensionFileFilePath, FileMode.Create);
            StreamWriter writer = new(file);

            writer.Write(jsonString);
            writer.Close();
            file.Close();
        }
    }
}
