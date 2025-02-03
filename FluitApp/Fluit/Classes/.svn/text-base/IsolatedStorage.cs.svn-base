using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Fluit.Classes
{
    public class IsolatedStorage
    {
        public enum FileName
        {
            RateApp = 0,
            HolesPressed = 1,
            Trialmode = 200,
        }

        /// <summary>
        /// Saves an integer to filesystem using filename
        /// </summary>
        /// <param name="fileNameKey"></param>
        /// <param name="value"></param>
        /// <returns>bool</returns>
        public static bool SaveIntToFile(FileName fileNameKey, int value)
        {
            String fileName = fileNameKey.ToString();
            using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var file = appStorage.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (var writer = new StreamWriter(file))
                    {
                        writer.Write(value);
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Read an int from given file
        /// </summary>
        /// <param name="fileNameKey"></param>
        /// <returns>-1 if file doesn't exist,                         
        /// InvalidCastException if it is not an int, 
        /// else a number</returns>
        public static int ReadIntFromFile(FileName fileNameKey)
        {
            String fileName = fileNameKey.ToString();
            using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (appStorage.FileExists(fileName))
                {
                    using (IsolatedStorageFileStream file = appStorage.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                    {
                        String text = new StreamReader(file).ReadToEnd();

                        int i;

                        if (int.TryParse(text, out i))
                        {
                            return i;
                        }
                        throw new InvalidCastException("Couldn't parse int: " + text);
                    }
                }
                return -1;
            }
        }
    }
}
