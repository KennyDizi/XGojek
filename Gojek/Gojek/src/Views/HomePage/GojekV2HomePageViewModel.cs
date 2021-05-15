using System.Threading.Tasks;
using System.Windows.Input;
using Gojek.ViewModels;
using ReactiveUI;
using Xamarin.Essentials;

namespace Gojek.Views.HomePage
{
    public class GojekV2HomePageViewModel : GojekBasePageViewModel
    {
        public GojekV2HomePageViewModel()
        {
            LoginCommand = ReactiveCommand.CreateFromTask(LoginTask, outputScheduler: RxApp.MainThreadScheduler);
        }

        public ICommand LoginCommand { get; }

        private async Task LoginTask()
        {
            var name = await MainThread.InvokeOnMainThreadAsync(async () => await this.Dialoger.DisplayPromptAsync("Hi", message: "What's your name?"));

            /*await this.Navigator.PushModalAsNavPageAsync(new GojekHomePageView(), animated: true);*/
        }

        private string _userName;

        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }
    }
}