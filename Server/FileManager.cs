using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    static class FileManager
    {
        public static string GetDataFileAsync(string filePath = "cards.txt")
        {
            if (!File.Exists("cards.txt"))
                File.Create("cards.txt").Close();
            string textFromFile = "";
            FileStream fstream = null;
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    textFromFile = reader.ReadToEnd();                    
                }
            }
            catch (Exception ex)
            {
                textFromFile = null;
            }
            finally
            {
                fstream?.Close();
            }

            return textFromFile;
        }

        public static bool WriteDataFile(string data, string filePath = "cards.txt")
        {
            if (!File.Exists("cards.txt"))
                File.Create("cards.txt").Close();
            bool result = true;
            FileStream fstream = null;
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.Write(data);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                fstream?.Close();
            }
            return result;
        }
    }
}
