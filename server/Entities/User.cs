using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace e_commerce.Entities
{
  public class User : IdentityUser<Guid>
  {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override Guid Id { get; set; } = Guid.NewGuid();

    public Guid? RoleId { get; set; }

    public Role Role { get; set; }

  }
}
