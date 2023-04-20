using Microsoft.AspNetCore.Identity;
using SystemDesign.EF.Models;
using SystemDesign.EF;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Internal;

namespace SystemDesign.UserManager.Services {
  public class UserService {
    private readonly IServiceProvider serviceProvider;
    private readonly JwtBuilder jwtBuilder;
    private readonly IHttpContextAccessor httpContextAccessor;
    public UserService(IServiceProvider serviceProvider, JwtBuilder jwtBuilder, IHttpContextAccessor httpContextAccessor) {
      this.serviceProvider = serviceProvider;
      this.jwtBuilder = jwtBuilder;
      this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<RegisterResult> CreateNewUserAsync(IdentityDTO userDTO)
    {
      var res = await CreateNormalUserAsync(userDTO);
      return res;
    }

    public async Task<LoginResult> LoginAsync(IdentityDTO userDto)
    {
      using (var scope = serviceProvider.CreateScope()) {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SDUser>>();
        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<SDUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
        var sdUser = !string.IsNullOrEmpty(userDto.UserName) ? await userManager.FindByNameAsync(userDto.UserName)
          : await userManager.FindByNameAsync(userDto.Email);
        if (sdUser == null) {
          return new LoginResult {
            Result = IdentityResult.Failed(new IdentityError { Description = "Invalid username/email address or password." }),
            Token = null,
            User = null
          };
        }
        var result = await signInManager.CheckPasswordSignInAsync(sdUser, userDto.Password, false);

        if (!result.Succeeded) {
          return new LoginResult {
            Result = IdentityResult.Failed(new IdentityError { Description = "Invalid username/email address or password." }),
            Token = null,
            User = null
          };
        }
        var jwtOption = scope.ServiceProvider.GetRequiredService<IOptions<JwtOption>>().Value;
        var accToken = await GetUserTokenAsync(sdUser, jwtOption);
        var refToken = await jwtBuilder.GetRefreshTokenAsync(sdUser, jwtOption);
        return new LoginResult {
          Result = IdentityResult.Success,
          Token = new Tokens {
            AccessToken = accToken,
            RefreshToken = refToken,
          },
          User = sdUser,
        };
      }
    }

    private async Task<string> GetUserTokenAsync(SDUser user,JwtOption jwtOption) {
      using (var scope = serviceProvider.CreateScope()) {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SDUser>>();
        string jwtToken = await jwtBuilder.GetAccessTokenAsync(user, jwtOption);
        return jwtToken;
      }
    }

    public async Task<SDUser?> GetUserInfoAsync(UserBaseInfo userDTO) {
      using (var scope = serviceProvider.CreateScope()) {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SDUser>>();
        SDUser? sdUser = null;
        if (!string.IsNullOrEmpty(userDTO.UserID))
          sdUser = await userManager.FindByIdAsync(userDTO.UserID);
        else if (!string.IsNullOrEmpty(userDTO.UserName)) {
          sdUser = await userManager.FindByNameAsync(userDTO.UserName);
        }
        else if (!string.IsNullOrEmpty(userDTO.Email)) {
          sdUser = await userManager.FindByEmailAsync(userDTO.Email);
        }
        return sdUser;
      }
    }

    
    /// <summary>
    /// Create a new normal(not admin) user
    /// </summary>
    /// <param name="userDto"></param>
    /// <returns></returns>
    public async Task<RegisterResult> CreateNormalUserAsync(IdentityDTO userDto) {
      
      using (var scope = serviceProvider.CreateScope()) {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SDUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
        bool roleExists = await roleManager.RoleExistsAsync("Normal");
        if (!roleExists) {
          UserRole role = new UserRole { Name = "Normal" };
          var resCreateRole = await roleManager.CreateAsync(role);
          if (!resCreateRole.Succeeded) {
            return new RegisterResult { Result = resCreateRole };
          }
        }
        
        var user = new SDUser {UserName = userDto.UserName, Email = userDto.Email,CreationTime = DateTime.Now };
        var resCreateUser = await userManager.CreateAsync(user, userDto.Password);
        if (!resCreateUser.Succeeded) {
          return new RegisterResult { Result = resCreateUser};
        }
        resCreateUser = await userManager.AddToRoleAsync(user, "Normal");
        var loginR = await LoginAsync(userDto);
        return new RegisterResult { Result = resCreateUser, Token = loginR.Token, User = loginR.User};
      }
    }

    public async Task<UpdateResult> UpdateUserInformationAsync(UserUpdateInfo userUpdateInfo) {
      using (var scope = serviceProvider.CreateScope()) {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SDUser>>();
        var user = await userManager.FindByIdAsync(userUpdateInfo.UserID);
        if(user == null) {
          return new UpdateResult { Result = IdentityResult.Failed(new IdentityError { Description = "User not found." }), User = null };
        }
        //Update Password
        if(userUpdateInfo.NewPassword != userUpdateInfo.ConfirmPassword) {
          return new UpdateResult { Result = IdentityResult.Failed(new IdentityError { Description = "Password and confirm password do not match." }), User = user };
        }
        if (!string.IsNullOrEmpty(userUpdateInfo.NewPassword)) {
          if(await userManager.CheckPasswordAsync(user, userUpdateInfo.PrevPassword) == false) {
            return new UpdateResult { Result = IdentityResult.Failed(new IdentityError { Description = "Invalid password." }) };
          }
          var pwUpdRes = await userManager.ChangePasswordAsync(user, userUpdateInfo.PrevPassword, userUpdateInfo.NewPassword);
          if(!pwUpdRes.Succeeded) {
            return new UpdateResult { Result = pwUpdRes,User = user};
          }
        }
        //Update other
        user.NickName = userUpdateInfo.NickName;
        user.Email = userUpdateInfo.Email;
        var res = await userManager.UpdateAsync(user);
        return new UpdateResult { Result = res, User = user };
      }
    }
    public async Task<(bool success,string msg)> SignOutAsync() {
        //pretend to sign out a user successfully
        return await Task.FromResult( (true, "SignOut succeed!"));
    }
  }

  public class IdentityDTO : UserBaseInfo {
    public string Password { get; set; }
  }

  public class UserBaseInfo { 
    public string UserID { get; set; }
    public string UserName { get; set;}
    public string Email { get; set; }
  }

  public class UserDetail : UserBaseInfo {
    public string NickName { get; set; }
    public string Portrait { get;set; }
  }

  public class UserUpdateInfo : UserDetail {
    public string PrevPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
  }

  public class Tokens {
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
  }

  public abstract class UserIdentifyResultBase {
    public Tokens? Token { get; set; } = null;
    public IdentityResult Result { get; set; }
    public SDUser? User { get; set; } = null;
  }

  public class LoginResult : UserIdentifyResultBase {

  }

  public class RegisterResult : UserIdentifyResultBase {

  }

  public class UpdateResult {
    public IdentityResult Result { get; set; }
    public SDUser? User { get; set; } = null;
  }
}
