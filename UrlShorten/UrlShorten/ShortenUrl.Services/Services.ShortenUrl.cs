using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.ComponentModel;
using SystemDesign.EF;
using SystemDesign.EF.Models;
using SystemDesign.Utils;

namespace SystemDesign.ShortenUrl.Services {
  public class ShortenUrlService
  {
    //private readonly MyDBContext ctx;
    private readonly IServiceProvider serviceProvider;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly IConfiguration config;
    private readonly IOptionsSnapshot<WebSetting> webSetting;

    public ShortenUrlService( IServiceScopeFactory serviceScopeFactory,IConfiguration config)
    {
      this.serviceScopeFactory = serviceScopeFactory;
      this.config = config;
    }

    public async Task<ShortenUrlEntity?> AddNewUrlAsync(long userId, string oriUrl)
    {
      using var scope = serviceScopeFactory.CreateAsyncScope();
      MyDBContext ctx = scope.ServiceProvider.GetRequiredService<MyDBContext>();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SDUser>>();
      if (!IsUrlValid(oriUrl))
        throw new ArgumentException($"Not a valid URL={oriUrl}");
      var url  = await ctx.ShortenUrls
        .FirstOrDefaultAsync(item => 
          oriUrl == item.OriginalUrl 
          && userId == item.OwnerUser.Id 
          && DateTime.Now <= item.ExpirationTime);
      if (url != null)
        return url;
      url = await GetAvailableUrlAsync(ctx);
      if (url == null)
        return null;
      url.OwnerUser = await userManager.FindByIdAsync(userId.ToString());
      url.OriginalUrl = oriUrl;
      url.CreationTime = DateTime.Now;
      url.ExpirationTime = url.CreationTime?.AddDays(30) ?? DateTime.Now.AddDays(30);
      await ctx.SaveChangesAsync();
      return url;
    }

    static public string Formaturl() {
      throw new NotImplementedException();
    }

    static public string CombineShortenUrl(string url,string domain) {
      return $"https://{domain}/s/{url}";
    }

    public async Task<bool> CheckIfOriUrlExist(long userId, string url)
    {
      using var scope = serviceScopeFactory.CreateAsyncScope();
      MyDBContext ctx = scope.ServiceProvider.GetRequiredService<MyDBContext>();
      return await ctx.ShortenUrls.AnyAsync(item => url == item.OriginalUrl && userId == item.OwnerUser.Id && item.ExpirationTime <= DateTime.Now);
    }

    public async Task TestAsync() {
      using var scope = serviceScopeFactory.CreateAsyncScope();
      MyDBContext ctx = scope.ServiceProvider.GetRequiredService<MyDBContext>();
      var x = await ctx.ShortenUrls
        .Include(s => s.OwnerUser)
        .FirstAsync();
      _ = x;
    }

    public async Task<ShortenUrlEntity?> GetAvailableUrlAsync(MyDBContext ctx)
    { 
      async Task<ShortenUrlEntity?> TryGetUnused() =>
         await ctx.ShortenUrls.FirstOrDefaultAsync(item => item.OwnerUser == null);
      var url = await TryGetUnused();
      if(url != null)
        return url;
      await CleanExpiredDataAsync(ctx);
      if ((url = await TryGetUnused()) != null)
        return url;
      await GenerateNewKeysAsync(ctx);
      url = await TryGetUnused();
      return url;
    }

    public async Task GenerateNewKeysAsync(MyDBContext ctx,long increase = 1000)
    {
      long maxKey = await ctx.ShortenUrls.AnyAsync() ? await ctx.ShortenUrls.MaxAsync(x => x.UrlKey) : 0;
      if (maxKey + 1 + increase > 56800235583)
        return;
      await ctx.ShortenUrls
        .AddRangeAsync(LongRange(0, increase).Select(key => new ShortenUrlEntity()));
      await ctx.SaveChangesAsync();

      foreach(var record in ctx.ShortenUrls.Where(x => string.IsNullOrEmpty(x.ShortenedUrl))){
        record.ShortenedUrl = record.UrlKey.HexTo62();
      }
      await ctx.SaveChangesAsync();
    }

    public async Task<bool?> RemoveShortUrlAsync(long userId, string sUrl)
    {
      using var scope = serviceScopeFactory.CreateAsyncScope();
      MyDBContext ctx = scope.ServiceProvider.GetRequiredService<MyDBContext>();
      var record = await ctx.ShortenUrls
        .Include(s => s.OwnerUser)
        .SingleOrDefaultAsync(item => item.ShortenedUrl  == sUrl && item.OwnerUser.Id == userId);
      if (record == null)
        return null;
      record.Reset();
      await ctx.SaveChangesAsync();
      return true;
    }

    private async Task<long> CleanExpiredDataAsync(MyDBContext ctx) {
      var expiredData = ctx.ShortenUrls.Where(item => item.ExpirationTime >= DateTime.Now);
      var cnt = await expiredData.LongCountAsync();
      foreach (var item in expiredData)
      {
        item.Reset();
      }
      await ctx.SaveChangesAsync();
      return cnt;
    }

    public async Task<List<ShortenUrlEntity>> GetUrlListByUserIDAsync(long userId) {
      using var scope = serviceScopeFactory.CreateAsyncScope();
      MyDBContext ctx = scope.ServiceProvider.GetRequiredService<MyDBContext>();
      return await ctx.ShortenUrls
        .Include(s => s.OwnerUser)
        .Where(item => item.OwnerUser!.Id == userId)
        .OrderBy(item => item.CreationTime)
        .ThenBy(item => item.ExpirationTime)
        .ToListAsync();
    }

    public async Task<string?> GetOriginalUrl(string shortenUrl) {
      Console.WriteLine(DateTime.Now);
      using var scope = serviceScopeFactory.CreateAsyncScope();
      MyDBContext ctx = scope.ServiceProvider.GetRequiredService<MyDBContext>();
      var record = await ctx.ShortenUrls
        .SingleOrDefaultAsync(item => item.ShortenedUrl == shortenUrl);
      return record?.OriginalUrl;
    }

    public async Task<ShortenUrlEntity?> GetUrlByKeyAsync(long urlKey) { 
      using var scope = serviceScopeFactory.CreateAsyncScope();
      MyDBContext ctx = scope.ServiceProvider.GetRequiredService<MyDBContext>();
      var record = await ctx.ShortenUrls
        .Include(s => s.OwnerUser)
        .SingleOrDefaultAsync(item => item.UrlKey == urlKey);
      return record;
    }

    public async Task<ShortenUrlEntity?> AddUrlExpirationTimeAsync(long urlKey,int days) {
      using var scope = serviceScopeFactory.CreateAsyncScope();
      MyDBContext ctx = scope.ServiceProvider.GetRequiredService<MyDBContext>();
      var record = await ctx.ShortenUrls
        .Include(s => s.OwnerUser)
        .SingleOrDefaultAsync(item => item.UrlKey == urlKey);
      if (record == null)
        return null;
      record.ExpirationTime = record.ExpirationTime?.AddDays(days) ?? DateTime.Now.AddDays(days);
      await ctx.SaveChangesAsync();
      return record;
    }

    public bool IsUrlValid(string url)
    {
      return true;
      //var res = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult);
      //return res;
        //&& uriResult?.Scheme == Uri.UriSchemeHttp
        //|| uriResult?.Scheme == Uri.UriSchemeHttps;
    }
    private static IEnumerable<long> LongRange(long start, long count) {
      var limit = start + count;

      while (start < limit) {
        yield return start;
        start++;
      }
    }
  }
}
