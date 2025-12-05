using Gtlabs.Api.AmbientData;
using Gtlabs.Api.AmbientData.Interfaces;
using GTLabs.Identity.Authorization.Domain.Apps.Entities;
using Gtlabs.Persistence.CustomDbContext;
using Microsoft.EntityFrameworkCore;

namespace GTLabs.Identity.Authorization.Infrastructure.Contexts;

public class AuthorizationDbcontext : GtLabsDbContext
{        
    public DbSet<App> App { get; set; }
    public DbSet<AppPermission> AppPermission { get; set; }

    public AuthorizationDbcontext(DbContextOptions options, IAmbientData ambientData) : base(options, ambientData)
    {
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<App>()
            .HasMany(s => s.Permissions)
            .WithOne(p => p.App)
            .HasForeignKey(p => p.AppId);
        
        modelBuilder.Entity<AppPermission>()
            .HasOne(p => p.PermittedApp)
            .WithMany(a => a.PermittedBy)
            .HasForeignKey(p => p.PermittedAppId)
            .OnDelete(DeleteBehavior.Cascade);
        base.OnModelCreating(modelBuilder);
    }
}
public class AuthorizationContextFactory : GtLabsDbContextFactory<AuthorizationDbcontext>
{
}