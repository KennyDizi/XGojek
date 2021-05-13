using Android.App;
using Android.Runtime;
using Android.OS;

namespace Gojek.Droid
{
    [Activity(Label = "GoNow", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
                               Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.UiMode |
                               Android.Content.PM.ConfigChanges.ScreenLayout |
                               Android.Content.PM.ConfigChanges.SmallestScreenSize)]
    public class GojekMainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public GojekMainActivity()
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Android.Glide.Forms.Init(this);
            Rg.Plugins.Popup.Popup.Init(this);

            LoadApplication(new GojekApp());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }
    }
}