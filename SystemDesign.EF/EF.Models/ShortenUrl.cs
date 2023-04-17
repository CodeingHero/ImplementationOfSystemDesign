using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace SystemDesign.EF.Models {
  public class ShortenUrlEntity  {
    //public ShortenUrlEntity(long urlKey)
    //{
    //  //UrlKey = urlKey;
    //  ShortenedUrl = urlKey.HexTo62();
    //}
    public SDUser? OwnerUser { get; set; }
    public long? OwnerUserId { get; set; }
    public ICollection<SDUser> OwnerUsers { get; set; }
    public long UrlKey { get; init; }
    public string? OriginalUrl { get; set; }
    public string? ShortenedUrl { get; set; }
    public DateTime? CreationTime { get; set; }
    public DateTime? ExpirationTime { get; set; }
  }

  public class ShortenUrlConfigEntity
  {
    public  DateTime DefaultExirationTime { get; set; }
    public  int IncreaseNumber { get; set; }
    public string Domain { get; set;}
  }

  public class ShortenUrlDBConfig : IEntityTypeConfiguration<ShortenUrlEntity> {
    public void Configure(EntityTypeBuilder<ShortenUrlEntity> builder) {
      builder.ToTable("T_ShortenUrl");
      builder.HasKey(e => e.UrlKey);
      //builder.HasOne<SDUser>(navigationName: "OwnerUser")
      //  .WithMany();

      builder.Property(e => e.UrlKey);
      builder.Property(e => e.OriginalUrl)
        .IsUnicode(true)
        .HasMaxLength(190000);
      builder.Property(e => e.ShortenedUrl)
        .HasMaxLength(6);
    }
  }

  public static class ShortenUrlExtensions
  {
    public static ShortenUrlEntity Reset(this ShortenUrlEntity item)
    {
      item.OwnerUser = null;
      item.OriginalUrl = null;
      item.CreationTime = null;
      item.ExpirationTime = null;
      return item;
    }
  }
}
