using System;
using System.Collections.Generic;
using System.Text;

namespace Gojek.Utilities.DPServices
{
    public interface ICrossMethods
    {
        //share progress
        void ShowSharedLoading(string loadingString, bool needTimeout = true, Action timeoutAction = null);

        void HideShareLoading();

        void ShowShareSuccess(string successString);

        void ShowSharedError(string errorString);
    }
}