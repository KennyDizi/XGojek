using System;
using Autofac;
using Gojek.src.Services.DialogServices;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;

namespace Gojek.src.Services.NavigationService
{
    public class CrossMvvmModule : Module
    {
        public CrossMvvmModule()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// register ioc modules
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            // standard Xamarin.Forms INavigation
            // remember to always use this as a deferred parameter in c'tor injection (Lazy<INavigation>)           
            //this is flexible with multi Application MainPage
            //this - can't SingleInstance if app containt multi type of Appication MainPage
            builder.Register(x => PageExs.GetCurrentContentPage().Navigation);

            //XNavigator - can't SingleInstance if app containt multi type of Appication MainPage
            builder.RegisterType<CrossNavigator>()
                .As<ICrossNavigator>();

            //CrossNamingConventions
            builder.RegisterType<CrossNamingConventions>()
                .As<ICrossNamingConventions>()
                .SingleInstance();

            //XViewFactory
            builder.RegisterType<CrossViewFactory>()
                .As<ICrossViewFactory>()
                .SingleInstance();

            //Func<Page> - always can't SingleInstance
            builder.RegisterInstance<Func<Page>>(PageExs.GetCurrentContentPage);

            //DialogService
            builder.RegisterType<CrossDialogProvider>()
                .As<ICrossDialogProvider>()
                .SingleInstance();

            //ActivationForViewFetcher
            builder.RegisterType<ActivationForViewFetcher>()
                .As<IActivationForViewFetcher>()
                .SingleInstance();
        }
    }
}