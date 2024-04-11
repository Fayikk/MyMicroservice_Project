using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService;



public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    public CustomProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        var roles = await _userManager.GetRolesAsync(user);


        var existingClaims = await _userManager.GetClaimsAsync(user);
        var claims = new List<Claim>(){
            new Claim("username",user.UserName),
            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim("role",roles.Last().ToString())
        };

        context.IssuedClaims.AddRange(claims);
        context.IssuedClaims.Add(existingClaims.FirstOrDefault(x=>x.Type == JwtClaimTypes.Name));
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}