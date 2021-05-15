using Autofac;
using Gojek.Services.NavigationService;
using Gojek.Utilities;
using Gojek.Views.HomePage;
using Plugin.SharedTransitions;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Gojek
{
    public partial class GojekApp
    {
        public GojekApp()
        {
            InitializeComponent();
            //init ioc
            CrossIoCDefine.Init();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                "NDQ1NDU5QDMxMzkyZTMxMmUzMEZadHNnWTJyTkxlVkVrMGZadVpFczBkL1FBK0hwODNlUnlicVltVUIveDg9");
            Sharpnado.MaterialFrame.Initializer.Initialize(loggerEnable: false, debugLogEnable: false);

            // disables accessibility scaling for named font sizes
            this.On<Xamarin.Forms.PlatformConfiguration.iOS>()
                .SetHandleControlUpdatesOnMainThread(true)
                .SetEnableAccessibilityScalingForNamedFontSizes(false);

            // simulate appearing/disppering look like ios
            this.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .SendAppearingEventOnResume(value: false)
                .SendDisappearingEventOnPause(value: false)
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize)
                .ShouldPreserveKeyboardOnResume(value: true);

            var factory = CrossIoCDefine.CrossIocContainer.Resolve<ICrossViewFactory>();
            var introPage = factory.ResolvePage<GojekV2HomePageViewModel>(CrossPageKeys.GojekV2HomePage.ToString());

            MainPage = new SharedTransitionNavigationPage(introPage);
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