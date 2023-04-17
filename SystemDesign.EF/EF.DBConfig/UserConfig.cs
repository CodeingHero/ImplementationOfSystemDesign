using SystemDesign.EF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SystemDesign.EF {

  public class UserRole : IdentityRole<long> {

  }

  public class UserConfig
  {
    public static void ConfigureUserOptions(IServiceCollection services) {
      services.AddIdentityCore<SDUser>(opt => {
        opt.Password.RequireDigit = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequiredLength = 6;
        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
        opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
        opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
        //opt.User.RequireUniqueEmail = true;
      });
    }
  }
}