using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gojek.ViewModels;
using Gojek.Views;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Gojek.src.Services.NavigationService
{
    public class CrossNavigator : ICrossNavigator
    {
        #region Fields

        /// <summary>
        /// must Lazy to compatible with autofac
        /// </summary>
        private readonly Lazy<INavigation> _lazyNavigation;

        /// <summary>
        /// page factory
        /// </summary>
        private readonly ICrossViewFactory _viewFactory;

        /// <summary>
        /// process page name
        /// </summary>
        private readonly ICrossNamingConventions _namingConventions;

        #endregion

        #region Construction

        public CrossNavigator()
        {
        }

        /// <summary>
        /// autofac constructor
        /// </summary>
        /// <param name="lazyNavigation"></param>
        /// <param name="namingConventions"></param>
        /// <param name="viewFactory"></param>
        public CrossNavigator(Lazy<INavigation> lazyNavigation, ICrossNamingConventions namingConventions,
            ICrossViewFactory viewFactory)
        {
            _lazyNavigation = lazyNavigation;
            _namingConventions = namingConventions;
            _viewFactory = viewFactory;
        }

        #endregion

        #region Properties

        /// <summary>
        /// list page inside navigation page
        /// </summary>
        public IReadOnlyList<string> NavigationStack
        {
            get
            {
                return _lazyNavigation.Value.NavigationStack
                    .Select(page => _namingConventions.StripViewOrViewModelEnding(page.GetType().Name))
                    .ToList();
            }
        }

        /// <summary>
        /// navigation modal page
        /// </summary>
        public IReadOnlyList<string> ModalStack
        {
            get
            {
                return _lazyNavigation.Value.ModalStack
                    .Select(page => _namingConventions.StripViewOrViewModelEnding(page.GetType().Name))
                    .ToList();
            }
        }

        #endregion

        #region Operations

        /// <inheritdoc />
        /// <summary>
        /// ResolvePage - resolve content page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public GojekBasePageView ResolvePage<T>(string name, NavigationParameters parameters = null)
            where T : GojekBasePageViewModel
        {
            return _viewFactory.ResolvePage<T>(name, parameters);
        }

        /// <inheritdoc />
        /// <summary>
        /// remove page inside navigation stack
        /// </summary>
        /// <param name="name"></param>
        public void RemovePage(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
#if DEBUG || DB_SMHOME
                throw new ArgumentNullException(nameof(name));
#else
                return;
#endif
            }

            var pageName = _namingConventions.GetViewName(name);
            var page = _lazyNavigation.Value.NavigationStack
                .FirstOrDefault(pg => pg.GetType().Name == pageName);

            if (page != null)
            {
                _lazyNavigation.Value.RemovePage(page);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// insert new page before a page with name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="nameBefore"></param>
        public void InsertPageBefore<T>(string name, string nameBefore)
            where T : GojekBasePageViewModel
        {
            if (string.IsNullOrEmpty(name))
            {
#if DEBUG_PARTNER || DEBUG_USER
                throw new ArgumentNullException(nameof(name));
#else
                return;
#endif
            }

            if (string.IsNullOrEmpty(nameBefore))
            {
#if DB_SMHOME
                throw new ArgumentNullException(nameof(nameBefore));
#else
                return;
#endif
            }

            var pageNameBefore = _namingConventions.GetViewName(nameBefore);
            var page = _viewFactory.ResolvePage<T>(name);
            var pageBefore = _lazyNavigation.Value.NavigationStack
                .FirstOrDefault(pg => pg.GetType().Name == pageNameBefore);

            if (page == null || pageBefore == null) return;

            _lazyNavigation.Value.InsertPageBefore(page, pageBefore);
        }

        /// <inheritdoc />
        /// <summary>
        /// push new page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task PushAsync<T>(string name, bool animated, NavigationParameters parameters = null)
            where T : GojekBasePageViewModel
        {
            if (string.IsNullOrEmpty(name))
            {
#if DEBUG_PARTNER || DEBUG_USER
                throw new ArgumentNullException(nameof(name));
#else
                return;
#endif
            }

            //test optimize denied push a page multi time
            var lastPage = _lazyNavigation.Value.NavigationStack.LastOrDefault();
            if (lastPage != null)
            {
                var pageName = lastPage.ToString();
                System.Diagnostics.Debug.WriteLine($"Last page is {pageName}");
                if (pageName.Contains(name)) return;
            }

            var page = _viewFactory.ResolvePage<T>(name, parameters);
            if (page == null) return;

            await _lazyNavigation.Value.PushAsync(page, animated);
        }

        public async Task PushAsync(Page contentPage, bool animated)
        {
            //test optimize denied push a page multi time
            var lastPage = _lazyNavigation.Value.NavigationStack.LastOrDefault();
            if (lastPage != null)
            {
                var pageName = lastPage.ToString();
                System.Diagnostics.Debug.WriteLine($"Last page is {pageName}");
                if (pageName.Contains(nameof(contentPage))) return;
            }

            await _lazyNavigation.Value.PushAsync(contentPage, animated: animated);
        }

        /// <inheritdoc />
        /// <summary>
        /// pop to previous page
        /// </summary>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task PopAsync(bool animated, NavigationParameters parameters = null)
        {
            await _lazyNavigation.Value.PopAsync(animated);

            //back with param
            var curPage = PageExs.GetCurrentContentPage();
            await curPage.OnNavigatedBackTo(parameters);
        }

        /// <inheritdoc />
        /// <summary>
        /// pop to root page
        /// </summary>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task PopToRootAsync(bool animated, NavigationParameters parameters = null)
        {
            //pop to root
            await _lazyNavigation.Value.PopToRootAsync(animated);

            //back with param
            var curPage = PageExs.GetCurrentContentPage();
            await curPage.OnNavigatedBackTo(parameters);
        }

        /// <inheritdoc />
        /// <summary>
        /// push modal page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task PushModalAsync<T>(string name, bool animated, NavigationParameters parameters = null)
            where T : GojekBasePageViewModel
        {
            if (string.IsNullOrEmpty(name))
            {
#if DEBUG || DB_SMHOME
                throw new ArgumentNullException(nameof(name));
#else
                return;
#endif
            }

            //test optimize denied push a page multi time
            var lastPage = _lazyNavigation.Value.ModalStack.LastOrDefault();
            if (lastPage != null)
            {
                var pageName = lastPage.ToString();
                System.Diagnostics.Debug.WriteLine($"Last page is {pageName}");
                if (pageName.Contains(name)) return;
            }

            var page = _viewFactory.ResolvePage<T>(name, parameters);
            if (page == null) return;

            await _lazyNavigation.Value.PushModalAsync(page, animated);
        }

        /// <inheritdoc />
        /// <summary>
        /// push an existing page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task PushModalAsync(ContentPage page)
        {
            //test optimize denied push a page multi time
            var lastPage = _lazyNavigation.Value.ModalStack.LastOrDefault();
            if (lastPage != null)
            {
                var pageName = lastPage.ToString();
                System.Diagnostics.Debug.WriteLine($"Last page is {pageName}");
                if (pageName.Contains(nameof(page))) return;
            }

            await _lazyNavigation.Value.PushModalAsync(page, animated: true);
        }

        /// <inheritdoc />
        /// <summary>
        /// push a modal page as nav page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task PushModalAsNavPageAsync<T>(string name, bool animated, NavigationParameters parameters = null)
            where T : GojekBasePageViewModel
        {
            if (string.IsNullOrEmpty(name))
            {
#if DEBUG || DB_SMHOME
                throw new ArgumentNullException(nameof(name));
#else
                return;
#endif
            }

            //test optimize denied push a page multi time
            var lastPage = _lazyNavigation.Value.ModalStack.LastOrDefault();
            if (lastPage is NavigationPage navigationPage)
            {
                var curPage = navigationPage.CurrentPage;
                var pageName = curPage.ToString();
                System.Diagnostics.Debug.WriteLine($"Last page is {pageName}");
                if (pageName.Contains(name)) return;
            }

            var page = _viewFactory.ResolvePage<T>(name, parameters);
            if (page == null) return;

            var navPage = new NavigationPage(page);

            //push new page
            await _lazyNavigation.Value.PushModalAsync(navPage, animated);
        }

        /// <inheritdoc />
        /// <summary>
        /// Push Modal As NavigationPage Async
        /// </summary>
        /// <param name="page"></param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public async Task PushModalAsNavPageAsync(ContentPage page, bool animated)
        {
            //test optimize denied push a page multi time
            var lastPage = _lazyNavigation.Value.ModalStack.LastOrDefault();

            if (lastPage is NavigationPage navigationPage)
            {
                var curPage = navigationPage.CurrentPage;
                var pageName = curPage.ToString();
                System.Diagnostics.Debug.WriteLine($"Last page is {pageName}");
                if (pageName.Contains(nameof(page))) return;
            }

            var navPage = new NavigationPage(page);
            await _lazyNavigation.Value.PushModalAsync(navPage, animated: animated);
        }

        /// <inheritdoc />
        /// <summary>
        /// pop modal page
        /// </summary>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task PopModalAsync(bool animated, NavigationParameters parameters = null)
        {
            //neu khong co trang nao trong modal stack thi return
            if (!_lazyNavigation.Value.ModalStack.Any()) return;

            //tien hanh pop modal va truyen back param toi trang truoc
            await _lazyNavigation.Value.PopModalAsync(animated);

            //back with param
            var curPage = PageExs.GetCurrentContentPage();
            await curPage.OnNavigatedBackTo(parameters);
        }

        /// <summary>
        /// pop all modal page
        /// </summary>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task PopAllModalAsync(bool animated, NavigationParameters parameters = null)
        {
            //neu khong co trang nao trong modal stack thi return
            if (!_lazyNavigation.Value.ModalStack.Any()) return;

            var numberModalPage = _lazyNavigation.Value.ModalStack.Count;
            for (var i = 0; i < numberModalPage; i++)
            {
                //tien hanh pop modal
                await _lazyNavigation.Value.PopModalAsync(animated);
            }

            //back with param
            var curPage = PageExs.GetCurrentContentPage();
            await curPage.OnNavigatedBackTo(parameters);
        }

        /// <summary>
        /// pop all modal page V2
        /// </summary>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task PopAllModalAsyncV2(bool animated, NavigationParameters parameters = null)
        {
            //neu khong co trang nao trong modal stack thi return
            if (!_lazyNavigation.Value.ModalStack.Any()) return;

            var numberModalPage = _lazyNavigation.Value.ModalStack.Count;

            //tien hanh pop modal
            await _lazyNavigation.Value.PopModalAsync(animated);
            for (var i = 1; i < numberModalPage; i++)
                await _lazyNavigation.Value.PopModalAsync(false);

            //back with param
            var curPage = PageExs.GetCurrentContentPage();
            await curPage.OnNavigatedBackTo(parameters);
        }

        /// <inheritdoc />
        /// <summary>
        /// Get Modal Stack Count
        /// </summary>
        /// <returns></returns>
        public int GetModalStackCount()
        {
            //neu khong co trang nao trong modal stack thi return
            return !_lazyNavigation.Value.ModalStack.Any() ? 0 : _lazyNavigation.Value.ModalStack.Count;
        }

        #endregion

        #region Rg.Plugins.Popup

        public async Task PushPopupAsync(PopupPage page, bool animate)
        {
            await _lazyNavigation.Value.PushPopupAsync(page, animate);
        }

        public async Task PopPopupAsync(bool animate, NavigationParameters parameters = null)
        {
            //pop popup page
            await _lazyNavigation.Value.PopPopupAsync(animate);

            //back with param
            var curPage = PageExs.GetCurrentContentPage();
            await curPage.OnNavigatedBackTo(parameters);
        }

        #endregion
    }

    public static class PageExs
    {

        #region ContentPage

        /// <summary>
        /// get current content page
        /// </summary>
        /// <returns></returns>
        public static Page GetCurrentContentPage()
        {
            var mainPage = Application.Current.MainPage;
            switch (mainPage)
            {
                case CrossTransNavigationPage _:
                    return mainPage.As<CrossTransNavigationPage>().GetContentPageInsideNavigationPage();

                case CrossNavigationPage _:
                    return mainPage.As<CrossNavigationPage>().GetContentPageInsideNavigationPage();

                case CrossBottomTabbedPage _:
                    return mainPage.As<CrossBottomTabbedPage>().GetContentPageInsideTabbedPage();

                case CrossMasterDetailPage _:
                    return mainPage.As<CrossMasterDetailPage>().GetContentPageInsideMasterDetailPage();

                default:
                    throw new InvalidCastException("No page type detected");
            }
        }

        public static Page GetCurrentContentPage(Page mainPage)
        {
            switch (mainPage)
            {
                case CrossNavigationPage _:
                    return mainPage.As<CrossNavigationPage>().GetContentPageInsideNavigationPage();
                case CrossTransNavigationPage _:
                    return mainPage.As<CrossTransNavigationPage>().GetContentPageInsideNavigationPage();
                case CrossBottomTabbedPage _:
                    return mainPage.As<CrossBottomTabbedPage>().GetContentPageInsideTabbedPage();
                case CrossMasterDetailPage _:
                    return mainPage.As<CrossMasterDetailPage>().GetContentPageInsideMasterDetailPage();
                default:
                    return mainPage;
            }
        }

        public static CrossNativeBasePageView GetContentPageInsideMasterDetailPage(
            this CrossMasterDetailPage masterDetailPage)
        {
            CrossNativeBasePageView foundedPage = null;
            var curPageOfMasterDetail = masterDetailPage.Detail;
            switch (curPageOfMasterDetail)
            {
                case CrossNavigationPage _:
                    {
                        var navigationPage = curPageOfMasterDetail.As<CrossNavigationPage>();
                        foundedPage = navigationPage.GetContentPageInsideNavigationPage();
                    }
                    break;
                case CrossTransNavigationPage _:
                    {
                        var navigationPage = curPageOfMasterDetail.As<CrossTransNavigationPage>();
                        foundedPage = navigationPage.GetContentPageInsideNavigationPage();
                    }
                    break;
                case CrossBottomTabbedPage _:
                    {
                        var tabbedPage = curPageOfMasterDetail.As<CrossBottomTabbedPage>();
                        foundedPage = tabbedPage.GetContentPageInsideTabbedPage();
                    }
                    break;
            }

            return foundedPage;
        }

        public static CrossNativeBasePageView GetContentPageInsideTabbedPage(
            this CrossBottomTabbedPage bottomTabbedPage)
        {
            var curPageOfTabbedPage = bottomTabbedPage.CurrentPage;

            if (curPageOfTabbedPage is CrossNavigationPage)
            {
                var navPage = curPageOfTabbedPage.As<CrossNavigationPage>();
                var modalPage = navPage.CurrentPage.Navigation.ModalStack.LastOrDefault();
                var foundedPage = (modalPage is NavigationPage modalNavigationPage
                                      ? modalNavigationPage.CurrentPage
                                      : modalPage) ??
                                  navPage.CurrentPage;
                return foundedPage.As<CrossNativeBasePageView>();
            }
            else
            {
                var navPage = curPageOfTabbedPage is CrossTransNavigationPage
                    ? curPageOfTabbedPage.As<CrossTransNavigationPage>()
                    : curPageOfTabbedPage.As<CrossMasterDetailPage>().Detail.As<CrossTransNavigationPage>();
                var modalPage = navPage.CurrentPage.Navigation.ModalStack.LastOrDefault();
                var foundedPage = (modalPage is NavigationPage modalNavigationPage
                                      ? modalNavigationPage.CurrentPage
                                      : modalPage) ??
                                  navPage.CurrentPage;
                return foundedPage.As<CrossNativeBasePageView>();
            }
        }

        public static CrossNativeBasePageView GetContentPageInsideNavigationPage(
            this NavigationPage navigationPage)
        {
            var modalPage = navigationPage.CurrentPage.Navigation.ModalStack.LastOrDefault();
            var foundedPage = (modalPage is NavigationPage modalNavigationPage
                                  ? modalNavigationPage.CurrentPage
                                  : modalPage) ??
                              navigationPage.CurrentPage;
            return foundedPage.As<CrossNativeBasePageView>();
        }

        #endregion

        #region NaviagationPage

        public static CrossNavigationPage GetCurrentNavigationPage()
        {
            var mainPage = Application.Current.MainPage;
            switch (mainPage)
            {
                case CrossNavigationPage _:
                    return mainPage.As<CrossNavigationPage>().GetItSelfNavigationPage();
                case CrossBottomTabbedPage _:
                    return mainPage.As<CrossBottomTabbedPage>().GetNavPageInsideTabbedPage();
                case CrossMasterDetailPage _:
                    return mainPage.As<CrossMasterDetailPage>().GetNavPageInsideMasterDetailPage();
                default:
                    throw new InvalidCastException("No page type detected");
            }
        }

        private static CrossNavigationPage GetItSelfNavigationPage(
            this CrossNavigationPage navigationPage)
        {
            var modalPage = navigationPage.CurrentPage.Navigation.ModalStack.LastOrDefault();
            var foundedPage = modalPage is CrossNavigationPage modalNavigationPage
                ? modalNavigationPage
                : navigationPage;
            return foundedPage;
        }

        private static CrossNavigationPage GetNavPageInsideTabbedPage(
            this CrossBottomTabbedPage bottomTabbedPage)
        {
            var curPageOfTabbedPage = bottomTabbedPage.CurrentPage;
            var navPage = curPageOfTabbedPage is CrossNavigationPage
                ? curPageOfTabbedPage.As<CrossNavigationPage>()
                : curPageOfTabbedPage.As<CrossMasterDetailPage>().Detail.As<CrossNavigationPage>();
            var modalPage = navPage.CurrentPage.Navigation.ModalStack.LastOrDefault();
            return modalPage is CrossNavigationPage modalNavigationPage
                ? modalNavigationPage
                : navPage;
        }

        private static CrossNavigationPage GetNavPageInsideMasterDetailPage(
            this CrossMasterDetailPage masterDetailPage)
        {
            CrossNavigationPage foundedPage = null;
            var curPageOfMasterDetail = masterDetailPage.Detail;
            switch (curPageOfMasterDetail)
            {
                case CrossNavigationPage _:
                    var navigationPage = curPageOfMasterDetail.As<CrossNavigationPage>();
                    foundedPage = navigationPage.GetItSelfNavigationPage();
                    break;
                case CrossBottomTabbedPage _:
                    var tabbedPage = curPageOfMasterDetail.As<CrossBottomTabbedPage>();
                    foundedPage = tabbedPage.GetNavPageInsideTabbedPage();
                    break;
            }

            return foundedPage;
        }

        #endregion

        /// <summary>
        /// call OnNavigatedBackTo
        /// </summary>
        /// <param name="page"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task OnNavigatedBackTo(this Page page, NavigationParameters parameters = null)
        {
            //call on page
            var nativePage = page.As<CrossNativeBasePageView>();
            await nativePage.OnNavigatedBackTo(parameters: parameters);

            //call on vm
            var bindingContext = page.BindingContext.As<CrossNativeBasePageViewModel>();
            await bindingContext.OnNavigatedBackTo(parameters: parameters);
        }

        /// <summary>
        /// extension call OnConnected
        /// </summary>
        /// <param name="page"></param>
        public static void OnConnected(this Page page)
        {
            //call on page
            var nativePage = page.As<CrossNativeBasePageView>();
            nativePage?.OnConnected();

            var bindingContext = nativePage?.BindingContext.As<CrossNativeBasePageViewModel>();
            bindingContext?.OnConnected();
        }

        /// <summary>
        /// extension call OnDisConnected
        /// </summary>
        /// <param name="page"></param>
        public static void OnDisConnected(this Page page)
        {
            //call on page
            var nativePage = page.As<CrossNativeBasePageView>();
            nativePage?.OnDisConnected();

            var bindingContext = nativePage?.BindingContext.As<CrossNativeBasePageViewModel>();
            bindingContext?.OnDisConnected();
        }
    }
}
