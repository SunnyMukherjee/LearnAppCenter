using System;
using System.IO;
using GoGoGiphy.iOS.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileAccessHelper))]
namespace GoGoGiphy.iOS.Helpers
{
    /// <summary>
    /// Class works but is not used because of Xamarin.Essentials.FileSystem namespace.
    /// </summary>
    public class FileAccessHelper
    {
        internal static string _filename = "SqlLiteDb.db3";

        public static string GetLocalFilePath()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder  
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder  

            if (!Directory.Exists(libraryPath))
            {
                Directory.CreateDirectory(libraryPath);
            }

            string path = Path.Combine(libraryPath, _filename);
            return path;            
        }
    }
}