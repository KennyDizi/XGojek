namespace Gojek.Services.NavigationService
{
    public interface IConnectivityAware
    {
        /// <summary>
        /// called when connectivity connected
        /// </summary>
        void OnConnected();

        /// <summary>
        /// called when connectivity disconnected
        /// </summary>
        void OnDisConnected();
    }
}