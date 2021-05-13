using Autofac;
using Gojek.Services.NavigationService;

namespace Gojek.Utilities
{
    public static class CrossIoCDefine
    {
        public static void Init()
        {
            CrossIocContainer = InitIoC();
        }

        public static IContainer CrossIocContainer { get; private set; }

        /// <summary>
        /// InitIoC
        /// </summary>
        /// <returns></returns>
        private static IContainer InitIoC()
        {
            //init builder
            var builder = new ContainerBuilder();

            // Register MVVM framework.
            builder.RegisterModule<CrossMvvmModule>();

            // Register all views/pages and viewmodels that reside in this assembly.
            builder.RegisterModule<CrossViewsModule>();

            return builder.Build();
        }
    }
}