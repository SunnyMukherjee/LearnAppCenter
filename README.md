# LearnAppCenter - GoGoGiphy

The sample app in this repository is called GoGoGiphy.

## <b>Purpose</b>

GoGoGiphy is a sample learning app used with the book, "Learn Visual Studio App Center with Xamarin Forms".  You can either fork this project or import this project into Azure DevOps.  I will show you those steps in my book.  

Since I will not be managing this app or fixing bugs going forward, please do not submit any pull requests.  

This app is used purely for training and demonstration purposes.  It is designed in the Xamarin Forms framework on the .NET Standard using C#.  

## <b>Project Layout</b>

You will find 2 different versions of the sample app: Start and Finish.  Both are functional.  The project in the Start folder has most of the working functions except those functions that interface with the App Center NuGet packages.  Those functions are left as empty stubs.  You can find the empty stubs in Visual Studio by searching for "TODO".  The working versions of the same functions can be found in the Finish folder.  Otherwise, both projects are identical.

## <b>Technologies</b>

### FreshMVVM

First and foremost, this app is designed using the FreshMvvm framework.  If you understand the Model-View-ViewModel design pattern, then you understand the eventual need of page navigation, use of the BindingContext from XAML pages, dependency injection, and inversion-of-control practices.  I will not go into great detail about each of these concepts, but I made use of the FreshMvvm framework because the author, Michael Ridland, has done a wonderful job creating an MVVM framework specifically for Xamarin Forms and made it easy to integrate in a Xamarin Forms app.  You can find more information at his GitHub page below.

https://github.com/rid00z/FreshMvvm

You can find more useful information on his Quick Start guide from his blog below.

https://michaelridland.com/xamarin/freshmvvm-quick-start-guide/


### Xamarin Essentials

The next big toolkit that I used in the app is Xamarin Essentials.  This toolkit gives a developer the power to use a single cross-platform API to tap into cross-platform specific features like Connectivity, Battery, Clipboard, etc. instead of having to learn each API separately for Android, iOS, and Windows.  You can find the GitHub page for Xamarin Essentials at the site below.  In this sample app, I do make use of the Connectivity, MainThread, and File System Helper API's.  If you decide to use my sample app as a base for your own app and if you come up with more ideas, you can leverage the cross-platform API's from Xamarin Essentials.  I encourage you to read through their GitHub page and documentation at the links below.

https://github.com/xamarin/Essentials
https://docs.microsoft.com/en-us/xamarin/essentials/

James Montemagno, who has been a major voice on the Xamarin Forms scene for a long time, is a major contributor among a team of other open-source contributors to Xamarin Essentials.  If you do not know much about him, you can always follow his blog below because he publishes a lot of relevant how-to articles relating to Xamarin Forms and other technologies.  He regularly publishes sample code from which you can learn.

https://montemagno.com/

### FFImageLoading

Another crucial library used in this sample app is the FFImageLoading library.  This library is crucial because it gives the app the ability to display gifs.  This library is available in a number of different frameworks like Xamarin.iOS, Xamarin.Android, and of course, Xamarin.Forms.  I personally love this library because you can simply plug and play the controls, modify a few settings, and never worry about it.  I encourage you to read more about this library as you browse through the code in the sample app.

https://github.com/luberda-molinet/FFImageLoading
https://github.com/luberda-molinet/FFImageLoading/wiki


### SQLite

Of course, these Gifs need to be stored somewhere locally so the user can save the images in collections for later viewing.  That is why I implemented SQLite into the Core project of the solution because it was easy to implement and easy to learn even for a beginner.  The NuGet package integrated into the Core project is the .NET wrapper package around the SQLite client.  You can find more information at the links below.

https://github.com/praeclarum/sqlite-net
https://www.nuget.org/packages/sqlite-net-pcl
https://www.sqlite.org/index.html

Lastly, the final important framework to learn about is Json.NET.  If you have ever developed either a website or a web API project or a mobile app, Javascript Object Notation (JSON) has become our payload-of-choice because it is easy to use, easy to read, and easy to serialize or deserialize into a .NET object.  If you are new to Json.NET, I encourage you to read through the documentation on their website below.

https://www.newtonsoft.com/json
https://www.newtonsoft.com/json/help/


## <b>How to set up the app to run locally</b>

-	Verify that your Xcode versions match between your Visual Studio on Windows and the Xcode version on your Mac.
-	Open Settings.cs in the GoGoGiphy.Core project.  In this class, you will find several static strings that are used throughout the code.  Two of the variables pertinent to this topic are AppCenterSecretiOS and AppCenterSecreAndroid.  We will need to replace the "iOSSecret" string with the actual App Secret value from App Center because this is how App Center identifies your app from the millions of other apps out there.

Follow these simple steps to get the App Secret value for iOS.

1.	Go to your iOS app in App Center.
2.	Go to the left-hand panel and click on Settings at the bottom.
3.	At the top right corner of the page, you will see a vertical dot icon button.  Click on the button.
4.	Click on "Copy App Secret".
5.	Paste and replace the value for "iOSSecret" in the AppCenterSecretiOS variable in the Settings.cs.

Follow these simple steps to get the App Secret value for Android.

1.	Go to your Android app in App Center.
2.	Go to the left-hand panel and click on Settings at the bottom.
3.	At the top right corner of the page, you will see a vertical dot icon button.  Click on the button.
4.	Click on "Copy App Secret".
5.	Paste and replace the value for "AndroidSecret" in the AppCenterSecretAndroid variable in the Settings.cs.

Follow these simple steps to set up your Giphy account.

1. Go to the following website, set up a Developer Giphy account, and go to your dashboard.

   https://developers.giphy.com

2. Set up a new app in your account.  You will find your app listed under "Your Apps".  And your API Key is given inside.  
3. Copy the value and replace the "GiphyApiKey" string value in the ApiKey variable in the Settings.cs.

**CAUTION**

It is unsafe to commit your App Secret and your Api Key values into source control by simply committing the changes in the Settings class as they currently exist.  You can use the steps described above to get the app to run locally on your iOS and Android simulators.  But I will show you in the book regarding what steps are needed to avoid committing secret values and api keys into source control and getting App Center to replace those values for you dynamically before each build.
