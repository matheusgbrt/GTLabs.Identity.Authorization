using Gtlabs.Core.Dtos;
using Gtlabs.Core.Enums;
using GTLabs.Identity.Authorization.Domain.Apps.Models;
using GTLabs.Identity.Authorization.Domain.Apps.Services;
using GTLabs.Identity.Authorization.Host.Apps.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GTLabs.Identity.Authorization.Host.Apps.Controllers;

[Route("apps")]
public class AppsController : ControllerBase
{
    private readonly IAppService _appService;

    public AppsController(IAppService appService)
    {
        _appService = appService;
    }

    [Route("{id}")]
    [HttpGet]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _appService.GetById(id);

        if (!result.Found)
            return NotFound();
        return Ok(result);
    }
    
    [Route("{id}/permissions")]
    [HttpGet]
    public async Task<IActionResult> GetByIdWithPermissions([FromRoute] Guid id)
    {
        var result = await _appService.GetByIdWithIncludes(id);

        if (!result.Found)
            return NotFound();
        return Ok(result.Entity);
    }
    
    [Route("{id}/permissions/{permittedAppId}")]
    [HttpPost]
    public async Task<IActionResult> AddPermission([FromRoute] Guid id, [FromRoute] Guid permittedAppId)
    {
        if (id == permittedAppId)
        {
            var problemDetails = new ProblemDetails()
            {
                Detail = "Não pode vincular um serviço com ele mesmo"
            };
            return BadRequest(problemDetails);
        }
        var result = await _appService.AddPermission(id,permittedAppId);

        if (!result.Success)
            switch (result.Error)
            {
                case EntityAlterationError.Conflict:
                    return Conflict(result);
                case EntityAlterationError.NotFound:
                    return NotFound(result);
                default:
                    return BadRequest(result);
            }
        
        return Created();
    }
    
    [Route("{id}/permissions/{permittedAppId}")]
    [HttpDelete]
    public async Task<IActionResult> RemovePermission([FromRoute] Guid id, [FromRoute] Guid permittedAppId)
    {
        var result = await _appService.RemovePermission(id,permittedAppId);

        if (!result.Success)
            switch (result.Error)
            {
                case EntityAlterationError.Conflict:
                    return Conflict(result);
                case EntityAlterationError.NotFound:
                    return NotFound(result);
                default:
                    return BadRequest(result);
            }
        
        return Ok();
    }

    [Route("")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PagedRequest pagedRequest)
    {
        var searchResult = await _appService.GetList(pagedRequest);
        return Ok(searchResult);
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppRegistrationDto registrationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var app = registrationDto.ToApp();
        var result = await _appService.Create(app);

        if (!result.Success)
        {
            switch (result.Error)
            {
                case EntityAlterationError.Conflict:
                    return Conflict(result);
                case EntityAlterationError.NotFound:
                    return NotFound(result);
                default:
                    return BadRequest(result);
            }
        }

        return Created();
    }

    [Route("{id}")]
    [HttpPut]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AppUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var app = updateDto.ToApp(id);
        var result = await _appService.Update(app);

        if (!result.Success)
        {
            switch (result.Error)
            {
                case EntityAlterationError.Conflict:
                    return Conflict(result);
                case EntityAlterationError.NotFound:
                    return NotFound(result);
                default:
                    return BadRequest(result);
            }
        }

        return Ok(result);
    }

    [Route("{id}")]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _appService.Delete(id);

        if (!result.Success)
        {
            switch (result.Error)
            {
                case EntityAlterationError.Conflict:
                    return Conflict(result);
                case EntityAlterationError.NotFound:
                    return NotFound(result);
                default:
                    return BadRequest(result);
            }
        }

        return Ok(result);
    }
}