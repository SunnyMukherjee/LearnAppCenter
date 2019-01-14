using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace GoGoGiphy.Core.Services
{
    public class BaseService
    {
        #region Members

        internal string _filename = "SqlLiteDb.db3";
        internal string _filePath;
        public static SQLiteAsyncConnection sqliteAsyncConnection;
        public static SQLiteConnection sqliteConnection;

        #endregion


        #region Constructors

        public BaseService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, _filename);
        }

        #endregion 
    }
}
