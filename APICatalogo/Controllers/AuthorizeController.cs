using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APICatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    
    public AuthorizeController(
        UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return "AuthorizeController :: Acessado em : " + DateTime.Now.ToLongDateString();
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] UserDTO userDTO)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
        
        var user = new IdentityUser
        {
            UserName = userDTO.Email,
            Email = userDTO.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, userDTO.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);


        await _signInManager.SignInAsync(user, false);
        return Ok(GenerateToken(userDTO));
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserDTO userDTO)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
        
        var result = await _signInManager.PasswordSignInAsync(userDTO.Email, userDTO.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return Ok(GenerateToken(userDTO));
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Login Inválido...");
            return BadRequest(ModelState);
        }
    }

    private UserTokenDTO GenerateToken(UserDTO userDto)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userDto.Email),
            new Claim("meuPet", "spyke"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var expiration = _configuration["TokenConfigurations:ExpireHours"];
        var expirationDate = DateTime.UtcNow.AddHours(double.Parse(expiration!));
        
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["TokenConfigurations:Issuer"],
            audience: _configuration["TokenConfigurations:Audience"],
            claims: claims,
            expires: expirationDate,
            signingCredentials: credentials
        );

        return new UserTokenDTO()
        {
            Authenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expirationDate,
            Message = "Token JWT Ok"
        };
    }
    
}