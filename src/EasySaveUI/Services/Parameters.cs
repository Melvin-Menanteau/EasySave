using System.Reflection;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace EasySaveUI.Services
{
    public class Parameters
    {
        private static Parameters _instance;

        public List<string> EncryptionExstensionsList = [];
        public List<string> PriorityExtensionsList = [];
        public List<string> BusinessApplicationsList = [];
        public string EncryptionKey;
        public int MaxFileSize;

        private static string _extensionFileFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extensionFile.json");

        private Parameters()
        {
            ReadJsonEncryptionExtensionFile();
            ReadJsonBusinessApplicationsFile();
            ReadJsonPriorityExtensionFile();

            ReadMaxFileSize();
        }

        public static Parameters GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Parameters();
            }

            return _instance;
        }

        public void WriteEncryptionKey(string key)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string cryptosoftDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
            string keyFilePath = Path.Combine(cryptosoftDirectory, "Cryptosoft\\encryptionKey.txt");

            try
            {
                File.WriteAllText(keyFilePath, key);
                Console.WriteLine("Encryption key has been successfully written to the file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing encryption key: {ex.Message}");
            }
        }

        public string ReadEncryptionKey()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string cryptosoftDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
            string keyFilePath = Path.Combine(cryptosoftDirectory, "Cryptosoft\\encryptionKey.txt");

            try
            {
                if (File.Exists(keyFilePath))
                {
                    string key = System.IO.File.ReadAllText(keyFilePath);
                    return key;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading encryption key: {ex.Message}");
                return null;
            }
        }

        public void ReadJsonEncryptionExtensionFile()
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
            EncryptionExstensionsList = JsonSerializer.Deserialize<List<string>>(jsonContent);
        }

        public void WriteJsonEncryptionExtensionFile()
        {
            string jsonString = JsonSerializer.Serialize(EncryptionExstensionsList);
            FileStream file = new(_extensionFileFilePath, FileMode.Create);
            StreamWriter writer = new(file);

            writer.Write(jsonString);
            writer.Close();
            file.Close();
        }

        public void ReadJsonBusinessApplicationsFile()
        {
            string businessApplicationsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "businessApplications.json");

            if (!File.Exists(businessApplicationsFilePath))
            {
                FileStream file = new FileStream(businessApplicationsFilePath, FileMode.Create);
                StreamWriter writer = new StreamWriter(file);
                writer.Write("[]");
                writer.Close();
                file.Close();

                return;
            }

            string jsonContent = File.ReadAllText(businessApplicationsFilePath);
            BusinessApplicationsList = JsonSerializer.Deserialize<List<string>>(jsonContent);
        }

        public void WriteJsonBusinessApplicationsFile()
        {
            string businessApplicationsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "businessApplications.json");

            string jsonString = JsonSerializer.Serialize(BusinessApplicationsList);
            FileStream file = new(businessApplicationsFilePath, FileMode.Create);
            StreamWriter writer = new(file);

            writer.Write(jsonString);
            writer.Close();
            file.Close();
        }

        public void ReadJsonPriorityExtensionFile()
        {
            string priorityExtensionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "priorityExtension.json");

            if (!File.Exists(priorityExtensionFilePath))
            {
                FileStream file = new FileStream(priorityExtensionFilePath, FileMode.Create);
                StreamWriter writer = new StreamWriter(file);
                writer.Write("[]");
                writer.Close();
                file.Close();

                return;
            }

            string jsonContent = File.ReadAllText(priorityExtensionFilePath);
            PriorityExtensionsList = JsonSerializer.Deserialize<List<string>>(jsonContent);
        }

        public void WriteJsonPriorityExtensionFile()
        {
            string priorityExtensionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "priorityExtension.json");

            string jsonString = JsonSerializer.Serialize(PriorityExtensionsList);
            FileStream file = new(priorityExtensionFilePath, FileMode.Create);
            StreamWriter writer = new(file);

            writer.Write(jsonString);
            writer.Close();
            file.Close();
        }

        public void ReadMaxFileSize()
        {
            string maxFileSizeFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "maxFileSize.json");

            if (!File.Exists(maxFileSizeFilePath))
            {
                MaxFileSize = 0; // Default value
                return;
            }

            string jsonContent = File.ReadAllText(maxFileSizeFilePath);
            MaxFileSize = JsonSerializer.Deserialize<int>(jsonContent);
        }

        public void WriteMaxFileSize()
        {
            string maxFileSizeFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "maxFileSize.json");

            string jsonString = JsonSerializer.Serialize(MaxFileSize);
            File.WriteAllText(maxFileSizeFilePath, jsonString);
        }
    }
}
