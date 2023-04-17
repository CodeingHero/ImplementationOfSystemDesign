using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SystemDesign.EF;
using SystemDesign.EF.Models;
namespace SystemDesign.UserManager.Services {
  public class JwtBuilder {
    private readonly IServiceProvider serviceProvider;

    public JwtBuilder(IServiceProvider serviceProvider) {
      this.serviceProvider = serviceProvider;
    }

    private static string BuildToken(
        IEnumerable<Claim> claims, 
        JwtOption options
      ) {
      DateTime expires = DateTime.Now.AddHours(options.AccessExpireHours);
      byte[] keyBytes = Encoding.UTF8.GetBytes(options.SigningKey.ToCharArray());
      var secKey = new SymmetricSecurityKey(keyBytes);
      var credentials = new SigningCredentials(secKey,
        SecurityAlgorithms.HmacSha256Signature);
      var token = new JwtSecurityToken(expires: expires,
        signingCredentials: credentials, claims: claims);
      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GetAccessTokenAsync(SDUser user, JwtOption jwtOption) {
      using (var scope = serviceProvider.CreateScope()) {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SDUser>>();
        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<SDUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
        //Create Claim
        var claims = new List<Claim> {
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(ClaimTypes.Name,user.UserName),
          new Claim(ClaimTypes.Expiration,DateTime.Now.AddSeconds(jwtOption.AccessExpireHours*60*60).ToString())
        };
        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles) {
          claims.Add(new Claim(ClaimTypes.Role, role));
        }
        string jwtToken = BuildToken(claims, jwtOption);
        return jwtToken;
      }
    }

    public async Task<string> GetRefreshTokenAsync(SDUser user, JwtOption jwtOption) {
      using (var scope = serviceProvider.CreateScope()) {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SDUser>>();
        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<SDUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
        //Create Claim
        var claims = new List<Claim> {
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(ClaimTypes.Name,user.UserName),
          new Claim(ClaimTypes.Expiration,DateTime.Now.AddSeconds(jwtOption.RefreshExpireHours*60*60).ToString())
        };
        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles) {
          claims.Add(new Claim(ClaimTypes.Role, role));
        }
        string jwtToken = BuildToken(claims, jwtOption);
        return jwtToken;
      }
    }
  }
}
