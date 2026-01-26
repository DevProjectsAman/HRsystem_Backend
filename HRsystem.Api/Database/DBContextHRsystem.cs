using HRsystem.Api.Database.DataSeeder;
using HRsystem.Api.Database.DataTables;
//using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Database.Entities;
 
using HRsystem.Api.Shared.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace HRsystem.Api.Database;

public class DBContextHRsystem : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public DBContextHRsystem(DbContextOptions<DBContextHRsystem> options)
        : base(options)
    {
    }

    public virtual DbSet<AspPermission> AspPermissions { get; set; }
    public virtual DbSet<AspRolePermissions> AspRolePermissions { get; set; }

    public virtual DbSet<TbActivityStatus> TbActivityStatuses { get; set; }

    public virtual DbSet<TbActivityType> TbActivityTypes { get; set; }

    public virtual DbSet<TbAuditLog> TbAuditLogs { get; set; }

    public virtual DbSet<TbCity> TbCities { get; set; }

    public virtual DbSet<TbAttendanceStatues> TbAttendanceStatues { get; set; }
    
    public virtual DbSet<TbCompany> TbCompanies { get; set; }

    public virtual DbSet<TbDepartment> TbDepartments { get; set; }

    public virtual DbSet<TbEmployee> TbEmployees { get; set; }

    public virtual DbSet<TbEmployeeMonthlyReport> TbEmployeeMonthlyReports { get; set; }

    public virtual DbSet<TbEmployeeActivity> TbEmployeeActivities { get; set; }

    public virtual DbSet<TbEmployeeActivityApproval> TbEmployeeActivityApprovals { get; set; }

    public virtual DbSet<TbEmployeeAttendance> TbEmployeeAttendances { get; set; }

    public virtual DbSet<TbEmployeeAttendancePunch> TbEmployeeAttendancePunches { get; set; }

    public virtual DbSet<TbEmployeeExcuse> TbEmployeeExcuses { get; set; }

    public virtual DbSet<TbEmployeeMission> TbEmployeeMissions { get; set; }

    public virtual DbSet<TbEmployeeProject> TbEmployeeProjects { get; set; }

    public virtual DbSet<TbEmployeeShift> TbEmployeeShifts { get; set; }

    public virtual DbSet<TbEmployeeVacation> TbEmployeeVacations { get; set; }

    public virtual DbSet<TbEmployeeVacationBalance> TbEmployeeVacationBalances { get; set; }

    public virtual DbSet<TbEmployeeWorkLocation> TbEmployeeWorkLocations { get; set; }

    public virtual DbSet<TbGov> TbGovs { get; set; }

    public virtual DbSet<TbGroup> TbGroups { get; set; }

    public virtual DbSet<TbJobLevel> TbJobLevels { get; set; }

    public virtual DbSet<TbRemoteWorkDay> TbRemoteWorkDays { get; set; }

    //public virtual DbSet<TbWorkDaysRule> TbWorkDaysRules { get; set; }

    public virtual DbSet<TbJobTitle> TbJobTitles { get; set; }

    public virtual DbSet<TbProject> TbProjects { get; set; }

    public virtual DbSet<TbShift> TbShifts { get; set; }

    public virtual DbSet<TbShiftRule> TbShiftRules { get; set; }

    public virtual DbSet<TbVacationRule> TbVacationRules { get; set; }

    public virtual DbSet<TbVacationType> TbVacationTypes { get; set; }

    public virtual DbSet<TbWorkLocation> TbWorkLocations { get; set; }
    public virtual DbSet<TbNationality> TbNationalities { get; set; }

    public DbSet<TbActivityTypeStatus> TbActivityTypeStatuses { get; set; }
    public DbSet<TbWorkDays> TbWorkDays { get; set; }
    public DbSet<TbWorkDaysRule> TbWorkDaysRules { get; set; }
    public DbSet<TbHolidays> TbHolidays { get; set; }
    public DbSet<TbHolidayType> TbHolidayTypes { get; set; }
    public DbSet<TbActivityStatusWorkflow> TbActivityStatusWorkflow { get; set; }
    public DbSet<TbVacationRulesGroup> TbVacationRulesGroups { get; set; }
    public DbSet<TbVacationRulesGroupDetail> TbVacationRulesGroupDetails { get; set; }

    public DbSet<TbMaritalStatus> TbMaritalStatuses { get; set; }
    public DbSet<TbContractType> TbContractTypes { get; set; }
    public DbSet<TbShiftRuleMappng> TbShiftRuleMappngs { get; set; }
    public DbSet<TbEmployeeDevicesTrack> TbEmployeeDevicesTrack { get; set; }

    public DbSet<TbEmployeeCodeTracking> TbEmployeeCodeTrackings { get; set; }
    public DbSet<TbUserSession> TbUserSession { get; set; }
    public DbSet<TbUserLoginHistory> TbUserLoginHistories { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply Identity configurations
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

        // 🔹 Global case-insensitive JSON options
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        // 🔹 Helper for localized data conversion
        ValueConverter<LocalizedData, string> LocalizedConverter =
            new ValueConverter<LocalizedData, string>(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<LocalizedData>(v, jsonOptions)!
            );

        // 🔹 Apply unified conversion for all entities with LocalizedData fields
        modelBuilder.Entity<TbActivityStatus>(entity =>
        {
            entity.Property(e => e.StatusName)
                  .HasConversion(LocalizedConverter)
                  .HasColumnType("json");
        });

        modelBuilder.Entity<TbActivityType>(entity =>
        {
            entity.Property(e => e.ActivityName)
                  .HasConversion(LocalizedConverter)
                  .HasColumnType("json");
        });

        modelBuilder.Entity<TbHolidays>(entity =>
        {
            entity.Property(e => e.HolidayName)
                  .HasConversion(LocalizedConverter)
                  .HasColumnType("json");
        });

        modelBuilder.Entity<TbHolidayType>(entity =>
        {
            entity.Property(e => e.HolidayTypeName)
                  .HasConversion(LocalizedConverter)
                  .HasColumnType("json");
        });

        modelBuilder.Entity<TbAttendanceStatues>(entity =>
        {
            entity.Property(e => e.AttendanceStatuesName)
                  .HasConversion(LocalizedConverter)
                  .HasColumnType("json");
        });

        modelBuilder.Entity<TbProject>(entity =>
        {
            entity.Property(e => e.ProjectName)
                  .HasConversion(LocalizedConverter)
                  .HasColumnType("json");
        });

        modelBuilder.Entity<TbDepartment>(entity =>
        {
            entity.Property(e => e.DepartmentName)
                  .HasConversion(LocalizedConverter)
                  .HasColumnType("json");
        });

        modelBuilder.Entity<TbVacationType>(entity =>
        {
            entity.Property(e => e.VacationName)
                  .HasConversion(LocalizedConverter)
                  .HasColumnType("json");
        });

        modelBuilder.Entity<TbWorkLocation>(entity =>
        {
            entity.Property(e => e.LocationName)
                  .HasConversion(LocalizedConverter)
                  .HasColumnType("json");
        });

        modelBuilder.Entity<TbShift>(entity =>
        {
            entity.Property(e => e.ShiftName)
                  .HasConversion(LocalizedConverter)
                  .HasColumnType("json");
        });

        modelBuilder.Entity<TbJobTitle>(entity =>
        {
            entity.Property(e => e.TitleName)
                  .HasConversion(LocalizedConverter)
                  .HasColumnType("json");
        });

        // 🔹 JSON-only fields (no conversion needed)
        modelBuilder.Entity<TbAuditLog>(entity =>
        {
            entity.Property(e => e.OldData).HasColumnType("json");
            entity.Property(e => e.NewData).HasColumnType("json");
        });

        // 🔹 Enums stored as strings (MySQL-friendly)
        modelBuilder.Entity<TbEmployee>(entity =>
        {
            entity.Property(e => e.Gender)
                  .HasConversion<string>()
                  .HasColumnType("ENUM('Male','Female')");
        });

        modelBuilder.Entity<TbVacationRule>(entity =>
        {
            entity.Property(e => e.Gender)
                  .HasConversion<string>()
                  .HasColumnType("ENUM('Male','Female','All')");

            entity.Property(e => e.Religion)
                  .HasConversion<string>()
                  .HasColumnType("ENUM('All','Muslim','Christian')");
        });

        // 🔹 Composite key for Role-Permission
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

        // 🔹 Unique index on permission names
        modelBuilder.Entity<AspPermission>()
            .HasIndex(p => p.PermissionName)
            .IsUnique();

 

        modelBuilder.Entity<TbUserSession>()
            .HasIndex(x => x.Jti)
            .IsUnique();
    }



}