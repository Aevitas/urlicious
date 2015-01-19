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
        public Dictionary<string, object> Parameters { get; private set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Url"/> class.
        /// </summary>
        /// <path name="url">The URL.</path>
        /// <exception cref="System.ArgumentException">url</exception>
        public Url(string url, bool encodeParameters = true)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("url");

            AbsolutePath = url;
            EncodeParameters = encodeParameters;
            Parameters = new Dictionary<string, object>();
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
        /// Adds or updates the specified parameter and assigns it the specified value.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="value">The value.</param>
        public Url AppendParameter(string param, object value)
        {
            if (param.Contains(param))
            {
                Parameters[param] = value;
                return this;
            }

            Parameters.Add(param, value);

            return this;
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
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">url</exception>
        public static string GetRootUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("url");

            var uri = new Uri(url);

            return uri.GetComponents(
                UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped);
        }

        /// <summary>
        /// Resets this URL to the root URL, removing all paths and parameters.
        /// </summary>
        public void Reset(bool truncateParameters = true)
        {
            AbsolutePath = GetRootUrl(AbsolutePath);

            if (truncateParameters)
                Parameters.Clear();
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
            if (Parameters.Count <= 0)
                return url;

            if (url.EndsWith("/"))
                url = url.TrimEnd('/');

            url += "?";

            var param = Parameters.ToList();
            for (int i = 0; i < Parameters.Count; i++)
            {
                var p = param[i];
                url += string.Format("{0}={1}", p.Key, ProcessArgument(p.Value.ToString()));

                if (i < param.Count - 1)
                    url += "&";
            }

            return url;
        }

        private string ProcessArgument(string what)
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
    }
}
