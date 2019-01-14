using GoGoGiphy.Droid.Helpers;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileAccessHelper))]
namespace GoGoGiphy.Droid.Helpers
{
    /// <summary>
    /// Class works but is not used because of Xamarin.Essentials.FileSystem namespace.
    /// </summary>
    public class FileAccessHelper
    {
        internal static string _filename = "SqlLiteDb.db3";

        public static string GetLocalFilePath()
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentsPath, _filename);
            return path;
        }
    }
}
