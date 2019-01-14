using System;
using System.IO;
using Xamarin.UITest;
using Xamarin.UITest.Configuration;
using Xamarin.UITest.Queries;

namespace GoGoGiphy.UITest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                string androidPath = System.AppDomain.CurrentDomain.BaseDirectory.Replace("UITest", "Android");
                string filePath = Path.Combine(androidPath, "com.companyname.GoGoGiphy.apk");

                return ConfigureApp
                    .Android
                    .ApkFile(filePath)
                    .StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}