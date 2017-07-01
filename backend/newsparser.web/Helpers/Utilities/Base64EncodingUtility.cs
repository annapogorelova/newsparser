namespace NewsParser.Helpers.Utilities
{
    /// <summary>
    /// Utility class for Base64 encoding/deconding
    /// </summary>
    public static class Base64EncodingUtility
    {
        public static string Encode(string plainText) 
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Decode(string base64EncodedData) 
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}