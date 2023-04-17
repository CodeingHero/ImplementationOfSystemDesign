using Microsoft.AspNetCore.Identity;
using SystemDesign.EF.Models;
using SystemDesign.EF;
namespace SystemDesing.UserManager.Services {
  public class Auth
  {
    private readonly WebApplicationBuilder webApplicationBuilder;

    public Auth(WebApplicationBuilder webApplicationBuilder)
    {
      this.webApplicationBuilder = webApplicationBuilder;
    }

    public IdentityBuilder GetIdentityBuilder()
    {
      var idBuilder = new IdentityBuilder(typeof(SDUser), typeof(UserRole), webApplicationBuilder.Services);
      idBuilder.AddEntityFrameworkStores<MyDBContext>()
        .AddDefaultTokenProviders()
        .AddRoleManager<RoleManager<UserRole>>()
        .AddUserManager<UserManager<SDUser>>();
      return idBuilder;
    }
  }
}
