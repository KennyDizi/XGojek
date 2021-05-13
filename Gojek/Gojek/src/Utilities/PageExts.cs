using Gojek.ViewModels;
using Gojek.Views;

namespace Gojek.Utilities
{
    public static class PageExts
    {
        public static GojekBasePageView<TViewModel> CreateNewPage<TViewModel>(GojekBasePageView view, TViewModel viewModel)
            where TViewModel : GojekBasePageViewModel
        {
            var navigation = view.Navigation;
            viewModel.Navigation = navigation;
            //to do create autofac page view
        }
    }
}