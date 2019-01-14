using GoGoGiphy.Core.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoGoGiphy.Core.Services
{
    public class SearchCacheService : BaseService
    {        
        public SearchCacheService()
        {            
            sqliteAsyncConnection = new SQLiteAsyncConnection(_filePath);
            sqliteAsyncConnection.CreateTableAsync<SearchCacheItem>().Wait();
        }

        /// <summary>
        /// Constructor overload is not used but kept as reference because of FileAccessHelper implementation.
        /// </summary>
        /// <param name="databasePath"></param>
        public SearchCacheService(string databasePath)
        {
            sqliteAsyncConnection = new SQLiteAsyncConnection(databasePath);
            sqliteAsyncConnection.CreateTableAsync<SearchCacheItem>().Wait();
        }

        public void DeleteSearchCache()
        {
            sqliteAsyncConnection.DeleteAllAsync<SearchCacheItem>();
        }       

        public async Task<List<SearchCacheItem>> GetSearchCache()
        {
            List<SearchCacheItem> searchCacheItems = await sqliteAsyncConnection.Table<SearchCacheItem>().ToListAsync();
            return searchCacheItems;
        }

        public void InsertSearchItem(SearchCacheItem searchCacheItem)
        {
            sqliteAsyncConnection.InsertAsync(searchCacheItem);
        }

        public void UpdateSearchItem(SearchCacheItem searchCacheItem)
        {
            sqliteAsyncConnection.UpdateAsync(searchCacheItem);
        }
    }
}
