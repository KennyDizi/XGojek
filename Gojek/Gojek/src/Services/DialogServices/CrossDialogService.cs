using System;

namespace Gojek.Services.DialogServices
{
    public class CrossDialogService : ICrossDialogService
    {
        public ICrossDialogProvider Dialoger { get; set; }

        #region constructor

        private static readonly Lazy<CrossDialogService> Lazy =
            new Lazy<CrossDialogService>(() => new CrossDialogService());

        public static ICrossDialogService Instance => Lazy.Value;

        private CrossDialogService()
        {
        }

        #endregion
    }
}