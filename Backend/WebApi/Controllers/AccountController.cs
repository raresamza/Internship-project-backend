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
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IdentityService _identityService;
    private readonly AppDbContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;    

    public AccountController(RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, IdentityService identityService, AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _ctx= context;
        _roleManager= roleManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _identityService = identityService;
        _userManager= userManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterUser register)
    {
        var identity = new IdentityUser { Email = register.Email, UserName = register.Email };
        var createdIdentity = await _userManager.CreateAsync(identity, register.Password);
        var newClaims = new List<Claim>
        {
            new("FirstName", register.FirstName),
            new("LastName", register.LastName),
        };
        await _userManager.AddClaimsAsync(identity, newClaims);

        if (register.Role == Role.Teacher)
        {
            var role = await _roleManager.FindByNameAsync("Teacher");
            if (role == null)
            {
                role = new IdentityRole("Teacher");
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(identity, "Teacher");

            newClaims.Add(new Claim(ClaimTypes.Role, "Teacher"));
        }
        else
        {
            var role = await _roleManager.FindByNameAsync("Student");
            if (role == null)
            {
                role = new IdentityRole("Student");
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(identity, "Student");

            newClaims.Add(new Claim(ClaimTypes.Role, "Student"));
        }

        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, identity.Email ?? throw new InvalidOperationException()),
            new(JwtRegisteredClaimNames.Email, identity.Email ?? throw new InvalidOperationException()),
            new("UserId", identity.Id ?? throw new InvalidOperationException()),
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
        var user=await _userManager.FindByNameAsync(login.Email);
        if (user is null) return BadRequest();

        var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
        if (!result.Succeeded) return BadRequest("Coudln't sign in");

        var roles= await _userManager.GetRolesAsync(user);

        var claims = await _userManager.GetClaimsAsync(user);

        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Email ?? throw new InvalidOperationException()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? throw new InvalidOperationException()),
            new("UserId", user.Id ?? throw new InvalidOperationException()),
        });

        claimsIdentity.AddClaims(claims);
        foreach(var role in roles)
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
        Student
    }

    public record RegisterUser (Role Role,string Email,string Password,string LastName,string FirstName,int Age,int PhoneNumber,string Address, string ParentEmail,string ParentName);
    public record AuthenticationResult (string Token);
    public record LoginUser(string Email, string Password);
}
