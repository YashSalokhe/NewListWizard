
namespace NewListWizard.Services
{
    public class EncryptDecryptService
    {
        public string Encrypt(string password)
        {
            string passwordToEncrypt = password;
            string toReturn = string.Empty;
            string publicKey = "12345678";
            string IV = "87654321";
            

            byte[] secretkeyByte = Encoding.UTF8.GetBytes(IV);

            byte[] publickeybyte = Encoding.UTF8.GetBytes(publicKey);

            byte[] inputbyteArray = Encoding.UTF8.GetBytes(passwordToEncrypt);
            DES des = DES.Create();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
            cs.Write(inputbyteArray, 0, inputbyteArray.Length);
            cs.FlushFinalBlock();
            toReturn = Convert.ToBase64String(ms.ToArray());

            return toReturn;

        }


        public string Decrypt(string password)
        {
            string passwordToDecrypt = password;
            string toReturn = string.Empty;
            string publickey = "12345678";
            string IV = "87654321";

            byte[] privatekeyByte = Encoding.UTF8.GetBytes(IV);

            byte[] publickeybyte = Encoding.UTF8.GetBytes(publickey);

            byte[] inputbyteArray = Convert.FromBase64String(passwordToDecrypt.Replace(" ", "+"));
            DES des = DES.Create();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
            cs.Write(inputbyteArray, 0, inputbyteArray.Length);
            cs.FlushFinalBlock();
            Encoding encoding = Encoding.UTF8;
            toReturn = encoding.GetString(ms.ToArray());

            return toReturn;
        }
    }
}
