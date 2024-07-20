using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rstolsmark.WakeOnLanServer.Configuration;
using Rstolsmark.WakeOnLanServer.Services.PortForwarding;
using Rstolsmark.WakeOnLanServer.Api.Errors;

namespace Rstolsmark.WakeOnLanServer.Api.PortForwarding;
#nullable enable
[ApiController]
[SkipStatusCodePages]
[TypeFilter(typeof(ApiExceptionFilter))]
[Route("api/[controller]")]
public class PortForwardingController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IAuthorizationPolicyProvider _authorizationPolicyProvider;
    private readonly IPortForwardingService? _portForwardingService;

    public PortForwardingController(
        IAuthorizationService authorizationService, 
        IAuthorizationPolicyProvider authorizationPolicyProvider,
        IPortForwardingService? portForwardingService = null)
    {
        _authorizationService = authorizationService;
        _authorizationPolicyProvider = authorizationPolicyProvider;
        _portForwardingService = portForwardingService;
    }

    private async Task<bool> UserIsAuthorizedForPortForwardingAccess()
    {
        if (await _authorizationPolicyProvider.GetPolicyAsync(AuthorizationConfiguration.RequirePortForwardingRole) != null)
        {
            var requirePortForwardingRoleResult =
                await _authorizationService.AuthorizeAsync(User, null,
                    AuthorizationConfiguration.RequirePortForwardingRole);
            return requirePortForwardingRoleResult.Succeeded;
        }
        return true;
    }
    
    [MemberNotNullWhen(true, nameof(_portForwardingService))]
    private bool PortForwardingIsConfigured() => _portForwardingService is not null;
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!PortForwardingIsConfigured())
        {
            return NotFound();
        }
        if (!await UserIsAuthorizedForPortForwardingAccess())
        {
            return Forbid();
        }
        var portForwardings = await _portForwardingService.GetAll();
        
        var portForwardingResult =
            portForwardings
                .Select(p => new PortForwardingWithIdAndEnabledDto(p));
        return Ok(portForwardingResult);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(string id){

        if (!PortForwardingIsConfigured())
        {
            return NotFound();
        }
        if (!await UserIsAuthorizedForPortForwardingAccess())
        {
            return Forbid();
        }
        var portForwarding = await _portForwardingService.GetById(id);
        if(portForwarding == null){
            return NotFound();
        }
        return Ok(new PortForwardingWithIdAndEnabledDto(portForwarding));
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteById(string id){

        if (!PortForwardingIsConfigured())
        {
            return NotFound();
        }
        if (!await UserIsAuthorizedForPortForwardingAccess())
        {
            return Forbid();
        }
        var portForwarding = await _portForwardingService.GetById(id);
        if(portForwarding == null){
            return NotFound();
        }
        await _portForwardingService.Delete(id);
        return Ok();
    }

    [HttpPost]
    [Route("{id}/enable")]
    public async Task<IActionResult> EnableById(string id){

        if (!PortForwardingIsConfigured())
        {
            return NotFound();
        }
        if (!await UserIsAuthorizedForPortForwardingAccess())
        {
            return Forbid();
        }
        var portForwarding = await _portForwardingService.GetById(id);
        if(portForwarding == null){
            return NotFound();
        }
        await _portForwardingService.Enable(id);
        return Ok();
    }
}