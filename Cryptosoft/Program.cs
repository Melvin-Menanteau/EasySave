using CryptoSoft;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Environment.Exit(-1);
        }
        else
        {
            DateTime StartDateTime = DateTime.Now;

            string InputFilePath = args[0];
            string OutputFilePath = args[1];
            string KeyFile = "C:\\Users\\pierr\\source\\repos\\Prosit5\\Prosit5\\Key\\key.txt";

            byte[] message = FileReader.ReadFile(InputFilePath); // Message à chiffrer
            byte[] key = FileReader.ReadFile(KeyFile); // Clé de chiffrement

            if (key != null)
            {
                byte[] EncryptDecryptMessage = XorEncrypt.EncryptDecrypt(message, key);

                FileReader.WriteFile(OutputFilePath, EncryptDecryptMessage);
            }

            TimeSpan Duration = DateTime.Now - StartDateTime;

            if (Duration > TimeSpan.Zero)
            {
                Console.WriteLine(Duration.ToString());
            }
            else
            {
                Console.WriteLine("ECHEC");
            }
        }
    }
}

