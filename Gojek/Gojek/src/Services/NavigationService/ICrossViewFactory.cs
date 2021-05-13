using Gojek.ViewModels;
using Gojek.Views;

namespace Gojek.Services.NavigationService
{
    public interface ICrossViewFactory
    {
        /// <summary>
        /// ResolvePage - resolve content page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        GojekBasePageView ResolvePage<T>(string name, NavigationParameters parameters = null)
            where T : GojekBasePageViewModel;
    }
}