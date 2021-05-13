using Autofac;
using Gojek.ViewModels;
using Gojek.Views;

namespace Gojek.Services.NavigationService
{
    public class CrossViewsModule : Module
    {
        public CrossViewsModule()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers all Views (CrossNativeBasePageView) 
        /// and ViewModels (CrossNativeBasePageViewModel) that reside in this
        /// assembly such that that they are resolvable by name.
        /// </summary>
        /// <remarks>
        /// Views must derive from <see cref="T:CrossMobilePCL.SourceCode.CrossMobileShareUI.MaterialPage.NativeBasePage.CrossNativeBasePageView" />
        /// ViewModels must derive from <see cref="T:CrossMobilePCL.SourceCode.CrossMobileShareUI.MaterialPage.NativeBasePage.CrossNativeBasePageViewModel" />.<br />
        /// Per default, all classes are registered with transient lifetime scope. However, if the class should be
        /// registered as a singleton, you can use the <see cref="T:CrossMobilePCL.SourceCode.Services.NavigationService.ICrossSingletonLifetimeScope" /> marker interface.
        /// </remarks>
        /// <param name="builder">The Autofac ContainerBuilder which registers the module on app startup.</param>
        protected override void Load(ContainerBuilder builder)
        {
            // Views (Single Page)
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<GojekBasePageView>()
                .Where(v => !v.IsAssignableTo<ICrossSingletonLifetimeScope>())
                .Named<GojekBasePageView>(t => t.Name);

            // ViewModels
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<GojekBasePageViewModel>()
                .Where(v => !v.IsAssignableTo<ICrossSingletonLifetimeScope>())
                .Named<GojekBasePageViewModel>(t => t.Name);
        }
    }
}