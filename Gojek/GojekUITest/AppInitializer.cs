using Xamarin.UITest;

namespace GojekUITest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.ApkFile("../../../XamTestApps/AGoNow/com.nowsolutions.gonow_enduser-Signed.apk").StartApp();
            }

            return ConfigureApp.iOS.AppBundle("../../../XamTestApps/iOS/com.nowsolutions.gonow_enduser.app").StartApp();
        }
    }
}