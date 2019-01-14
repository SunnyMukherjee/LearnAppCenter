using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace GoGoGiphy.UITest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class HomePageTests
    {
        IApp app;
        Platform platform;

        public HomePageTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        /// <summary>
        /// This UITest fails on purpose on iPhone devices to show the working example in the next UITest function below.
        /// </summary>
        [Test]
        public void TestSearchBroken()
        {
            app.EnterText("SearchBar", "Iron Man");
            app.PressEnter();

            Assert.IsTrue(app.Query("CachedImage").Any());
        }

        /// <summary>
        /// UITest function enters a search query and tests if it gets any results back.
        /// </summary>
        [Test]
        public void TestSearchWorking()
        {
            app.EnterText("SearchBar", "IronMan");
            app.PressEnter();
            app.WaitForElement("CachedImage", "The CachedImage control was not available in time", new TimeSpan(0, 0, 2, 0, 0));

            Assert.IsTrue(app.Query("CachedImage").Any());
        }
    }
}
