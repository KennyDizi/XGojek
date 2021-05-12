using Xamarin.Forms;

namespace Gojek
{
    public partial class GojekApp : Application
    {
        public GojekApp()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}