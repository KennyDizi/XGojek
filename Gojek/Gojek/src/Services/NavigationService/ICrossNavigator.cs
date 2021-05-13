using System.Collections.Generic;
using System.Threading.Tasks;
using Gojek.ViewModels;
using Gojek.Views;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Gojek.Services.NavigationService
{
    public interface ICrossNavigator
    {
        IReadOnlyList<string> NavigationStack { get; }
        IReadOnlyList<string> ModalStack { get; }

        /// <summary>
        /// ResolvePage - resolve content page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        GojekBasePageView ResolvePage<T>(string name, NavigationParameters parameters = null)
            where T : GojekBasePageViewModel;

        /// <summary>
        /// remove page with page name
        /// </summary>
        /// <param name="name"></param>
        void RemovePage(string name);

        /// <summary>
        /// insert page before page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="nameBefore"></param>
        void InsertPageBefore<T>(string name, string nameBefore)
            where T : GojekBasePageViewModel;

        /// <summary>
        /// push a normal page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task PushAsync<T>(string name, bool animated,
            NavigationParameters parameters = null)
            where T : GojekBasePageViewModel;

        /// <summary>
        /// simple push content page
        /// </summary>
        /// <param name="contentPage"></param>
        /// <param name="animated"></param>
        /// <returns></returns>
        Task PushAsync(ContentPage contentPage, bool animated);

        /// <summary>
        /// pop normal page
        /// </summary>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task PopAsync(bool animated, NavigationParameters parameters = null);

        /// <summary>
        /// pop to root page of navigation page
        /// </summary>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task PopToRootAsync(bool animated, NavigationParameters parameters = null);

        /// <summary>
        /// push normal modal page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task PushModalAsync<T>(string name, bool animated,
            NavigationParameters parameters = null)
            where T : GojekBasePageViewModel;

        /// <summary>
        /// push an existing page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task PushModalAsync(ContentPage page);

        /// <summary>
        /// push navigation page as modal page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task PushModalAsNavPageAsync<T>(string name, bool animated,
            NavigationParameters parameters = null)
            where T : GojekBasePageViewModel;

        /// <summary>
        /// push normal page without IOC
        /// </summary>
        /// <param name="page"></param>
        /// <param name="animated"></param>
        /// <returns></returns>
        Task PushModalAsNavPageAsync(ContentPage page, bool animated);

        /// <summary>
        /// pop modal page
        /// </summary>
        /// <param name="animated"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task PopModalAsync(bool animated, NavigationParameters parameters = null);

        /// <summary>
        /// get modal stack count
        /// </summary>
        /// <returns></returns>
        int GetModalStackCount();

        #region Rg.Plugins.Popup

        /// <summary>
        /// Open new PopupPage
        /// </summary>
        /// <param name="page"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        Task PushPopupAsync(PopupPage page, bool animate);

        /// <summary>
        /// Hide last PopupPage
        /// </summary>
        /// <param name="animate"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task PopPopupAsync(bool animate, NavigationParameters parameters = null);

        #endregion
    }
}