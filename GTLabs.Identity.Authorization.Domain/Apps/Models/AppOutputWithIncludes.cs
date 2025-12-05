namespace GTLabs.Identity.Authorization.Domain.Apps.Models;

public class AppOutputWithIncludes : AppOutput
{
    public List<AppOutput> PermittedApps { get; set; } = new List<AppOutput>();
}