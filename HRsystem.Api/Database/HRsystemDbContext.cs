using HRsystem.Api.Database.DataSeeder;
using HRsystem.Api.Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Database;

public class HRsystemDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{


    public HRsystemDbContext(DbContextOptions<HRsystemDbContext> options)
        : base(options)
    {
    }



    public virtual DbSet<AspPermission> AspPermissions { get; set; }
    public virtual DbSet<AspRolePermissions> AspRolePermissions { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //  modelBuilder.ApplyConfiguration(new RoleConfiguration());

        base.OnModelCreating(modelBuilder);


        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());



        modelBuilder.Entity<AspRolePermissions>()
           .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<AspRolePermissions>()
            .HasOne(rp => rp.Role)
            .WithMany()
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<AspRolePermissions>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId);


        modelBuilder.Entity<AspPermission>()
       .HasIndex(p => p.PermissionName)
       .IsUnique();

    }

}
