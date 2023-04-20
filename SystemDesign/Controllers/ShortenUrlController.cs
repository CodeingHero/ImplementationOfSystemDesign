using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Writers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using SystemDesign.EF;
using SystemDesign.EF.Models;
using SystemDesign.Models;
using SystemDesign.Responses;
using SystemDesign.ShortenUrl.Services;

namespace SystemDesign.Controllers {
  [Route("sd/[controller]/[action]")]
  [ApiController]
  public class ShortenUrlController : ControllerBase {
    private readonly ShortenUrlService shortenUrlService;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IConfiguration configuration;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly IOptionsSnapshot<WebSetting> webSetting;
    public ShortenUrlController(ShortenUrlService shortenUrlService, IWebHostEnvironment webHostEnvironment,IConfiguration configuration,IServiceScopeFactory serviceScopeFactory)
    {
      this.shortenUrlService = shortenUrlService;
      this.webHostEnvironment = webHostEnvironment;
      this.configuration = configuration;
      this.serviceScopeFactory = serviceScopeFactory;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUrl(string url) {
      var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
      //if (id == null)
      //  id = "1";
      var sUrl = await shortenUrlService.AddNewUrlAsync(long.Parse(id), url);
      if (sUrl == null)
        return BadRequest(new CommonResponse<string>() {
          IsSuccess = false,
          IsError = true,
          Message = "Failed to generate short url.Please try again.",
          Data = ShortenUrlService.CombineShortenUrl(sUrl.ShortenedUrl, configuration.GetSection("WebSetting").Get<WebSetting>().DomainName),
          Errors = new List<string> { "Failed to generate short url.Please try again later." }
        });
      return Ok(new CommonResponse<ShortenUrlResponse>() {
        IsSuccess = true,
        IsError = false,
        Message = "Success",
        Data = sUrl.ToResponse() 
      });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUrlsByUserID() {
      var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (userId == null)
        userId = "10000";
      var urls = await shortenUrlService.GetUrlListByUserIDAsync(long.Parse(userId));
      var res = new CommonResponse<ICollection<ShortenUrlResponse>> {
        IsSuccess = urls.Count > 0,
        IsError = false,
        Message = urls.Count > 0 ? "Success" : "No data found",
        Data = urls.Select(item => item.ToResponse()).ToList(),
      };
      return Ok(res);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUrlByKey(string urlKey) {
      var url = await shortenUrlService.GetUrlByKeyAsync(long.Parse(urlKey));
      if (url == null)
        return NotFound(new CommonResponse<ShortenUrlResponse>() {
          IsSuccess = false,
          IsError = true,
          Message = "Url not found",
          Data = null,
          Errors = new List<string> { "Url not found" }
        });
      return Ok(new CommonResponse<ShortenUrlResponse>() {
        IsSuccess = true,
        IsError = false,
        Message = "Success",
        Data = url.ToResponse(),
      });
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> AddUrlExpirationTime(string urlKey,int days) {
      if(days < 0)
        return BadRequest(new CommonResponse<ShortenUrlResponse>() {
          IsSuccess = false,
          IsError = true,
          Message = "Days must be greater than 0",
          Data = null,
          Errors = new List<string> { "Days must be greater than 0" }
        });
      var url = await shortenUrlService.AddUrlExpirationTimeAsync(long.Parse(urlKey),days);
      if (url == null)
        return NotFound(new CommonResponse<ShortenUrlResponse>() {
          IsSuccess = false,
          IsError = true,
          Message = "Url not found",
          Data = null,
          Errors = new List<string> { "Url not found" }
        });
      return Ok(new CommonResponse<ShortenUrlResponse>() {
        IsSuccess = true,
        IsError = false,
        Message = "Success",
        Data = url.ToResponse(),
      });
    }

    [HttpGet]
    [Route("/s/{url}")]
    //https://localhost:7111/s/0qX
    public async Task<IActionResult> Redirection(string url) {
      var oriUrl = await shortenUrlService.GetOriginalUrl(url);
      if (oriUrl == null)
        return NotFound("Original url not found!");
      return Redirect("https://"+ oriUrl);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUrlByKey(string sUrl) {
      var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (userId == null)
        userId = "10000";
      var res = await shortenUrlService.RemoveShortUrlAsync(long.Parse(userId), sUrl);
      if (res == null)
        return NotFound($"{sUrl} not found");
      if (res == false)
        return BadRequest($"Remove {Url} failed.Please try again later");
      return Ok($"{sUrl} has been deleted permanently");
    }
  }
}
namespace SystemDesign.Responses {
  public static partial class ConvertExtensions {
    public static ShortenUrlResponse ToResponse(this ShortenUrlEntity @this) {
      return new ShortenUrlResponse {
        UserID = @this.OwnerUser.Id.ToString(),
        UrlKey = @this.UrlKey.ToString(),
        OriginalUrl =  @this.OriginalUrl,
        ShortenedUrl = "/s/" + @this.ShortenedUrl,
        CreationTime = @this.CreationTime,
        ExpirationTime = @this.ExpirationTime,
      };
    }
  }
}
