using GTLabs.Identity.Authorization.Domain.Apps.Models;
using Gtlabs.Persistence.Entities;

namespace GTLabs.Identity.Authorization.Domain.Apps.Entities;

public class AppPermission : AuditedEntity
{
    public Guid AppId { get; set; }
    public Guid PermittedAppId { get; set; }
    public App App { get; set; } = null!;
    public App PermittedApp { get; set; } = null!;

    public AppPermissionOutput ToOutput()
    {
        var output = new AppPermissionOutput();
        output.AppId = AppId;
        output.Identifier = App.Identifier;
        return output;
    }
}