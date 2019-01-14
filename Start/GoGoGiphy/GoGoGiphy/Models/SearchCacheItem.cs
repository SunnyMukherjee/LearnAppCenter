using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoGoGiphy.Core.Models
{
    [Table("SearchCache")]
    public class SearchCacheItem
    {
        public SearchCacheItem()
        {

        }

        public SearchCacheItem(string searchString = "")
        {
            SearchString = searchString;
            TimeSearched = DateTime.Now;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
       
        public string SearchString { get; set; }

        public DateTime TimeSearched { get; set; }
    }
}
