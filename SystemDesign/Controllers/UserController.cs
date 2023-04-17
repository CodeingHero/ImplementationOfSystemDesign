
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Text.RegularExpressions;
using SystemDesign.Models;
using SystemDesign.UserManager;
using SystemDesign.UserManager.Services;

namespace SystemDesign.Controllers {

  [Route("user/[action]")]
  [ApiController]
  public class UserController : ControllerBase {
    private  readonly UserService userService;
    private  readonly  IWebHostEnvironment webHostEnvironment;
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserController(UserService loginService, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) {
      this.userService = loginService;
      this.webHostEnvironment = webHostEnvironment;
      this.httpContextAccessor = httpContextAccessor;
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(UserRequest userRequest)
    {
      var res = await userService.LoginAsync(userRequest.ToUserIdentityDTO());
      if (!res.Result.Succeeded)
        return BadRequest(new CommonResponse<LoginResponse>(null) {
          IsSuccess = false,
          IsError =true,
          Message = "Login Failed",
          Errors =  res.Result.Errors.Select(err => err.Description).ToList(),
        });
      return Ok(new CommonResponse<LoginResponse>(res.ToResponse() as LoginResponse) {
        IsSuccess = true,
        IsError = false,
        Message = "Login succeed"
      });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Info() {
      var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
      var res = await userService.GetUserInfoAsync(new UserDetail { UserID = id });
      if(res != null) {
        return Ok(new CommonResponse<UserDetail> {
          IsSuccess = true,
          IsError = false,
          Message = "User info get succeed",
          Data = new UserDetail {
            UserID = res.Id.ToString(),
            UserName = res.UserName,
            Email = res.Email,
            Portrait = null
          }
        });
      }
      return BadRequest(new CommonResponse<object> { IsSuccess = true,Message ="Cant find user information.Please try again later" });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Sign_Out() {
      var res = await userService.SignOutAsync();
      if (res.success)
        return Ok(new CommonResponse<object> (null) {
          IsSuccess = true,
          IsError = false,
          Message = "Logout succeed"
        });
      return BadRequest(new CommonResponse<object> (null) {
        IsSuccess = false,
        IsError = true,
        Message = "Logout failed",
        Errors = new List<string> { res.msg }
      });
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRequest userRequest) {
      var res = await userService.CreateNewUserAsync(userRequest.ToUserIdentityDTO());
      if (!res.Result.Succeeded)
        return BadRequest(new CommonResponse<RegisterResponse> (null) {
          IsSuccess = false,
          IsError = true,
          Message = "Registration failed",
          Errors = res.Result.Errors.Select(err => err.Description).ToList()
        });
      return Ok(new CommonResponse<RegisterResponse> (res.ToResponse() as RegisterResponse) {
        IsSuccess = true,
        IsError = false,
        Message = "Registration succeed"
      });
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateUser(UserUpdateRequest userUpdateInfo) {
      var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (userUpdateInfo.UserID != id ) {
        return StatusCode(StatusCodes.Status403Forbidden,new CommonJsonResponse (null) {
          IsSuccess = false,
          IsError = true,
          Message = "Update failed",
          Errors = new List<string> { "You can only update your own information" }
        });
      }
      var res = await userService.UpdateUserInformationAsync(userUpdateInfo);

      if (res.Result.Succeeded) {
        return Ok(new CommonJsonResponse(new UserDetail {
              Email = res.User.Email,
              UserName = res.User.UserName,
              NickName = res.User.NickName,
              UserID = res.User.Id.ToString()
            }) {
          IsSuccess = true,
          IsError = false,
          Message = "Update succeed",
        });
      }
      return BadRequest(new CommonJsonResponse(null) {
        IsSuccess = false,
        IsError = true,
        Message = "Update failed",
        Errors = res.Result.Errors.Select(err => err.Description).ToList()
      });
    }
  }

  public class UserRequest {
    public string? UserKey { get; set; }
    public string? Password { get; set; }
  }
 
  public class UserUpdateRequest : UserUpdateInfo {
    
  }

  public static class UserRequestExtensions {
    public static IdentityDTO ToUserIdentityDTO(this UserRequest userRequest) {
      var res = new IdentityDTO { Password = userRequest.Password};
      if (Regex.IsMatch(userRequest.UserKey, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}$")) {
        res.Email = userRequest.UserKey;
      }
      else {
        res.UserName = userRequest.UserKey;
      }
      return res;
    }

    public static UserBaseInfo ToUserBaseInfo(this UserRequest userRequest) {
      var res = new UserBaseInfo();
      if (Regex.IsMatch(userRequest.UserKey, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}$")) {
        res.Email = userRequest.UserKey;
      }
      else {
        res.UserName = userRequest.UserKey;
      }
      return res;
    }
  } 
}

namespace SystemDesign.Models {
  public static partial class ConvertExtensions {
    //public static LoginResponse ToResponse(this LoginResult @this) {
    //  return new LoginResponse {
    //    UserID = @this.User?.Id,
    //    Email = @this.User.Email,
    //    UserName = @this.User.UserName,
    //  };
    //}
    public static IdentityResponseBase ToResponse(this UserIdentifyResultBase @this) {
      if(@this.GetType() == typeof(LoginResult)) { 
        return new LoginResponse {
          UserID = @this.User?.Id.ToString(),
          Email = @this.User.Email,
          UserName = @this.User.UserName,
          Token = @this.Token
        };
      }
      else if(@this.GetType() == typeof(RegisterResult) ){
        return new RegisterResponse {
          UserID = @this.User?.Id.ToString(),
          Email = @this.User.Email,
          UserName = @this.User.UserName,
          Token = @this.Token
        };
      }
      throw new ArgumentException();
    }
  }
}
