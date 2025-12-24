using HRsystem.Api.Database.ConestKey;
using HRsystem.Api.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRsystem.Api.Database.DataSeeder
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            // Seed the SystemAdmin role
            builder.HasData(
                new ApplicationRole
                {
                  //  Id = Guid.Parse("08dd03ec-ef32-4b77-8c77-19f8bb6bda50"),
                    Id = 1,
                    Name = Keys.RoleKey.SystemAdmin,
                    ConcurrencyStamp = "0b2f3b2f-a5d1-4d3f-be8b-db04070caed1",
                    NormalizedName = Keys.RoleKey.SystemAdmin.ToUpper(),
                     Category = "System",
                     DisplayName = "System Administrator",
                        Description = "Role with full system access"
                }
            );
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Seed the SystemAdmin user
            builder.HasData(
                new ApplicationUser
                {
                    // Id = Guid.Parse("08dd03ec-aff2-4d24-89cf-738fd51097a9"),
                    Id = 1,
                    CreatedAt = DateTime.UtcNow,
                    Email = "systemadmin@example.com",
                    NormalizedEmail = "SYSTEMADMIN@EXAMPLE.COM",
                    PhoneNumber = "01200000000",
                    PasswordHash = "AQAAAAIAAYagAAAAEHuYA7U5KAgI1iuzqry/7jPmIBrciy7nyILnyLHLuOwz3plNoiOeAavDPyJliZul9A==",
                    SecurityStamp = "6QVLU2WHQVYOV4FRB6EFKIGE2KJJICGL",
                    ConcurrencyStamp = "2cc3da7b-b1d4-43fc-b129-4e706e02ac96",
                    PhoneNumberConfirmed = true,
                    UserName = "Boles",
                    NormalizedUserName = "BOLES",
                    UserFullName = "Boles Lewis Boles",


                    // Custom fields
                    RowGuid = Guid.NewGuid(),

                    CompanyId = 1, // assign to default company
                    
                    EmployeeId = 1,                // 👈 must be set (since not nullable)
                    PermissionVersion = 1,         // 👈 initialize with default


                    IsActive = true,
                    IsToChangePassword = false,
                    ForceLogout = false,

                    LastPasswordChangedAt = DateTime.UtcNow,
                    LastLoginAt = null, // first login not happened yet
                    LastFailedLoginAt = null,
                    FailedLoginCount = 0,

                    PreferredLanguage = "en",

                    CreatedBy = null, // system seeded

                }
            );
        }
    
    
    }




    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
        {
            // Seed the relationship between the SystemAdmin user and SystemAdmin role
            builder.HasData(
                new IdentityUserRole<int>
                {
                  //  RoleId = Guid.Parse("08dd03ec-ef32-4b77-8c77-19f8bb6bda50"), // SystemAdmin role ID
                   // UserId = Guid.Parse("08dd03ec-aff2-4d24-89cf-738fd51097a9")  // SystemAdmin user ID

                     RoleId = 1, // SystemAdmin role ID
                    UserId = 1  // SystemAdmin user ID



                }
            );
        }
    }
}
