using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rstolsmark.WakeOnLanServer.Configuration;
using Rstolsmark.WakeOnLanServer.Api.Errors;
using Rstolsmark.WakeOnLanServer.ValidationHelpers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Rstolsmark.WakeOnLanServer.Services.WakeOnLan;

namespace Rstolsmark.WakeOnLanServer.Api.WakeOnLan;
#nullable enable
[ApiController]
[SkipStatusCodePages]
[TypeFilter(typeof(ApiExceptionFilter))]
[Route("api/[controller]")]
public class WakeOnLanController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IAuthorizationPolicyProvider _authorizationPolicyProvider;
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly ComputerService _computerService;

    public WakeOnLanController(
        IAuthorizationService authorizationService, 
        IAuthorizationPolicyProvider authorizationPolicyProvider,
        ProblemDetailsFactory problemDetailsFactory,
        ComputerService computerService)
    {
        _authorizationService = authorizationService;
        _authorizationPolicyProvider = authorizationPolicyProvider;
        _problemDetailsFactory = problemDetailsFactory;
        _computerService = computerService;
    }

    private async Task<bool> UserIsAuthorizedForWakeOnLanAccess()
    {
        if (await _authorizationPolicyProvider.GetPolicyAsync(AuthorizationConfiguration.RequireWakeOnLanRole) != null)
        {
            var requireWakeOnLanRoleResult =
                await _authorizationService.AuthorizeAsync(User, null,
                    AuthorizationConfiguration.RequireWakeOnLanRole);
            return requireWakeOnLanRoleResult.Succeeded;
        }
        return true;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!await UserIsAuthorizedForWakeOnLanAccess())
        {
            return Forbid();
        }
        var computers = await _computerService.GetAllComputersWithAwakeStatus();
        return Ok(computers);
    }

    [HttpGet]
    [Route("{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        if (!await UserIsAuthorizedForWakeOnLanAccess())
        {
            return Forbid();
        }
        var computer = await _computerService.GetComputerWithAwakeStatusByName(name);
        if(computer == null){
            return NotFound();
        }
        return Ok(computer);
    }

    [HttpDelete]
    [Route("{name}")]
    public async Task<IActionResult> DeleteByName(string name)
    {
        if (!await UserIsAuthorizedForWakeOnLanAccess())
        {
            return Forbid();
        }
        var computer = _computerService.GetComputerByName(name);
        if(computer == null){
            return NotFound();
        }
        _computerService.Delete(name);
        return Ok();
    }

    [HttpPost]
    [Route("{name}/wakeup")]
    public async Task<IActionResult> WakeUpByName(string name){

        if (!await UserIsAuthorizedForWakeOnLanAccess())
        {
            return Forbid();
        }
        var computer = _computerService.GetComputerByName(name);
        if(computer == null){
            return NotFound();
        }
        await computer.WakeUp();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> AddComputer(Computer computer){
        if (!await UserIsAuthorizedForWakeOnLanAccess())
        {
            return Forbid();
        }
        var validator = new ComputerValidator(_computerService);
        var validationResult = validator.Validate(computer);
        if(!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            var validationProblemDetails = _problemDetailsFactory.CreateValidationProblemDetails(Request.HttpContext, ModelState);
            return BadRequest(validationProblemDetails);
        }
        _computerService.AddComputer(computer);
        var createdComputer = await _computerService.GetComputerWithAwakeStatusByName(computer.Name);
        return CreatedAtAction
        (
            nameof(GetByName),
            new { createdComputer.Name },
            createdComputer
        );
    }

    [HttpPut]
    [Route("{name}")]
    public async Task<IActionResult> EditPortForwarding(string name, Computer computer){
        if (!await UserIsAuthorizedForWakeOnLanAccess())
        {
            return Forbid();
        }
        
        var existingComputer = _computerService.GetComputerByName(name);
        if (existingComputer == null){
            return NotFound();
        }
        var validator = new ComputerValidator(_computerService, previousName: name);
        var validationResult = validator.Validate(computer);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            var validationProblemDetails = _problemDetailsFactory.CreateValidationProblemDetails(Request.HttpContext, ModelState);
            return BadRequest(validationProblemDetails);
        }
        
        _computerService.EditComputer(name, computer);

        return Ok();
    }
}