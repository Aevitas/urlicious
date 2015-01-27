using System;
using System.Collections.Generic;
using System.Linq;

namespace Urlicious
{
    /// <summary>
    /// Represents a Uniform Resource Locator (URL).
    /// </summary>
    public class Url
    {
        /// <summary>
        /// Gets or sets the absolute path to the host this URL points to.
        /// </summary>
        public string AbsolutePath { get; set; }

        /// <summary>
        /// Gets a value indicating whether HTTP parameters should be HTTP encoded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [encode parameters]; otherwise, <c>false</c>.
        /// </value>
        public bool EncodeParameters { get; private set; }

        /// <summary>
        /// Gets the parameters tied to this URL. The keys in this dictionary indicate the parameter names, where the keys indicate the respective values.
        /// Note that all keys should be appropriately convertable to string.
        /// </summary>
        public Dictionary<string, object> Queries { get; private set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Url"/> class.
        /// </summary>
        /// <path name="url">The URL.</path>
        /// <exception cref="System.ArgumentException">url</exception>
        public Url(string url, bool encodeParameters = true) : this()
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("url");

            AbsolutePath = url;
            EncodeParameters = encodeParameters;
        }

        private Url()
        {
            Queries = new Dictionary<string, object>();
        }

        #endregion

        /// <summary>
        /// Appends the specified path to the URL, taking into account slashes.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.ArgumentException">path</exception>
        public Url AppendPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("path");

            // Ensure the path ends with a slash.
            if (!AbsolutePath.EndsWith("/"))
                AbsolutePath += "/";

            // .. and the part we're appending doesn't.
            AbsolutePath += path.TrimStart('/').TrimEnd('/');

            return this;
        }

        /// <summary>
        /// Adds or updates the specified query parameter and assigns it the specified value.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="value">The value.</param>
        public Url AddQuery(string param, object value)
        {
            if (param.Contains(param))
            {
                Queries[param] = value;
                return this;
            }

            Queries.Add(param, value);

            return this;
        }

        /// <summary>
        /// Removes the query with the specified key from the URL query collection.
        /// </summary>
        /// <param name="key">The key.</param>
        public void RemoveQuery(string key)
        {
            if (!Queries.ContainsKey(key))
                return;

            Queries.Remove(key);
        }

        /// <summary>
        /// Gets the value of the URL query with the specified key.
        /// Keep in mind that parsed URLs will contain stringly typed query values regardless of their original type,
        /// and may require a custom retriever to return them to their original type.
        /// 
        /// Specify a retriever if you need to perform operations on the query value in order to return it as type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="retriever">The retriever.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public T GetQueryValue<T>(string key, Func<object, T> retriever = null)
        {
            if (!Queries.ContainsKey(key))
                throw new InvalidOperationException(string.Format("Can not obtain value for key {0}; it isn't registered for this URL."));

            var entry = Queries[key];

            return retriever != null ? retriever(entry) : (T)entry;
        }

        /// <summary>
        /// Gets a collection of all paths specified in the URL.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPaths()
        {
            var path = AbsolutePath.Remove(0, RootUrl.Length);

            path = path.Remove(path.Length - QueryString.Length, QueryString.Length);
            path = path.TrimStart('/').TrimEnd('/');

            return path.Split('/');
        }

        /// <summary>
        /// Gets the root URL of this Url instance.
        /// </summary>
        /// <value>
        /// The root URL.
        /// </value>
        public string RootUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AbsolutePath))
                    return null;

                return GetRootUrl(AbsolutePath);
            }
        }

        /// <summary>
        /// Obtains the root URL (protocol, host) from the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="includeTrailingSlash">if set to <c>true</c> [include trailing slash].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">url</exception>
        public static string GetRootUrl(string url, bool includeTrailingSlash = false)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("url");

            var uri = new Uri(url);

            // PERF:    The includeTrailingSlash is implemented poorly here - as string is immutable, a new string will be alloc'd regardless
            //          of whether or not we actually want to include the trailing slash. Should probably be optimized (depending on how the JIT handles it).
            return uri.GetComponents(
                UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped) + (includeTrailingSlash ? "/" : string.Empty);
        }

        /// <summary>
        /// Resets this URL to the root URL, removing all paths and parameters.
        /// </summary>
        public void Reset(bool truncateQueries = true)
        {
            AbsolutePath = GetRootUrl(AbsolutePath);

            if (truncateQueries)
                Queries.Clear();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this URL.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var url = AbsolutePath;

            // No params? Cool, we're done!
            if (Queries.Count <= 0)
                return url;

            if (url.EndsWith("/"))
                url = url.TrimEnd('/');

            url += "?";
            url += QueryString;

            return url;
        }

        private string ProcessQuery(string what)
        {
            return EncodeParameters ? Utilities.UrlEncode(what) : what;
        }

        #region Implicit Operators

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="Url"/>.
        /// </summary>
        /// <path name="url">The URL.</path>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Url(string url)
        {
            return new Url(url);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Url"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(Url url)
        {
            return url.ToString();
        }

        #endregion

        /// <summary>
        /// Parses the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">The current Uri is not an absolute URI. Relative URIs cannot be used with this method.</exception>
        public static Url Parse(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new InvalidOperationException("The current Uri is not an absolute URI. Relative URIs cannot be used with this method.");

            return Parse(new Uri(url));
        }

        /// <summary>
        /// Parses the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static Url Parse(Uri url)
        {
            // We need not check for WellFormedUriString here - if it's malformed, the subsequent operations will throw for us.
            var ret = new Url {AbsolutePath = url.ToString()};

            var q = url.GetComponents(UriComponents.Query, UriFormat.Unescaped).Split('&');

            ParseQueries(ret, q);

            return ret;
        }

        private static void ParseQueries(Url instance, string[] queries)
        {
            if (instance == null)
                throw new InvalidOperationException("Parsing queries requires a valid Url instance!");

            // Nothing to parse?
            if (queries.Length == 0)
                return;

            foreach (var s in queries.Select(q => q.Split('=')))
            {
                instance.Queries.Add(s[0], s[1]);
            }

            // As we concatenate the queries onto the path later on, we'll want to remove the query from the 
            // path after we're done parsing it - otherwise we'd end up with double queries when calling ToString.
            instance.AbsolutePath = instance.AbsolutePath.Split('?')[0];
        }

        private string QueryString
        {
            get
            {
                var url = string.Empty;
                var param = Queries.ToList();
                for (int i = 0; i < Queries.Count; i++)
                {
                    var p = param[i];
                    url += string.Format("{0}={1}", p.Key, ProcessQuery(p.Value.ToString()));

                    if (i < param.Count - 1)
                        url += "&";
                }

                return url;
            }
        }
    }
}
