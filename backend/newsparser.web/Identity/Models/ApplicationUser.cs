using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NewsParser.Identity.Models
{
    /// <summary>
    /// Application user class
    /// </summary>
    public class ApplicationUser: IdentityUser
    {
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
