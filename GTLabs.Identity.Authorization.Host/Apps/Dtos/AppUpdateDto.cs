using System.ComponentModel.DataAnnotations;
using GTLabs.Identity.Authorization.Domain.Apps.Entities;

namespace GTLabs.Identity.Authorization.Host.Apps.Dtos;

public class AppUpdateDto
{
    [Required]
    [MaxLength(128)]
    public string Identifier { get; set; } = String.Empty;
    [Required]
    [MaxLength(128)]
    public string Route { get; set; } = String.Empty;
    [Required]
    [MaxLength(256)]
    public string Description { get; set; } = String.Empty;


    public App ToApp(Guid id)
    {
        return new App()
        {
            Id = id,
            Identifier = Identifier,
            Route = Route,
            Description = Description
        };
    }
}