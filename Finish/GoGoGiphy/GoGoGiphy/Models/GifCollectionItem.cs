using GoGoGiphy.Core.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoGoGiphy.Core.Models
{
    [Table("GifCollectionItem")]
    public class GifCollectionItem
    {
        public GifCollectionItem()
        {

        }

        public GifCollectionItem(string collectionName)
        {
            Name = collectionName;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
