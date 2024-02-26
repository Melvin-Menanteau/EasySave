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
            DateTime StartTime = DateTime.Now;

            string InputFilePath = args[0];
            string OutputFilePath = args[1];
            string KeyFile = "C:\\Users\\pierr\\Documents\\GitHub\\projet-programmation-syst-me-groupe-4\\Cryptosoft\\bin\\Release\\net8.0\\win-x64\\publish\\EncryptionKey.txt";

            byte[] message = FileReader.ReadFile(InputFilePath); // Message à chiffrer
            byte[] key = FileReader.ReadFile(KeyFile); // Clé de chiffrement

            if (key != null)
            {
                byte[] EncryptDecryptMessage = XorEncrypt.EncryptDecrypt(message, key);

                DateTime EndTimeEncryption = DateTime.Now;

                TimeSpan DurationEncrypt = EndTimeEncryption - StartTime;

                FileReader.WriteFile(OutputFilePath, EncryptDecryptMessage);

                if (DurationEncrypt > TimeSpan.Zero)
                {
                    Console.WriteLine(DurationEncrypt.ToString());
                }
                else
                {
                    Console.WriteLine("ECHEC");
                }
            }
        }
    }
}

