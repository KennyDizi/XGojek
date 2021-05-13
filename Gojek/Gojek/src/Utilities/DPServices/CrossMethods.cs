using System;

namespace Gojek.Utilities.DPServices
{
    public static class CrossMethods
    {
        private static readonly Lazy<ICrossMethods> Implementation = new Lazy<ICrossMethods>(
            () => Xamarin.Forms.DependencyService.Get<ICrossMethods>(),
            System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static ICrossMethods Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
#if DEBUG
                    throw NotImplementedInReferenceAssembly();
#endif
                }

                return ret;
            }
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException(
                "This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}