using System.Text.RegularExpressions;

namespace NewsParser.FeedParser
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
        /// Crops the html string to the max length specified
        /// </summary>
        /// <param name="html">Html string</param>
        /// <param name="maxLength">Max length</param>
        /// <returns>Cropped html string</returns>
        public static string CropHtmlString(this string html, int maxLength)
        {
            if (html.Length < maxLength)
            {
                return html;
            }

           var croppedHtmlString = html.Substring(0, maxLength - 3);
            return $"{croppedHtmlString.Substring(0, croppedHtmlString.LastIndexOf(' '))}...";
        }
        
    }
}