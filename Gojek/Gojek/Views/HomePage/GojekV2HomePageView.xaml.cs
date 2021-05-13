using System;
using System.Reactive.Disposables;
using ReactiveUI;

namespace Gojek.Views.HomePage
{
    public partial class GojekV2HomePageView
    {
        private readonly CompositeDisposable _compositeDisposable;

        public GojekV2HomePageView()
        {
            InitializeComponent();
            _compositeDisposable = new CompositeDisposable();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenAnyValue(view => view.Entry1.Text,
                    view => view.Entry2.Text)
                .Subscribe(values =>
                {
                    this.ButtonLogin.IsEnabled =
                        !string.IsNullOrEmpty(values.Item1) && !string.IsNullOrEmpty(values.Item2);
                });
        }

        protected override void OnDisappearing()
        {
            _compositeDisposable.Dispose();
            base.OnDisappearing();
        }
    }
}