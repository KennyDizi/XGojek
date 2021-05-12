using Gojek.ViewModels;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;

namespace Gojek.Views
{
    /// <summary>
    /// This is an <see cref="ContentPage"/> that is also an <see cref="IViewFor{T}"/>.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="ReactiveUI.IViewFor{TViewModel}" />
    public class GojekBasePageView<TViewModel> : ReactiveContentPage<TViewModel>
        where TViewModel : GojekBasePageViewModel
    {
        public GojekBasePageView()
        {
        }
    }
}