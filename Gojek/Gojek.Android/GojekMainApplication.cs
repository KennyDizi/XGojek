using Android.App;
using Android.Runtime;
using System;
using Android.OS;
using Plugin.CurrentActivity;
using Android.Gms.Security;

namespace Gojek.Droid
{
#if DEBUG
    [Application(Debuggable = true)]
#else
    [Application(Debuggable = false)]
#endif
    public class GojekMainApplication : Application
    {
        public GojekMainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);

            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                // Support TLS1.2 on Android versions before Lollipop
                ProviderInstaller.InstallIfNeeded(Application.Context);
            }
        }
    }
}