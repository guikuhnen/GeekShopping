using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.DbModel
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
