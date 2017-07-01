using System.Text.RegularExpressions;

namespace NewsParser.FeedParser.Helpers
{
    /// <summary>
    /// Class contains extension methods for html strings
    /// </summary>
    public static class HtmlStringExtensions
    {
        /// <summary>
        /// Removes the tags from html string
        /// </summary>
        /// <param name="html">Html string</param>
        /// <returns>Cleaned html string</returns>
        public static string RemoveHtmlTags(this string html)
        {
            return Regex.Replace(html, "<.*?>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Replaces the tabulation characters with the one specified
        /// </summary>
        /// <param name="html">Html string</param>
        /// <param name="replacementCharacter">Character to replace the tabulation characters</param>
        /// <returns>Cleaned html string</returns>
        public static string RemoveTabulation(this string html, string replacementCharacter)
        {
            return Regex.Replace(html, @"\t|\n|\r", replacementCharacter);
        }

        /// <summary>
        /// Crops the string to the max length specified
        /// </summary>
        /// <param name="input">String</param>
        /// <param name="maxLength">Max length</param>
        /// <returns>Cropped string</returns>
        public static string CropString(this string input, int maxLength)
        {
            if (input.Length < maxLength)
            {
                return input;
            }

            var croppedString = input.Substring(0, maxLength - 3);
            return $"{croppedString.Substring(0, croppedString.LastIndexOf(' '))}...";
        }

        public static string RemoveNonAlphanumericCharacters(this string input)
        {
            return Regex.Replace(input, @"\p{Cs}", string.Empty);
        }
    }
}