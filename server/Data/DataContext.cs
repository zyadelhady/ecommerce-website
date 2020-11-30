using e_commerce.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace e_commerce.Data
{
  ////////////////////////////////////////////////////////FUCK
  public class DataContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
  {
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      var ListConverter = new ValueConverter<List<string>, string>(
        v => JsonSerializer.Serialize(v, default),
        v => JsonSerializer.Deserialize<List<string>>(v, default)
      );

      builder.Entity<Product>()
              .Property(p => p.Images)
              .HasConversion(ListConverter);
      builder.Entity<Product>()
        .Property(p => p.Colors)
        .HasConversion(ListConverter);
      builder.Entity<Product>()
        .Property(p => p.Sizes)
        .HasConversion(ListConverter);

      builder.Entity<Product>()
        .HasIndex(p => p.Slug)
        .IsUnique(true);

      builder.Entity<Collection>()
        .HasIndex(c => c.Name)
        .IsUnique(true);


      builder.Entity<User>()
      .HasOne<Role>(u => u.Role)
      .WithMany(u => u.Users)
      .HasForeignKey(u => u.RoleId);

    }

  }
}
