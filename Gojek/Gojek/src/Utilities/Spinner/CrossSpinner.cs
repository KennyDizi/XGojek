using System;
using Gojek.Utilities.DPServices;
using Xamarin.Essentials;

namespace Gojek.Utilities.Spinner
{
    public sealed class CrossSpinner : ICrossSpinner
    {
        #region constructor

        private static readonly Lazy<CrossSpinner> Lazy =
            new Lazy<CrossSpinner>(() => new CrossSpinner());

        public static ICrossSpinner Instance => Lazy.Value;

        private CrossSpinner()
        {
        }

        #endregion

        public void ShowLoadingOverlay(string loadingString = null, bool needTimeout = true, Action timeoutAction = null)
        {
            //make sure invoke on main thread
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (string.IsNullOrEmpty(loadingString))
                    loadingString = "Processing";

                CrossMethods.Current.ShowSharedLoading(loadingString: loadingString, needTimeout: needTimeout);
            });
        }

        public void HideLoadingOverlay(string hideFromContext)
        {
            System.Diagnostics.Debug.WriteLine($"hide loading from: {hideFromContext}");
            //make sure invoke on main thread
            MainThread.InvokeOnMainThreadAsync(() => { CrossMethods.Current.HideShareLoading(); });
        }

        public void ShowShareSuccess(string successString)
        {
            //make sure invoke on main thread
            MainThread.InvokeOnMainThreadAsync(() => { CrossMethods.Current.ShowShareSuccess(successString); });
        }

        public void ShowSharedErroer(string errorString)
        {
            //make sure invoke on main thread
            MainThread.InvokeOnMainThreadAsync(() => { CrossMethods.Current.ShowSharedError(errorString); });
        }
    }
}