using Gtlabs.Core.Dtos;
using GTLabs.Identity.Authorization.Domain.Apps.Entities;
using GTLabs.Identity.Authorization.Domain.Apps.Models;

namespace GTLabs.Identity.Authorization.Domain.Apps.Services;

public interface IAppService
{
    public Task<EntitySearchResult<AppOutput>> GetById(Guid id);
    public Task<PagedEntityListSearchResult<AppOutput>> GetList(PagedRequest pagedRequest);
    public Task<EntityAlterationResult<AppOutput>> Create(App appCreation);
    public Task<EntityAlterationResult<AppOutput>> Update(App appUpdate);
    public Task<EntityAlterationResult<AppOutput>> Delete(Guid id);
    public Task<EntitySearchResult<AppOutputWithIncludes>> GetByIdentifierWithIncludes(string identifier);
    public Task<EntityAlterationResult<AppOutputWithIncludes>> AddPermission(Guid appId, Guid permittedAppId);
    public Task<EntityAlterationResult<AppOutputWithIncludes>> RemovePermission(Guid appId, Guid permittedAppId);
}