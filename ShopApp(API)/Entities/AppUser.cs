using Microsoft.AspNetCore.Identity;

namespace ShopApp_API_.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

    }
}
