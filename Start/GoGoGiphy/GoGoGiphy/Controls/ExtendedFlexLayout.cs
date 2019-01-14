using GoGoGiphy.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoGoGiphy.Core.Controls
{
    public class ExtendedFlexLayout : FlexLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ExtendedFlexLayout), propertyChanged: OnItemsSourceChanged);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ExtendedFlexLayout));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldVal, object newVal)
        {
            try
            {
                IEnumerable newValue = newVal as IEnumerable;
                var layout = (ExtendedFlexLayout)bindable;

                var observableCollection = newValue as INotifyCollectionChanged;
                if (observableCollection != null)
                {
                    observableCollection.CollectionChanged += layout.OnItemsSourceCollectionChanged;
                }

                layout.Children.Clear();
                if (newValue != null)
                {
                    foreach (var item in newValue)
                    {
                        layout.Children.Add(layout.CreateChildView(item));
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        View CreateChildView(object item)
        {
            try
            {
                if (ItemTemplate is DataTemplateSelector)
                {
                    var dts = ItemTemplate as DataTemplateSelector;
                    var itemTemplate = dts.SelectTemplate(item, null);
                    itemTemplate.SetValue(BindableObject.BindingContextProperty, item);
                    return (View)itemTemplate.CreateContent();
                }
                else
                {
                    ItemTemplate.SetValue(BindableObject.BindingContextProperty, item);
                    return (View)ItemTemplate.CreateContent();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }

            return null;
        }

        void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    // Items cleared
                    Children.Clear();
                }

                if (e.OldItems != null)
                {
                    Children.RemoveAt(e.OldStartingIndex);
                }

                if (e.NewItems != null)
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var item = e.NewItems[i];
                        var view = CreateChildView(item);
                        Children.Insert(e.NewStartingIndex + i, view);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
    }
}
