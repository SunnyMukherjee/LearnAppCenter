using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace GoGoGiphy.Core.Converters
{
    public class UrlToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string url = value as string;

            int imageDimension = (Device.RuntimePlatform == Device.iOS) || (Device.RuntimePlatform == Device.Android) ? 150 : 150;

            //string urlSuffix = String.Format("?width{0}&height={0}&mode=max", imageDimension);
            string urlSuffix = "";

            return url + urlSuffix;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
