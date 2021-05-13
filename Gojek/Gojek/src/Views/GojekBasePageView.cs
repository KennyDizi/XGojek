using System.Reactive.Disposables;
using Gojek.src.Services.NavigationService;
using Gojek.ViewModels;
using ReactiveUI;
using Xamarin.Forms;

namespace Gojek.Views
{
    /// <summary>
    /// This is an <see cref="ContentPage"/> that is also an <see cref="IViewFor{T}"/>.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    /// <seealso cref="ReactiveUI.IViewFor{TViewModel}" />
    public class GojekBasePageView<TViewModel> : GojekBasePageView, IViewFor<TViewModel>
        where TViewModel : GojekBasePageViewModel
    {
        /// <summary>
        /// The view model bindable property.
        /// </summary>
        public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(
            nameof(ViewModel),
            typeof(TViewModel),
            typeof(GojekBasePageView<TViewModel>),
            default(TViewModel),
            BindingMode.OneWay,
            propertyChanged: OnViewModelChanged);

        /// <summary>
        /// Gets or sets the ViewModel to display.
        /// </summary>
        public TViewModel ViewModel
        {
            get => (TViewModel) GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        /// <inheritdoc/>
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel) value;
        }

        /// <inheritdoc/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            ViewModel = BindingContext as TViewModel;
        }

        private static void OnViewModelChanged(BindableObject bindableObject, object oldValue, object newValue) =>
            bindableObject.BindingContext = newValue;
    }

    public class GojekBasePageView : ContentPage
    {
        public readonly CompositeDisposable _compositeDisposable;
        public GojekBasePageView()
        {
            _compositeDisposable = new CompositeDisposable();
        }

        public virtual void OnInitializeView(NavigationParameters parameters)
        {
        }

        public virtual void SetupReactiveObservables()
        {
        }

        public bool ViewHasAppeared { get; set; } = false;
    }
}