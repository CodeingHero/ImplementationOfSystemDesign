using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemDesign.ShortenUrl.Services;

namespace SystemDesign.ShortenUrl.Controllers
{
    [Route("ShortenUrlEntity/[controller]/[action]")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
        //readonly ShortenUrlService convertUrl;
        //public ConverterController(ShortenUrlService convertUrl)
        //{
        //    this.convertUrl = convertUrl;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult<string> CreateUrl(string url)
        //{
        //    if (!convertUrl.IsLegalUrl(url))
        //        return BadRequest("Requested url is not a valid url");
        //    var customeUrl = convertUrl.CreateUrl(url);
        //    return @"https://127.0.0.1/" + customeUrl.Substring(0, 6);
        //}

        //[HttpGet]
        //public ActionResult DeleteUrl(string url)
        //{
        //    if (!convertUrl.IsLegalUrl(url))
        //        return BadRequest($"{url} is not an ilegal URL");
        //    if (!convertUrl.IsShortenUrlExist(url))
        //        return NotFound($"{url} not found");
        //    return Ok($"{url} has been deleted permanently");
        //}
    }
}
