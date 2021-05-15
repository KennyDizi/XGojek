using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Sharpnado.MaterialFrame.iOS;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfRotator.XForms.iOS;
using Syncfusion.XForms.iOS.Shimmer;
using UIKit;

namespace Gojek.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("GojekAppDelegate")]
    public partial class GojekAppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            Syncfusion.XForms.iOS.Border.SfBorderRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfSegmentedControlRenderer.Init();
            Syncfusion.XForms.iOS.TextInputLayout.SfTextInputLayoutRenderer.Init();
            Xamarin.Forms.Nuke.FormsHandler.Init();
            iOSMaterialFrameRenderer.Init();
            new SfRotatorRenderer();
            SfListViewRenderer.Init();
            SfShimmerRenderer.Init();
            AppCenter.Start("f6118f93-dcd9-4c84-af00-0e391d14547e",
                typeof(Analytics), typeof(Crashes));

            LoadApplication(new GojekApp());
            return base.FinishedLaunching(app, options);
        }
    }
}