using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;

namespace Gojek.Utilities
{
    public class CrossRestClient : HttpClient
    {
        public CrossRestClient()
        {
        }

        public CrossRestClient(HttpMessageHandler handler) : base(handler: handler)
        {
        }

        /// <summary>
        /// Add custom http header
        /// </summary>
        /// <param name="key">header name</param>
        /// <param name="value">header value</param>
        public void InjectHeader(string key, string value)
        {
            if (DefaultRequestHeaders.Contains(key))
            {
                DefaultRequestHeaders.Remove(key);
            }

            DefaultRequestHeaders.Add(key, value);
        }

        /// <summary>
        /// Injects the authorization token to header
        /// </summary>
        /// <param name="token">access token</param>
        public void InjectAuthorizationToken(string token)
        {
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Method to invoke GET operation
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="uri">target url</param>
        /// <returns>Task of type T</returns>
        public async Task<T> GetAsync<T>(string uri)
        {
            var response = await GetAsync(uri).ConfigureAwait(false);
            await response.Content.ReadAsStringAsync();

            return await GetResponseObject<T>(response);
        }

        /// <summary>
        /// Method to invoke GET operation
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="uri">target url</param>
        /// <returns>Task of type T</returns>
        public async Task<T> GetAsync<T>(Uri uri)
        {
            return await GetAsync<T>(uri.ToString());
        }

        /// <summary>
        /// Method to invoke POST operation
        /// </summary>
        /// <typeparam name="T">Request type</typeparam>
        /// <typeparam name="U">Response type</typeparam>
        /// <param name="uri">target url</param>
        /// <param name="obj">Request object</param>
        /// <returns>Task of type U</returns>
        public async Task<U> PostAsync<T, U>(string uri, T obj)
        {
            var json = JsonConvert.SerializeObject(obj);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await PutAsync(uri, content).ConfigureAwait(false);

            return await GetResponseObject<U>(response);
        }

        /// <summary>
        /// Method to invoke POST operation
        /// </summary>
        /// <typeparam name="T">Request type</typeparam>
        /// <typeparam name="U">Response type</typeparam>
        /// <param name="uri">target url</param>
        /// <param name="obj">Request object</param>
        /// <returns>Task of type U</returns>
        public async Task<U> PostAsync<T, U>(Uri uri, T obj)
        {
            return await PostAsync<T, U>(uri.ToString(), obj);
        }

        /// <summary>
        /// Method to invoke PUT operation
        /// </summary>
        /// <typeparam name="T">Request type</typeparam>
        /// <typeparam name="U">Response type</typeparam>
        /// <param name="uri">Target url</param>
        /// <param name="obj">Request object</param>
        /// <returns>Task of type U</returns>
        public async Task<U> PutAsync<T, U>(string uri, T obj)
        {
            var json = JsonConvert.SerializeObject(obj);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await PutAsync(uri, content).ConfigureAwait(false);

            return await GetResponseObject<U>(response);
        }

        /// <summary>
        /// Method to invoke PUT operation
        /// </summary>
        /// <typeparam name="T">Request type</typeparam>
        /// <typeparam name="U">Response type</typeparam>
        /// <param name="uri">Target url</param>
        /// <param name="obj">Request object</param>
        /// <returns>Task of type U</returns>
        public async Task<U> PutAsync<T, U>(Uri uri, T obj)
        {
            return await PutAsync<T, U>(uri.ToString(), obj);
        }

        /// <summary>
        /// Method to invoke DELETE operation
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="uri">target url</param>
        /// <returns>Task of type T</returns>
        public async Task<T> DeleteAsync<T>(string uri)
        {
            var response = await DeleteAsync(uri);
            return await GetResponseObject<T>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Method to invoke DELETE operation
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="uri">target url</param>
        /// <returns>Task of type T</returns>
        public async Task<T> DeleteAsync<T>(Uri uri)
        {
            return await DeleteAsync<T>(uri.ToString());
        }

        private static async Task<T> GetResponseObject<T>(HttpResponseMessage response)
        {
            var serializer = new JsonSerializer {NullValueHandling = NullValueHandling.Ignore};
            response.EnsureSuccessStatusCode();
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var json = new JsonTextReader(reader))
                    {
                        return serializer.Deserialize<T>(json);
                    }
                }
            }
        }
    }

    public static class CrossRestClientExts
    {
        public static CrossRestClient CreateNewOne()
        {
            var client = new CrossRestClient();
            return client;
        }

        public static CrossRestClient CreateNewOneFaster()
        {
            var client = new CrossRestClient(new NativeMessageHandler(throwOnCaptiveNetwork: false, new TLSConfig()));
            return client;
        }
    }
}