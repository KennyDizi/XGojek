using System.Threading.Tasks;

namespace Gojek.src.Services.NavigationService
{
    public interface INavigationAware
    {
        /// <summary>
        /// Called when navigation is performed to a page. You can use this method to load state if it is available.
        /// </summary>
        /// <param name="parameters"></param>
        /// e is page
        Task OnNavigatedBackTo(NavigationParameters parameters);
    }
}