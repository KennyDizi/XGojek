using Gojek.Views.HomePage;
using Xamarin.Forms;

namespace Gojek
{
    public partial class GojekApp : Application
    {
        public GojekApp()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("xxx");
            MainPage = new NavigationPage(new GojekHomePageView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}