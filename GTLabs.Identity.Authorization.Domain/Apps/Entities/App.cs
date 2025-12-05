using GTLabs.Identity.Authorization.Domain.Apps.Models;
using Gtlabs.Persistence.Entities;

namespace GTLabs.Identity.Authorization.Domain.Apps.Entities;

public class App : AuditedEntity
{
    public string Identifier { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public ICollection<AppPermission> Permissions { get; set; } = new List<AppPermission>();
    public ICollection<AppPermission> PermittedBy { get; set; } = new List<AppPermission>();

    public App Update(App appUpdate)
    {
        this.Identifier = appUpdate.Identifier;
        this.Description = appUpdate.Description;
        this.Route = appUpdate.Route;
        return this;
    }

    private T ToAppOutputBase<T>() where T : AppOutput, new()
    {
        return new T
        {
            Id = this.Id,
            Identifier = this.Identifier,
            Description = this.Description!,
            Route = this.Route
        };
    }

    public AppOutput ToAppOutput()
    {
        return ToAppOutputBase<AppOutput>();
    }

    public AppOutputWithIncludes ToAppOutputWithIncludes()
    {
        var appWithIncludes = ToAppOutputBase<AppOutputWithIncludes>();

        var outgoingApps = Permissions
            .Select(p => p.PermittedApp.ToAppOutput())
            .ToList();

        var incomingApps = PermittedBy
            .Select(p => p.App.ToAppOutput())
            .ToList();

        appWithIncludes.PermittedApps.AddRange(outgoingApps);
        appWithIncludes.PermittedApps.AddRange(incomingApps);

        return appWithIncludes;
    }
}