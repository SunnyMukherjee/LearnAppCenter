using GoGoGiphy.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GoGoGiphy.Core.Services
{
    public class GifDataService : BaseService
    {
        #region Members



        #endregion


        #region Commands



        #endregion


        #region Constructors

        public GifDataService()
        {
            try
            {
                sqliteAsyncConnection = new SQLiteAsyncConnection(_filePath);
                sqliteConnection = new SQLiteConnection(_filePath);

                sqliteAsyncConnection.CreateTableAsync<GifDataItem>().Wait();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #endregion


        #region Functions

        public GifDataItem GetGifDataItem(GifDataItem gifDataItem, GifCollectionItem gifCollectionItem)
        {
            try
            {
                List<GifDataItem> existingItems = sqliteConnection.Query<GifDataItem>
                    ($"select * from GifDataItem where Id = '{gifDataItem.Id}' and GifCollectionId = '{gifCollectionItem.Id}'");

                return (existingItems.Count != 0) ? existingItems[0] : null;
                //return sqliteConnection.GetAsync<GifDataItem>(gifDataItem.Id).Result;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                throw;
            }
        }

        public async Task<List<GifDataItem>> GetGifDataItems(int collectionId)
        {      
            List<GifDataItem> gifDataItems = await sqliteAsyncConnection.QueryAsync<GifDataItem>
               ($"select * from GifDataItem where GifCollectionId = '{collectionId}'");

            return gifDataItems;
        }

        public void InsertGifDataItem(GifDataItem gifDataItem)
        {
            try
            {
                sqliteConnection.Insert(gifDataItem);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        public void DeleteGifDataItem(GifDataItem gifDataItem)
        {           
            sqliteConnection.Delete<GifDataItem>(gifDataItem.Id);
        }

        #endregion
    }
}
