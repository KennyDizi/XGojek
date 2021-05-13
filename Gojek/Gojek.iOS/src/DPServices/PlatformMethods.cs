using System;
using BigTed;
using Gojek.iOS.DPServices;
using Gojek.Utilities.DPServices;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformMethods))]

namespace Gojek.iOS.DPServices
{
    public class PlatformMethods : ICrossMethods
    {
        public PlatformMethods()
        {
        }

        public void ShowSharedLoading(string loadingString, bool needTimeout = true, Action timeoutAction = null)
        {
            try
            {
                //show spinner + text
                ProgressHUD.Shared.Show(status: loadingString, progress: -1,
                    maskType: ProgressHUD.MaskType.Gradient,
                    timeoutMs: 600);
                Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(600), () =>
                {
                    timeoutAction?.Invoke();
                    return true;
                });
            }
            catch (System.Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"cannot showloading with {exception.Message}");
            }
        }

        public void HideShareLoading()
        {
            BTProgressHUD.Dismiss(); //dissmiss loading
        }

        public void ShowShareSuccess(string successString)
        {
            var image = UIImage.FromBundle("tick.png");
            BTProgressHUD.ShowImage(image, successString, 3000);
        }

        public void ShowSharedError(string errorString)
        {
            var image = UIImage.FromBundle("warning.png");
            BTProgressHUD.ShowImage(image, errorString, 3000);
        }
    }
}