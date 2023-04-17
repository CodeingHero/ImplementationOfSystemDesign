using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemDesign.EF.Models {
  public class JwtOption {
    public string SigningKey { get; set; }
    public int AccessExpireHours { get; set; }
    public int RefreshExpireHours { get; set; }
  }
}
