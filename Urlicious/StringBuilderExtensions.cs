using System.Linq;
using System.Text;

namespace Urlicious
{
    /// <summary>
    /// Contains utility methods for use with a StringBuilder.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Removes all leading occurrences of a set of characters specified in an array from the current StringBuilder object.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="chars">The chars.</param>
        public static StringBuilder TrimStart(this StringBuilder sb, params char[] chars)
        {
            if (!IsValidStringBuilder(sb))
                return sb;

            int truncate = 0;
            for (int i = 0; i < sb.Length; i++)
            {
                if (!chars.Contains(sb[i]))
                    break;

                truncate++;
            }

            // Use a single call to remove as this is the most expensive op in the entire method.
            if (truncate > 0)
                sb.Remove(0, truncate);

            return sb;
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of characters specified in an array from the current StringBuilder object.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="chars">The chars.</param>
        public static StringBuilder TrimEnd(this StringBuilder sb, params char[] chars)
        {
            if (!IsValidStringBuilder(sb))
                return sb;

            int truncate = 0;
            for (int i = sb.Length - 1; i > 0; i--)
            {
                if (!chars.Contains(sb[i]))
                    break;

                truncate++;
            }

            if (truncate > 0)
                sb.Remove(sb.Length - truncate, truncate);

            return sb;
        }

        /// <summary>
        /// Determines whether the end of this StringBuilder instance matches the specified string.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool EndsWith(this StringBuilder sb, string value)
        {
            if (!IsValidStringBuilder(sb))
                return false;

            if (value.Length > sb.Length)
                return false;

            for (int i = sb.Length - 1, j = value.Length - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (value[j] != sb[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the beginning of this StringBuilder instance matches the specified string.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool StartsWith(this StringBuilder sb, string value)
        {
            if (!IsValidStringBuilder(sb))
                return false;

            return !value.Where((t, i) => sb[i] != t).Any();
        }

        private static bool IsValidStringBuilder(StringBuilder sb)
        {
            if (sb == null || sb.Length == 0)
                return false;

            return true;
        }
    }
}
