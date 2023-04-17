using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SystemDesign.EF.Models;

namespace SystemDesign.EF {

  public class MyDBContext : IdentityDbContext<SDUser, UserRole, long> {
    public DbSet<ShortenUrlEntity> ShortenUrls { get; set; }
    //public  DbSet<SDUser> SDUsers { get; set; }
    public MyDBContext(DbContextOptions options) : base(options) {

    }

    protected override void OnModelCreating(ModelBuilder builder) {
      base.OnModelCreating(builder);
      builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
      builder.ApplyConfiguration(new SDUserConfiguration());
      //builder.HasSequence<long>("SDUsers").StartsAt(10000).IncrementsBy(1);
      //builder.Entity<SDUser>()
      //  .Property(e => e.Id)
        ;//.HasDefaultValueSql("NEXT VALUE FOR SDUsers");
      //builder.HasSequence<long>("UrlKeys").StartsAt(100000).IncrementsBy(1);
      //builder.Entity<ShortenUrlEntity>()
      //  .Property(e => e.Id)
        ;//.HasDefaultValueSql("NEXT VALUE FOR UrlKeys");
    }
    //protected override void OnModelCreating(ModelBuilder builder) {
    //  base.OnModelCreating(builder);
    //  builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    //  builder.ApplyConfiguration(new SDUserConfiguration());
    //  builder.HasSequence<long>("SDUsers").StartsAt(10000).IncrementsBy(1);
    //  builder.Entity<SDUser>()
    //    .Property(e => e.Id)
    //    .HasDefaultValueSql("NEXT VALUE FOR SDUsers");
    //  builder.HasSequence<long>("UrlKeys").StartsAt(100000).IncrementsBy(1);
    //  builder.Entity<ShortenUrlEntity>()
    //    .Property(e => e.UrlKey)
    //    .HasDefaultValueSql("NEXT VALUE FOR UrlKeys");
    //}
  }
}
