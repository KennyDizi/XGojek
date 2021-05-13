using System;

namespace Gojek.Utilities.Spinner
{
    public interface ICrossSpinner
    {
        /// <summary>
        /// show loading overlay
        /// </summary>
        /// <param name="loadingString"></param>
        /// <param name="needTimeout"></param>
        /// <param name="timeoutAction"></param>
        void ShowLoadingOverlay(string loadingString = null, bool needTimeout = true, Action timeoutAction = null);

        /// <summary>
        /// hide loading overlay
        /// </summary>
        /// <param name="hideFromContext"></param>
        void HideLoadingOverlay(string hideFromContext);

        /// <summary>
        /// show dialog success with big tick image
        /// </summary>
        /// <param name="successString"></param>
        void ShowShareSuccess(string successString);

        /// <summary>
        /// show dialog error with big error image
        /// </summary>
        /// <param name="errorString"></param>
        void ShowSharedErroer(string errorString);
    }
}