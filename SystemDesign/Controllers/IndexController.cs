using Microsoft.AspNetCore.Mvc;
using SystemDesign.Models;
using SystemDesign.Services;

namespace SystemDesign.Controllers {

  [Route("sd/[controller]/[action]")]
  [ApiController]
  public class IndexController : ControllerBase {

    private readonly IndexService indexService;

    public IndexController(IndexService indexService) {
      this.indexService = indexService;
    }

    [HttpGet]
    [Route("~/sd/[controller]/Introduction")]
    public async Task<IActionResult> GetIntroduction() {
      return Ok(new CommonJsonResponse(await indexService.GetIndroductionAsync()));
    }

    [HttpGet]
    [Route("~/sd/[controller]/About")]
    public async Task<IActionResult> GetAbout() {
      return Ok(new CommonJsonResponse("About"));
    }
  }
}
