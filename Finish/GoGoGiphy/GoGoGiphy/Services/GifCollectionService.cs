using GoGoGiphy.Core.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoGoGiphy.Core.Services
{
    public class GifCollectionService : BaseService
    {
        public GifCollectionService()
        {
            sqliteAsyncConnection = new SQLiteAsyncConnection(_filePath);
            sqliteAsyncConnection.CreateTableAsync<GifCollectionItem>().Wait();
        }

        public void DeleteGifCollection(GifCollectionItem gifCollectionItem)
        {
            sqliteConnection.Delete(gifCollectionItem);
        }

        public GifCollectionItem GetGifCollectionItem(string collectionItemName)
        {
            return sqliteConnection.Table<GifCollectionItem>().Where((item) => item.Name == collectionItemName).FirstOrDefault();
        }

        public GifCollectionItem GetGifCollectionItem(int id)
        {
            return sqliteConnection.Table<GifCollectionItem>().Where((item) => item.Id == id).FirstOrDefault();
        }

        public async Task<List<GifCollectionItem>> GetGifCollections()
        {
            List<GifCollectionItem> gifCollections = await sqliteAsyncConnection.Table<GifCollectionItem>().ToListAsync();
            return gifCollections;
        }

        public int GetLastGifCollectionItemId()
        {
            int id = sqliteConnection.ExecuteScalar<int>("select count(*) from GifCollectionItem");
            return id;
        }

        public void InsertGifCollectionItem(GifCollectionItem gifCollectionItem)
        {
            sqliteConnection.Insert(gifCollectionItem);
        }

        public void UpdateGifCollectionItem(GifCollectionItem gifCollectionItem)
        {
            sqliteConnection.Update(gifCollectionItem);
        }
    }
}
