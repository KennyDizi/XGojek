using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Gojek.src.Services.NavigationService
{
    /// <inheritdoc />
    /// <summary>
    /// Represents Navigation parameters.
    /// </summary>
    /// <remarks>
    /// This class can be used to to pass object parameters during Navigation.
    /// </remarks>
    public class NavigationParameters : Dictionary<string, object>
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:connect.me.mobile.Services.NavigationService.NavigationParameters" /> class.
        /// </summary>
        public NavigationParameters()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:connect.me.mobile.Services.NavigationService.NavigationParameters" /> class with a query string.
        /// </summary>
        /// <param name="query">The query string.</param>
        public NavigationParameters(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                var num = query.Length;
                for (var i = query.Length > 0 && query[0] == '?' ? 1 : 0; i < num; i++)
                {
                    var startIndex = i;
                    var num4 = -1;
                    while (i < num)
                    {
                        var ch = query[i];
                        if (ch == '=')
                        {
                            if (num4 < 0)
                            {
                                num4 = i;
                            }
                        }
                        else if (ch == '&')
                        {
                            break;
                        }

                        i++;
                    }

                    string str = null; //key
                    string str2 = null; //value
                    if (num4 >= 0)
                    {
                        str = query.Substring(startIndex, num4 - startIndex);
                        str2 = query.Substring(num4 + 1, i - num4 - 1);
                    }

                    if (str != null)
                        Add(Uri.UnescapeDataString(str), Uri.UnescapeDataString(str2));
                }
            }
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            if (base.TryGetValue(key, out object result))
            {
                if (result == null)
                {
                    value = default;
                    return false;
                }
                else if (result.GetType() == typeof(T))
                    value = (T) result;
                else if (typeof(T).GetTypeInfo().IsAssignableFrom(result.GetType().GetTypeInfo()))
                    value = (T) result;
                else
                    value = (T) Convert.ChangeType(result, typeof(T));

                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Converts the list of key value pairs to a query string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var queryBuilder = new StringBuilder();

            if (Count > 0)
            {
                queryBuilder.Append('?');
                var first = true;

                foreach (var kvp in this)
                {
                    if (!first)
                    {
                        queryBuilder.Append('&');
                    }
                    else
                    {
                        first = false;
                    }

                    queryBuilder.Append(Uri.EscapeDataString(kvp.Key));
                    queryBuilder.Append('=');
                    queryBuilder.Append(Uri.EscapeDataString(kvp.Value.ToString()));
                }
            }

            return queryBuilder.ToString();
        }
    }
}