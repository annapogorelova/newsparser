using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NewsParser.Web.Identity.Models
{
    /// <summary>
    /// Application user class
    /// </summary>
    public class ApplicationUser: IdentityUser
    {
        public bool HasSubscriptions { get; set; }
        public List<ExternalIdModel> ExternalIds { get; set; }

        public int GetId()
        {
            return int.Parse(Id);
        }
      
        public ApplicationUser()
        {
            ExternalIds = new List<ExternalIdModel>();
        }
    }
}
