namespace CryptoSoft
{
    internal class FileReader
    {
        public static byte[] ReadFile(string filePath)
        {
            byte[]? key = null;

            try
            {
                // Vérifie si le fichier existe
                if (File.Exists(filePath))
                {
                    // Lecture du contenu du fichier
                    key = File.ReadAllBytes(filePath);
                }
                else
                {
                    Console.WriteLine("Le fichier n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }

            return key;
        }

        public static void WriteFile(string filePath, byte[] text)
        {
            try
            {
                // Écriture du texte dans le fichier
                File.WriteAllBytes(filePath, text);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }
        }
    }
}
