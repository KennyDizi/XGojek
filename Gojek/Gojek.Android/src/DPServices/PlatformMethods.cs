using System;
using AndroidHUD;
using Gojek.Droid.DPServices;
using Gojek.Utilities.DPServices;
using Xamarin.Forms;

[assembly:Dependency(typeof(PlatformMethods))]

namespace Gojek.Droid.DPServices
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
                //action
                //Show a simple status message with an indeterminate spinner
                if (needTimeout)
                {
                    AndHUD.Shared.Show(XamForms.Activity, loadingString, -1, MaskType.Black,
                        TimeSpan.FromMilliseconds(600));
                    Device.StartTimer(TimeSpan.FromMilliseconds(600), () =>
                    {
                        timeoutAction?.Invoke();
                        return true;
                    });
                }
                else
                {
                    AndHUD.Shared.Show(XamForms.Activity, loadingString);
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"show loading error: {exception.Message}");
            }
        }

        public void HideShareLoading()
        {
            try
            {
                //Dismiss a HUD that will or will not be automatically timed out
                AndHUD.Shared.Dismiss(XamForms.Activity);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"show loading error: {exception.Message}");
            }
        }

        public void ShowShareSuccess(string successString)
        {
            try
            {
                AndHUD.Shared.ShowSuccessWithStatus(XamForms.Activity, successString, MaskType.Black,
                    TimeSpan.FromSeconds(2));
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"show toast error: {exception.Message}");
            }
        }

        public void ShowSharedError(string errorString)
        {
            try
            {
                AndHUD.Shared.ShowErrorWithStatus(XamForms.Activity, errorString, MaskType.Black,
                    TimeSpan.FromSeconds(2));
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"show toast error: {exception.Message}");
            }
        }
    }
}