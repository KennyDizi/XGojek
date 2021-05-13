using Gojek.Views.HomePage;
using Xamarin.Forms;

namespace Gojek
{
    public partial class GojekApp
    {
        public GojekApp()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDQ1NDU5QDMxMzkyZTMxMmUzMEZadHNnWTJyTkxlVkVrMGZadVpFczBkL1FBK0hwODNlUnlicVltVUIveDg9");
            Sharpnado.MaterialFrame.Initializer.Initialize(loggerEnable: false, debugLogEnable: false);
            MainPage = new NavigationPage(new GojekV2HomePageView());
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