namespace CryptoSoft
{
    internal class XorEncrypt
    {
        public static byte[] EncryptDecrypt(byte[] data, byte[] key)
        {
            byte[] encryptedData = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                // Application de l'opération XOR entre chaque octet du fichier et de la clé
                encryptedData[i] = (byte)(data[i] ^ key[i % key.Length]);
            }

            return encryptedData;
        }
    }
}
