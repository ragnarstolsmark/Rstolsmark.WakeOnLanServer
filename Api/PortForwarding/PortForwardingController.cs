using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rstolsmark.WakeOnLanServer.Configuration;
using Rstolsmark.WakeOnLanServer.Services.PortForwarding;
using Rstolsmark.WakeOnLanServer.Api.Errors;
using FluentValidation;
using Rstolsmark.WakeOnLanServer.ValidationHelpers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly IPortForwardingService? _portForwardingService;
    private readonly IValidator<PortForwardingDto>? _validator;

    public PortForwardingController(
        IAuthorizationService authorizationService, 
        IAuthorizationPolicyProvider authorizationPolicyProvider,
        ProblemDetailsFactory problemDetailsFactory,
        IPortForwardingService? portForwardingService = null,
        IValidator<PortForwardingDto>? validator = null)
    {
        _authorizationService = authorizationService;
        _authorizationPolicyProvider = authorizationPolicyProvider;
        _problemDetailsFactory = problemDetailsFactory;
        _portForwardingService = portForwardingService;
        _validator = validator;
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
    
    [MemberNotNullWhen(true, nameof(_portForwardingService), nameof(_validator))]
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

    [HttpPost]
    [Route("{id}/disable")]
    public async Task<IActionResult> DisableById(string id){

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
        await _portForwardingService.Disable(id);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> AddPortForwarding(PortForwardingDto portForwarding){
        if (!PortForwardingIsConfigured())
        {
            return NotFound();
        }
        if (!await UserIsAuthorizedForPortForwardingAccess())
        {
            return Forbid();
        }
        var validationResult = _validator.Validate(portForwarding);
        if(!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            var validationProblemDetails = _problemDetailsFactory.CreateValidationProblemDetails(Request.HttpContext, ModelState);
            return BadRequest(validationProblemDetails);
        }
        var createdPortForwarding = await _portForwardingService.AddPortForwarding(new PortForwardingData(portForwarding));
        return CreatedAtAction
        (
            nameof(GetById),
            new { createdPortForwarding.Id },
            new PortForwardingWithIdAndEnabledDto(createdPortForwarding)
        );
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> EditPortForwarding(string id, PortForwardingDto portForwarding){
        if (!PortForwardingIsConfigured())
        {
            return NotFound();
        }
        
        if (!await UserIsAuthorizedForPortForwardingAccess())
        {
            return Forbid();
        }
        
        var existingPortForwarding = await _portForwardingService.GetById(id);
        if (existingPortForwarding == null){
            return NotFound();
        }

        var validationResult = _validator.Validate(portForwarding);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            var validationProblemDetails = _problemDetailsFactory.CreateValidationProblemDetails(Request.HttpContext, ModelState);
            return BadRequest(validationProblemDetails);
        }
        
        await _portForwardingService.EditPortForwarding(id, new PortForwardingData(portForwarding));

        return Ok();
    }
}