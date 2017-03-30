using newsparser.DAL.Models;

namespace NewsParser.Identity.Models
{
    /// <summary>
    /// Class represents the external id model
    /// </summary>
    public class ExternalIdModel
    {
        public string ExternalId { get; set; }
        public ExternalAuthProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets the external id with alias based on external auth provider
        /// </summary>
        public string NormalizedExternalId
        {
            get
            {
                switch (AuthProvider)
                {
                    case ExternalAuthProvider.Facebook:
                        return $"facebook:{ExternalId}";
                    case ExternalAuthProvider.Google:
                        return $"google:{ExternalId}";
                    default:
                        return ExternalId;
                }
            }
        }
    }
}
