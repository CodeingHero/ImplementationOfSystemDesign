using System.Runtime.CompilerServices;
using SystemDesign.EF.Models;

namespace SystemDesign.Services {
  public class IndexService {
    private readonly IConfiguration configuration;

    public IndexService(IConfiguration configuration) {
      this.configuration = configuration;
    }

    public async Task<Introduction> GetIndroductionAsync() {
      var res = configuration.GetSection("Introduction").Get<Introduction>();
      return await Task.FromResult(res);
    }
  }
}
