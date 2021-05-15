using System.Threading.Tasks;
using System.Windows.Input;
using Gojek.ViewModels;
using ReactiveUI;

namespace Gojek.Views.HomePage
{
    public class GojekV2HomePageViewModel : GojekBasePageViewModel
    {
        public GojekV2HomePageViewModel()
        {
            LoginCommand = ReactiveCommand.CreateFromTask(LoginTask);
        }

        public ICommand LoginCommand { get; }

        private async Task LoginTask()
        {
            await this.Navigator.PushModalAsNavPageAsync(new GojekHomePageView(), animated: true);
        }

        private string _userName;

        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }
    }
}