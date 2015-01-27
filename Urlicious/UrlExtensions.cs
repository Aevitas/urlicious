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
        /// Appends the specified collection of paths to the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="paths">The paths.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">paths</exception>
        public static Url AppendPaths(this Url url, IEnumerable<string> paths)
        {
            EnsureValidUrl(url);

            if (paths == null)
                throw new ArgumentException("paths");

            foreach (var p in paths)
            {
                url.AppendPath(p);
            }

            return url;
        }

        /// <summary>
        /// Appends the specified paths to the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="paths">The paths.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">paths</exception>
        public static Url AppendPaths(this Url url, params string[] paths)
        {
            EnsureValidUrl(url);

            if (paths == null || paths.Length == 0)
                throw new ArgumentException("paths");

            return url.AppendPaths(paths.ToList());
        }

        /// <summary>
        /// Determines whether the specified Url instance is a well-formed, absolute Uri.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool IsWellFormed(this Url url)
        {
            EnsureValidUrl(url);

            // Our URL should always be absolute, as it entails the "AbsolutePath" to our resource.
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }

        private static void EnsureValidUrl(Url url)
        {
            if (url == null)
                throw new ArgumentException("url");
        }
    }
}
