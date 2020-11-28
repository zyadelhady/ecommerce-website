using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace e_commerce.Entities
{
  public class Role : IdentityRole<Guid>
  {
    public ICollection<User> Users { get; set; }

    // public ICollection<UserRole> UserRoles { get; set; }
  }
}