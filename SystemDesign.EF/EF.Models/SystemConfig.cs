using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SystemDesign.EF.Models {
  public class WebSetting
  {
    public string DomainName { get; set; }
  }
}
