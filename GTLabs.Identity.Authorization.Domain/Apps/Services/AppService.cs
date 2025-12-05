using Gtlabs.Core.Dtos;
using Gtlabs.Core.Enums;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using GTLabs.Identity.Authorization.Domain.Apps.Entities;
using GTLabs.Identity.Authorization.Domain.Apps.Models;
using Gtlabs.Persistence.Extensions;
using Gtlabs.Persistence.Repository;
using Gtlabs.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace GTLabs.Identity.Authorization.Domain.Apps.Services;

public class AppService : IAppService, ITransientDependency
{
    private readonly IRepository<App> _apps;
    private readonly IRepository<AppPermission> _appPermissions;
    private readonly IUnitOfWork _unitOfWork;
    
    public AppService(IRepository<App> apps, IRepository<AppPermission> appPermissions, IUnitOfWork unitOfWork)
    {
        _apps = apps;
        _appPermissions = appPermissions;
        _unitOfWork = unitOfWork;
    }

    public async Task<EntitySearchResult<AppOutput>> GetById(Guid id)
    {
        var app = await _apps.GetByIdAsync(id);
        if (app == null)
            return new EntitySearchResult<AppOutput>()
            {
                Found = false
            };

        return new EntitySearchResult<AppOutput>()
        {
            Found = true,
            Entity = app.ToAppOutput()
        };
    }

    public async Task<EntitySearchResult<AppOutputWithIncludes>> GetByIdentifierWithIncludes(string identifier)
    {
        var appEntity = await _apps.Query()
            .Include(a => a.Permissions)
            .ThenInclude(p => p.PermittedApp)
            .Include(a => a.PermittedBy)
            .ThenInclude(p => p.App)
            .FirstOrDefaultAsync(a => a.Identifier == identifier);
        if (appEntity == null)
            return new EntitySearchResult<AppOutputWithIncludes>()
            {
                Found = false
            };
        
        return new EntitySearchResult<AppOutputWithIncludes>()
        {
            Found = true,
            Entity = appEntity.ToAppOutputWithIncludes()
        };
    }

    public async Task<EntityAlterationResult<AppOutputWithIncludes>> AddPermission(Guid appId, Guid permittedAppId)
    {
        var appEntity = await _apps.Query()
            .Include(a => a.Permissions)
            .ThenInclude(p => p.PermittedApp)
            .Include(a => a.PermittedBy)
            .ThenInclude(p => p.App)
            .FirstOrDefaultAsync(a => a.Id == appId);

        var permittedApp = await _apps.Query()
            .Include(a => a.Permissions)
            .FirstOrDefaultAsync(a => a.Id == permittedAppId);

        if (appEntity == null || permittedApp == null)
        {
            return new EntityAlterationResult<AppOutputWithIncludes>
            {
                Success = false,
                Error = EntityAlterationError.NotFound,
                ErrorMessage = "App não encontrado"
            };
        }
        var outgoingExists = appEntity.Permissions
            .Any(p => p.PermittedAppId == permittedAppId);
        var incomingExists = permittedApp.Permissions
            .Any(p => p.PermittedAppId == appId);

        bool alreadyExists = outgoingExists || incomingExists;

        if (alreadyExists)
        {
            return new EntityAlterationResult<AppOutputWithIncludes>
            {
                Success = false,
                Error = EntityAlterationError.Conflict,
                Entity = appEntity.ToAppOutputWithIncludes(),
                ErrorMessage = "App já permitido."
            };
        }

        appEntity.Permissions.Add(new AppPermission
        {
            AppId = appId,
            PermittedAppId = permittedAppId
        });

        await _apps.UpdateAsync(appEntity, true);

        return new EntityAlterationResult<AppOutputWithIncludes>
        {
            Success = true
        };
    }
    
    public async Task<EntityAlterationResult<AppOutputWithIncludes>> RemovePermission(Guid appId, Guid permittedAppId)
    {
        var appEntity = await _apps.Query()
            .Include(a => a.Permissions)
            .ThenInclude(p => p.PermittedApp)
            .Include(a => a.PermittedBy)
            .ThenInclude(p => p.App)
            .FirstOrDefaultAsync(a => a.Id == appId);

        var permittedApp = await _apps.Query()
            .Include(a => a.Permissions)
            .FirstOrDefaultAsync(a => a.Id == permittedAppId);

        if (appEntity == null || permittedApp == null)
        {
            return new EntityAlterationResult<AppOutputWithIncludes>
            {
                Success = false,
                Error = EntityAlterationError.NotFound,
                ErrorMessage = "App não encontrado"
            };
        }
        var outgoingExists = appEntity.Permissions
            .Any(p => p.PermittedAppId == permittedAppId);
        var incomingExists = permittedApp.Permissions
            .Any(p => p.PermittedAppId == appId);

        if (!outgoingExists && !incomingExists)
        {
            return new EntityAlterationResult<AppOutputWithIncludes>()
            {
                Success = false,
                Error = EntityAlterationError.NotFound,
                Entity = appEntity.ToAppOutputWithIncludes(),
                ErrorMessage = "Permissão não encontrada"
            };
        }
        if (outgoingExists)
        {
            var permission = appEntity.Permissions.FirstOrDefault(p => p.PermittedAppId == permittedAppId);
            await _appPermissions.DeleteAsync(permission, true);
        }

        if (incomingExists)
        {
            var permission = permittedApp.Permissions.FirstOrDefault(p => p.PermittedAppId == appId);
            await _appPermissions.DeleteAsync(permission, true);
        }
        
        
        return new EntityAlterationResult<AppOutputWithIncludes>()
        {
            Success = true,
        };
    }

    public async Task<PagedEntityListSearchResult<AppOutput>> GetList(PagedRequest pagedRequest)
    {
        var apps = await _apps.Query().Page(pagedRequest.Page, pagedRequest.PageSize).ToListAsync();
        return new PagedEntityListSearchResult<AppOutput>()
        {
            Entities = apps.Select(app => app.ToAppOutput()).ToList(),
            TotalCount = _apps.Query().Count()
        };
    }

    public async Task<EntityAlterationResult<AppOutput>> Create(App appCreation)
    {
        var app = await _apps.Query().Where(appc => appc.Identifier == appCreation.Identifier).FirstOrDefaultAsync();
        if (app != null)
            return new EntityAlterationResult<AppOutput>()
            {
                Success = false,
                Error = EntityAlterationError.Conflict,
                ErrorMessage = "App já existe.",
                Entity = app.ToAppOutput()
            };

        await _apps.InsertAsync(appCreation,true);
        return new EntityAlterationResult<AppOutput>()
        {
            Success = true
        };
    }

    public async Task<EntityAlterationResult<AppOutput>> Update(App appUpdate)
    {
        var app = await _apps.GetByIdAsync(appUpdate.Id);
        if (app == null)
            return new EntityAlterationResult<AppOutput>()
            {
                Success = false,
                Error = EntityAlterationError.NotFound,
                ErrorMessage = "App não encontrado."
            };

        app.Update(appUpdate);

        await _apps.UpdateAsync(app);
        return new EntityAlterationResult<AppOutput>()
        {
            Success = true,
            Entity = app!.ToAppOutput(),
        };
    }

    public async Task<EntityAlterationResult<AppOutput>> Delete(Guid id)
    {
        var appEntity = await _apps.Query()
            .Include(a => a.Permissions)
            .ThenInclude(p => p.PermittedApp)
            .Include(a => a.PermittedBy)
            .ThenInclude(p => p.App)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (appEntity == null)
            return new EntityAlterationResult<AppOutput>()
            {
                Success = false,
                Error = EntityAlterationError.NotFound,
                ErrorMessage = "App não encontrado."
            };
        
        using (_unitOfWork.BeginAsync())
        {
            foreach (var perm in appEntity.Permissions)
                await _appPermissions.SetEntityStateAsync(perm, EntityState.Deleted);

            foreach (var perm in appEntity.PermittedBy)
                await _appPermissions.SetEntityStateAsync(perm, EntityState.Deleted);

            await _apps.SetEntityStateAsync(appEntity, EntityState.Deleted);
            await _unitOfWork.SaveChangesAsync();
        }

        return new EntityAlterationResult<AppOutput>()
        {
            Success = true,
        };
    }
}