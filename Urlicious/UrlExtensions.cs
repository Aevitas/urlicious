using System;
using System.Collections.Generic;
using System.Linq;

namespace Urlicious
{
    /// <summary>
    /// Extension methods for "fluent" URL creation.
    /// </summary>
    public static class UrlExtensions
    {
        /// <summary>
        /// Adds the specified parameter and value to this URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Url AddParam(this Url url, string key, object value)
        {
            EnsureValidUrl(url);

            return url.AppendParameter(key, value);
        }

        /// <summary>
        /// Adds the specified sub-path to this URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static Url AddPath(this Url url, string path)
        {
            EnsureValidUrl(url);

            return url.AppendPath(path);
        }

        /// <summary>
        /// Appends the specified collection of paths to the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="paths">The paths.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">paths</exception>
        public static Url AppendPaths(this Url url, IEnumerable<string> paths)
        {
            if (paths == null)
                throw new ArgumentException("paths");

            foreach (var p in paths)
            {
                url.AppendPath(p);
            }

            return url;
        }

        /// <summary>
        /// Appends the paths.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="paths">The paths.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">paths</exception>
        public static Url AppendPaths(this Url url, params string[] paths)
        {
            if (paths == null || paths.Length == 0)
                throw new ArgumentException("paths");

            return url.AppendPaths(paths.ToList());
        }

        private static void EnsureValidUrl(Url url)
        {
            if (url == null)
                throw new ArgumentException("url");
        }
    }
}
