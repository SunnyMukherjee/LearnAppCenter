using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoGoGiphy.Core.Behaviors
{
    public class TextChangedBehavior : Behavior<SearchBar>
    {
        protected override void OnAttachedTo(SearchBar bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += Bindable_TextChanged;
        }

        protected override void OnDetachingFrom(SearchBar bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= Bindable_TextChanged;
        }

        private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = sender as SearchBar;

            if (searchBar != null)
            {
                searchBar.SearchCommand?.Execute(e.NewTextValue);
            }
        }
    }
}
