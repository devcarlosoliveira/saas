using Microsoft.AspNetCore.Identity;

namespace Saas.Web.Models;

public class ApplicationRole : IdentityRole<string>
{
    public ApplicationRole()
    {
        Id = Ulid.NewUlid().ToString();
    }
}
