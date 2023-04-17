using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SystemDesign.EF.Models {
  public class SDUser : IdentityUser<long> {
    public DateTime CreationTime { get; set; }
    public string? NickName { get; set; }
  }

  public class SDUserConfiguration : IEntityTypeConfiguration<SDUser> {
    public void Configure(EntityTypeBuilder<SDUser> builder) {
    }
  }
}
