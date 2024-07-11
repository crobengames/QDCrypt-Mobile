using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace QDCrypt.Models
{
    public static class QDAgreement
    {

        private const string FILENAME = "appData.json";

        public static bool UserHasAgreed()
        {
            string path = FileSystem.Current.AppDataDirectory;
            string fullPath = Path.Combine(path, FILENAME);

            if (File.Exists(fullPath))
            {
                QDAppData appDAta;

                try
                {
                    string  jsonString = File.ReadAllText(fullPath);
                    appDAta = JsonSerializer.Deserialize<QDAppData>(jsonString)!;
                }
                catch (Exception)
                {
                    DeleteUserAgreement(fullPath);
                    return false;
                }

                if (!appDAta.HasAgreed)     // User data is only saved with a true value. 
                {                           // So if this is triggered then the agreement has been tampered or corrupted.
                    DeleteUserAgreement(fullPath);
                    return false;
                }

                return true;
            }
        
            return false;
        }

        public static void Save() 
        {
            string path = FileSystem.Current.AppDataDirectory;
            string fullPath = Path.Combine(path, FILENAME);

            QDAppData appData = new()
            {
                HasAgreed = true
            };

            try
            {   
                // This serialization only works with class "properties" NOT fields!
                string jsonString = JsonSerializer.Serialize(appData);
                File.WriteAllText(fullPath, jsonString);
            }
            catch (Exception)
            {
                // Something was wrong!
            }   
        }

        public static void DeleteUserAgreement(string path)
        {
            // Just to be safe do a try catch.
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                // Doing nothing...
            }            
        }
    }
}
