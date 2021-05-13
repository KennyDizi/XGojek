namespace Gojek.Services.NavigationService
{
    public interface ICrossNamingConventions
    {
        /// <summary>
        /// ending name of a page
        /// </summary>
        string ViewEnding { get; }

        /// <summary>
        /// ending name of a page's viemodel
        /// </summary>
        string ViewModelEnding { get; }

        /// <summary>
        /// funtion to get page name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetViewName(string name);

        /// <summary>
        /// funtion to get page's viewmodel name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetViewModelName(string name);

        /// <summary>
        /// get viewmodel title
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetViewModelTitle(string name);

        /// <summary>
        /// get strip or viewmodel ending
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        string StripViewOrViewModelEnding(string className);
    }
}