using System;
using System.Threading.Tasks;
using Gojek.src.Services.DialogServices;
using Gojek.src.Services.NavigationService;
using ReactiveUI;
using Xamarin.Forms;

namespace Gojek.ViewModels
{
    public class GojekBasePageViewModel : ReactiveObject, IConnectivityAware
    {
        public GojekBasePageViewModel()
        {
        }

        public string Title { get; set; }

        public bool HasNavigationBar { get; set; } = false;

        public bool HasBackButton { get; set; } = true;

        public string BackButtonTitle { get; set; } = string.Empty;

        public ICrossNavigator Navigator { get; set; }

        public ICrossDialogProvider Dialoger { get; set; }

        public virtual void OnInitializeViewModel(NavigationParameters parameters)
        {
        }

        public virtual void SetupReactiveObservables()
        {
        }
        
        public virtual Task OnDisappearing()
        {
            return Task.FromResult(0);
        }
        
        public virtual Task OnAppearing()
        {
            return Task.FromResult(0);
        }
        
        public virtual Task OnNavigatedBackTo(NavigationParameters parameters)
        {
            return Task.FromResult(0);
        }

        public virtual void OnConnected()
        {
        }

        public virtual void OnDisConnected()
        {
        }
    }
}