using Backend.Domain.Models;
using Backend.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IdentityService _identityService;
    private readonly AppDbContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, IdentityService identityService, AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _ctx = context;
        _roleManager = roleManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _identityService = identityService;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser register)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var identity = new IdentityUser
        {
            Email = register.Email,
            UserName = register.Email,
            PhoneNumber = register.PhoneNumber
        };

        var createdIdentityResult = await _userManager.CreateAsync(identity, register.Password);
        if (!createdIdentityResult.Succeeded)
        {
            return BadRequest(createdIdentityResult.Errors);
        }

        var newClaims = new List<Claim>
        {
            new("FirstName", register.FirstName),
            new("LastName", register.LastName)
        };

        var claimsResult = await _userManager.AddClaimsAsync(identity, newClaims);
        if (!claimsResult.Succeeded)
        {
            await _userManager.DeleteAsync(identity); // Rollback user creation
            return BadRequest(claimsResult.Errors);
        }

        IdentityRole role = null;
        string roleName = (register.Role ?? Role.Student).ToString();

        role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            role = new IdentityRole(roleName);
            var roleResult = await _roleManager.CreateAsync(role);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(identity); // Rollback user creation
                return BadRequest(roleResult.Errors);
            }
        }

        var roleAssignResult = await _userManager.AddToRoleAsync(identity, roleName);
        if (!roleAssignResult.Succeeded)
        {
            await _userManager.DeleteAsync(identity); // Rollback user creation
            return BadRequest(roleAssignResult.Errors);
        }

        newClaims.Add(new Claim(ClaimTypes.Role, roleName));

        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, identity.Email ?? throw new InvalidOperationException()),
            new(JwtRegisteredClaimNames.Email, identity.Email ?? throw new InvalidOperationException()),
            new("UserId", identity.Id ?? throw new InvalidOperationException())
        });

        claimsIdentity.AddClaims(newClaims);

        var token = _identityService.CreateSecurityToken(claimsIdentity);
        var response = new AuthenticationResult(_identityService.WriteToken(token));

        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginUser login)
    {
        var user = await _userManager.FindByNameAsync(login.Email);
        if (user is null) return BadRequest();

        var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
        if (!result.Succeeded) return BadRequest("Coudln't sign in");

        var roles = await _userManager.GetRolesAsync(user);

        var claims = await _userManager.GetClaimsAsync(user);

        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Email ?? throw new InvalidOperationException()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? throw new InvalidOperationException()),
            new("UserId", user.Id ?? throw new InvalidOperationException()),
            //check to see if entities can be retireved by full name (first+last)
        });

        claimsIdentity.AddClaims(claims);
        foreach (var role in roles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        var token = _identityService.CreateSecurityToken(claimsIdentity);

        var response = new AuthenticationResult(_identityService.WriteToken(token));
        return Ok(response);
    }


    public enum Role
    {
        Teacher,
        Student,
        Admin,
        Parent
    }

    public record RegisterUser(Role? Role, string Email, string Password, string LastName, string FirstName, int Age, string PhoneNumber, string Address, string ParentEmail, string ParentName);
    public record AuthenticationResult(string Token);
    public record LoginUser(string Email, string Password);
}
