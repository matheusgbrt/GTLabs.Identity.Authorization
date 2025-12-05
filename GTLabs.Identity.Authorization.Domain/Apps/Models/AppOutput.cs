namespace GTLabs.Identity.Authorization.Domain.Apps.Models;

public class AppOutput
{
    public Guid Id { get; set; }
    public string Identifier { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}