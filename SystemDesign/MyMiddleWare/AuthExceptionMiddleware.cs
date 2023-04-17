using System.Net;

namespace SystemDesign.MyMiddleWare {
  //public class AuthExceptionMiddleware {
  //  readonly RequestDelegate _request;

  //  public AuthExceptionMiddleware(RequestDelegate request) {
  //    _request = request;
  //  }

  //  public async Task InvokeAsync(HttpContext context) {
  //    try {
  //      await _request(context);
  //    }
  //    catch (UnauthorizedAccessException ex) {
  //      context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
  //      var error = ex.Message;
  //      await context.Response.WriteAsJsonAsync(error);
  //    }
  //    catch(Exception ex) {
  //      throw ex;
  //    }
  //  }
  //}
}
