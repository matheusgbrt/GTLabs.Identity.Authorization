using System.ComponentModel.DataAnnotations;
using GTLabs.Identity.Authorization.Domain.Apps.Entities;

namespace GTLabs.Identity.Authorization.Host.Apps.Dtos;

public class AppRegistrationDto
{
    [Required]
    [MaxLength(128)]
    public string Identifier { get; set; } = string.Empty;
    [Required]
    [MaxLength(128)]
    public string Route { get; set; } = string.Empty;
    [Required]
    [MaxLength(256)]
    public string Description { get; set; } = string.Empty;


    public App ToApp()
    {
        return new App()
        {
            Identifier = Identifier,
            Route = Route,
            Description = Description,
            Permissions = new List<AppPermission>()
        };
    }
}