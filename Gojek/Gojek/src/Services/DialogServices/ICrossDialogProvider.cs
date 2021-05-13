using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Gojek.src.Services.DialogServices
{
    public interface ICrossDialogProvider
    {
        /// <summary>
        /// display simple dialog
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task DisplayAlert(string title, string message, string cancel);

        /// <summary>
        /// display simple confirm dialog
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="accept"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);

        /// <summary>
        /// display simple actionsheet
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cancel"></param>
        /// <param name="destruction"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);

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
        /// <returns></returns>
        Task<string> DisplayPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel",
            string placeholder = null, int maxLength = -1, Keyboard keyboard = default, string initialValue = "");

        #region extra dialog

        /// <summary>
        /// display confirm dialog with action accept and action cancel
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="accept"></param>
        /// <param name="cancel"></param>
        /// <param name="actionAccept"></param>
        /// <param name="actionCancel"></param>
        /// <param name="actionAcceptParam"></param>
        /// <param name="actionCancelParam"></param>
        /// <returns></returns>
        Task DisplayAlertEx(string title, string message, string accept, string cancel,
            Action<object> actionAccept = null,
            Action<object> actionCancel = null, object actionAcceptParam = null, object actionCancelParam = null);

        /// <summary>
        /// display simple dialog with allway action
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="accept"></param>
        /// <param name="actionAccept"></param>
        /// <param name="actionAcceptParam"></param>
        /// <returns></returns>
        Task DisplayAlertEx(string title, string message, string accept, Action<object> actionAccept = null,
            object actionAcceptParam = null);

        /// <summary>
        /// force execute command
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="accept"></param>
        /// <param name="commandAccept"></param>
        /// <returns></returns>
        Task DisplayAlertEx(string title, string message, string accept, ICommand commandAccept);

        /// <summary>
        /// execute command if use accept
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="accept"></param>
        /// <param name="cancel"></param>
        /// <param name="commandAccept"></param>
        /// <param name="commandCancel"></param>
        /// <returns></returns>
        Task DisplayAlertEx(string title, string message, string accept, string cancel, ICommand commandAccept,
            ICommand commandCancel = null);

        /// <summary>
        /// display action sheet with execute command with param is index of <see cref="buttons"/>
        /// if choosen is destruction param is -1
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cancel"></param>
        /// <param name="destruction"></param>
        /// <param name="executeCommand"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        Task DisplayActionSheetEx(string title, string cancel, string destruction, ICommand executeCommand,
            params string[] buttons);

        /// <summary>
        /// display actionshet with execute command with param is index of <see cref="buttons"/>
        /// if choosen is destruction param is -1
        /// this option can use navigator to navigate other page
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cancel"></param>
        /// <param name="destruction"></param>
        /// <param name="executeCommand"></param>
        /// <param name="needUseNavigator"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        Task DisplayActionSheetExWitNav(string title, string cancel, string destruction, ICommand executeCommand,
            bool needUseNavigator, params string[] buttons);

        #endregion
    }
}