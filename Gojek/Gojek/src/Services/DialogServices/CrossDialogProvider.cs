using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Gojek.Services.NavigationService;
using Gojek.Utilities.Spinner;
using ReactiveUI;
using Xamarin.Forms;

namespace Gojek.Services.DialogServices
{
    public class CrossDialogProvider : ICrossDialogProvider
    {
        private readonly Func<ContentPage> _pageResolver;
        private readonly ICrossNavigator _navigator;

        public CrossDialogProvider()
        {
        }

        /// <summary>
        /// constructor DialogProvider with autofac
        /// </summary>
        /// <param name="pageResolver"></param>
        /// <param name="navigator"></param>
        public CrossDialogProvider(Func<ContentPage> pageResolver, ICrossNavigator navigator)
        {
            _pageResolver = pageResolver;
            _navigator = navigator;
        }

        /// <inheritdoc />
        /// <summary>
        /// standard DisplayAlert
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            //hide loading first
            CrossSpinner.Instance.HideLoadingOverlay("DialogProvider");

            //show dialog
            await _pageResolver().DisplayAlert(title, message, cancel);
        }

        /// <inheritdoc />
        /// <summary>
        /// standard DisplayAlert
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="accept"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            //hide loading first
            CrossSpinner.Instance.HideLoadingOverlay("DialogProvider");

            //show dialog
            return await _pageResolver().DisplayAlert(title, message, accept, cancel);
        }

        /// <summary>
        /// Display alert with InputText
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="accept"></param>
        /// <param name="cancel"></param>
        /// <param name="placeholder"></param>
        /// <param name="maxLength"></param>
        /// <param name="keyboard"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public async Task<string> DisplayPromptAsync(string title, string message, string accept = "OK",
            string cancel = "Cancel", string placeholder = null, int maxLength = -1, 
            Keyboard keyboard = default, string initialValue = "")
        {
            //hide loading first
            CrossSpinner.Instance.HideLoadingOverlay("DialogProvider");

            // small delay
            await Task.Delay(TimeSpan.FromMilliseconds(200));

            return await _pageResolver()
                .DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard, initialValue);
        }

        /// <inheritdoc />
        /// <summary>
        /// standard DisplayActionSheet
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cancel"></param>
        /// <param name="destruction"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction,
            params string[] buttons)
        {
            return await _pageResolver().DisplayActionSheet(title, cancel, destruction, buttons);
        }

        /// <inheritdoc />
        /// <summary>
        /// extended DisplayAlert with action
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="accept"></param>
        /// <param name="cancel"></param>
        /// <param name="actionAccept"></param>
        /// <param name="actionCancel"></param>
        /// <param name="actionAcceptParam"></param>
        /// <param name="actionCancelParam"></param>
        public async Task DisplayAlertEx(string title, string message, string accept, string cancel,
            Action<object> actionAccept = null, Action<object> actionCancel = null,
            object actionAcceptParam = null, object actionCancelParam = null)
        {
            //hide loading first
            CrossSpinner.Instance.HideLoadingOverlay("DialogProvider");

            //show dialog
            var result = await _pageResolver().DisplayAlert(title, message, accept, cancel);
            if (result)
            {
                actionAccept?.Invoke(actionAcceptParam);
            }
            else
            {
                actionCancel?.Invoke(actionCancelParam);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// extended DisplayAlert with action
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="accept"></param>
        /// <param name="actionAccept"></param>
        /// <param name="actionAcceptParam"></param>
        public async Task DisplayAlertEx(string title, string message, string accept,
            Action<object> actionAccept = null, object actionAcceptParam = null)
        {
            //hide loading first
            CrossSpinner.Instance.HideLoadingOverlay("DialogProvider");

            //show dialog
            await _pageResolver().DisplayAlert(title, message, accept);
            actionAccept?.Invoke(actionAcceptParam);
        }

        public async Task DisplayAlertEx(string title, string message, string accept, ICommand commandAccept)
        {
            //hide loading first
            CrossSpinner.Instance.HideLoadingOverlay("DialogProvider");

            //show dialog
            await _pageResolver().DisplayAlert(title, message, accept);

            //execute command
            commandAccept?.Execute($"force execute accept command with question: {message}");
        }

        public async Task DisplayAlertEx(string title, string message, string accept, string cancel,
            ICommand commandAccept, ICommand commandCancel = null)
        {
            //hide loading first
            CrossSpinner.Instance.HideLoadingOverlay("DialogProvider");

            //show dialog
            var anwser = await _pageResolver().DisplayAlert(title, message, accept, cancel);
            if (anwser)
                commandAccept?.Execute($"execute accept command with question: {message}");
            else
                commandCancel?.Execute($"execute cancel command with question: {message}");
        }

        /// <inheritdoc />
        /// <summary>
        /// extended DisplayActionSheet with command
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cancel"></param>
        /// <param name="destruction"></param>
        /// <param name="executeCommand"></param>
        /// <param name="buttons"></param>
        public async Task DisplayActionSheetEx(string title, string cancel, string destruction, 
            ICommand executeCommand, params string[] buttons)
        {
            var result = await _pageResolver().DisplayActionSheet(title, cancel, destruction, buttons);
            var index = buttons.ToList().IndexOf(result);
            if (index >= 0 && index <= buttons.Length - 1)
            {
                executeCommand?.Execute(index);
            }
            else if (!string.IsNullOrEmpty(destruction))
            {
                executeCommand?.Execute(-1);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// if you want use INavigation when use action sheet
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cancel"></param>
        /// <param name="destruction"></param>
        /// <param name="executeCommand"></param>
        /// <param name="needUseNavigator"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public async Task DisplayActionSheetExWitNav(string title, string cancel, string destruction,
            ICommand executeCommand, bool needUseNavigator, params string[] buttons)
        {
            var result = await _pageResolver().DisplayActionSheet(title, cancel, destruction, buttons);
            var index = buttons.ToList().IndexOf(result);
            if (index >= 0 && index <= buttons.Length - 1)
            {
                executeCommand?.Execute(new Tuple<int, ICrossNavigator>(index, _navigator));
            }
            else if (!string.IsNullOrEmpty(destruction) && destruction.Equals(result))
            {
                executeCommand?.Execute(new Tuple<int, ICrossNavigator>(-1, _navigator));
            }
        }
    }

    public static class CrossDialogExs
    {
        public static async Task DisplayAlert(this ICrossDialogProvider dialoger, string message)
        {
            await dialoger.DisplayAlert(title: "Alert", message: message,
                cancel: "Dismiss");
        }

        public static async Task DisplayAlert(this ICrossDialogProvider dialoger, string message,
            Action<object> actionAccept, object actionAcceptParam = null)
        {
            await dialoger.DisplayAlertEx(title: "Alert", message: message,
                accept: "OK", actionAccept: actionAccept,
                actionAcceptParam: actionAcceptParam);
        }

        public static async Task DisplayAlertEx(this ICrossDialogProvider dialoger, string message,
            ICommand commandAccept)
        {
            await dialoger.DisplayAlertEx(title: "Alert", message: message,
                accept: "OK", commandAccept: commandAccept);
        }

        public static async Task DisplayAlertEx(this ICrossDialogProvider dialoger, string message, Task taskAccept)
        {
            await dialoger.DisplayAlertEx(title: "Alert", message: message,
                accept: "OK",
                commandAccept: ReactiveCommand.CreateFromTask(async () => await taskAccept));
        }

        public static async Task DisplayAlertEx(this ICrossDialogProvider dialoger, string message,
            Action<object> actionAccept, Action<object> actionCancel = null)
        {
            await dialoger.DisplayAlertEx(title: "Alert", message: message,
                accept: "OK",
                cancel: "Cancel", actionAccept: actionAccept,
                actionCancel: actionCancel);
        }

        public static async Task DisplayActionSheetEx(this ICrossDialogProvider dialoger, string title,
            ICommand executeCommand,
            params string[] buttons)
        {
            await dialoger.DisplayActionSheetEx(title, "Dismiss", null,
                executeCommand, buttons);
        }

        public static async Task DisplayActionSheetEx(this ICrossDialogProvider dialoger, string title,
            ICommand executeCommand, string destruction, params string[] buttons)
        {
            await dialoger.DisplayActionSheetEx(title, "Dismiss",
                destruction: destruction,
                executeCommand, buttons);
        }
    }
}