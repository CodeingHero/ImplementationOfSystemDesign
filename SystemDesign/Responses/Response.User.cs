using Microsoft.AspNetCore.Identity;
using SystemDesign.UserManager.Services;

namespace SystemDesign.Models {


  public abstract class IdentityResponseBase {
    public string? UserID { get; set;}
    public string? UserName { get; set;}
    public string? Email { get; set;}
    public Tokens? Token { get; set;}
  }


  public class LoginResponse : IdentityResponseBase {
  }
  public class RegisterResponse : IdentityResponseBase {
  }

}
