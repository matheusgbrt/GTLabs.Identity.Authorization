namespace GTLabs.Identity.Authorization.Domain.Apps.Models;

public class AppPermissionOutput
{
    public string Identifier { get; set; } = string.Empty;
    public Guid AppId { get; set; } = Guid.Empty;
}