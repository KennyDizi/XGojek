using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Autofac;
using Gojek.Services.DialogServices;
using Gojek.ViewModels;
using Gojek.Views;
using ReactiveUI;
using Xamarin.Forms;

namespace Gojek.Services.NavigationService
{
    public class CrossViewFactory : ICrossViewFactory
    {
        #region Fields

        private readonly IComponentContext _componentContext;

        private readonly ICrossNamingConventions _namingConventions;

        #endregion

        #region Construction

        public CrossViewFactory()
        {
        }

        public CrossViewFactory(IComponentContext componentContext, ICrossNamingConventions namingConventions)
        {
            _componentContext = componentContext;
            _namingConventions = namingConventions;
        }

        #endregion

        public GojekBasePageView ResolvePage<T>(string name, NavigationParameters parameters = null)
            where T : GojekBasePageViewModel
        {
            var viewName = _namingConventions.GetViewName(name);
            if (!_componentContext.IsRegisteredWithName<GojekBasePageView>(viewName)) return null;

            var page = _componentContext.ResolveNamed<GojekBasePageView>(viewName);

            var viewModelName = _namingConventions.GetViewModelName(name);
            if (!_componentContext.IsRegisteredWithName<GojekBasePageViewModel>(viewModelName)) return page;
            var viewModel = _componentContext.ResolveNamed<GojekBasePageViewModel>(viewModelName);

            //assign current page navigator, dialoger
            viewModel.Navigator = _componentContext.Resolve<ICrossNavigator>();
            viewModel.Dialoger = _componentContext.Resolve<ICrossDialogProvider>();

            #region title

            //set an toan truong hop quen dat title cho mot trang nao do trong resource
            if (!string.IsNullOrEmpty(viewModel.Title))
            {
                viewModel.Title = viewModel.Title;
            }

            NavigationPage.SetHasNavigationBar(page, viewModel.HasNavigationBar);
            NavigationPage.SetHasBackButton(page, viewModel.HasBackButton);
            NavigationPage.SetBackButtonTitle(page, viewModel.BackButtonTitle);

            #endregion

            //call init before page appearing only if parameter not null
            if (parameters != null)
            {
                page.OnInitializeView(parameters: parameters);
                viewModel.OnInitializeViewModel(parameters: parameters);
            }

            //extension bind viewmodel => view (page)
            page.BindCrossPageViewModel<T>(viewModel);
            return page;
        }
    }

    public static class PageExts
    {
        /// <summary>
        /// extension bind viewmodel to view
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="pageViewModel"></param>
        public static void BindCrossPageViewModel<T>(this GojekBasePageView page,
            GojekBasePageViewModel pageViewModel) where T : GojekBasePageViewModel
        {
            //setup binding context
            page.BindingContext = pageViewModel;

            //setup rx viewmodel
            if (page is IViewFor<T> rxPage)
                rxPage.ViewModel = (T) pageViewModel;

            //call function setup RxUI
            page.SetupReactiveObservables();
            pageViewModel.SetupReactiveObservables();

            //page Appearing
            Observable.FromEventPattern<EventHandler, EventArgs>(
                    h => page.Appearing += h, h => page.Appearing -= h)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(async args =>
                {
                    //call viewmodel
                    await pageViewModel.OnAppearing();
                    page.ViewHasAppeared = true;
                }).DisposeWith(page._compositeDisposable);

            //page Disappearing
            Observable.FromEventPattern<EventHandler, EventArgs>(
                    h => page.Disappearing += h, h => page.Disappearing -= h)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(async args => { await pageViewModel.OnDisappearing(); })
                .DisposeWith(page._compositeDisposable);
        }
    }
}