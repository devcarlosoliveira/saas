using Microsoft.AspNetCore.Identity;

namespace Saas.Web.Models;

public class ApplicationUser : IdentityUser<string>
{
    public ApplicationUser()
    {
        Id = Ulid.NewUlid().ToString();
    }
}
