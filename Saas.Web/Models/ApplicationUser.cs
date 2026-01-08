using Microsoft.AspNetCore.Identity;

namespace Saas.Web.Models;

public class ApplicationUser : IdentityUser<string>
{
    public ApplicationUser()
    {
        Id = Ulid.NewUlid().ToString();
    }

    // Propriedades de navegação adicionadas
    public virtual ICollection<Documento> Documentos { get; set; } = [];
    public virtual ICollection<PromptTemplate> TemplatesCriados { get; set; } = [];
}
