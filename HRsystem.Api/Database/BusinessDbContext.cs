using System;
using System.Collections.Generic;
using HRsystem.Api.Database.DataTables;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace HRsystem.Api.Database;

public partial class BusinessDbContext : DbContext
{
    public BusinessDbContext()
    {
    }

    public BusinessDbContext(DbContextOptions<BusinessDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aspnetrole> Aspnetroles { get; set; }

    public virtual DbSet<Aspnetroleclaim> Aspnetroleclaims { get; set; }

    public virtual DbSet<Aspnetuser> Aspnetusers { get; set; }

    public virtual DbSet<Aspnetuserclaim> Aspnetuserclaims { get; set; }

    public virtual DbSet<Aspnetuserlogin> Aspnetuserlogins { get; set; }

    public virtual DbSet<Aspnetusertoken> Aspnetusertokens { get; set; }

    public virtual DbSet<Asppermission> Asppermissions { get; set; }

    public virtual DbSet<Asprolepermission> Asprolepermissions { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<TbActivityStatus> TbActivityStatuses { get; set; }

    public virtual DbSet<TbActivityType> TbActivityTypes { get; set; }

    public virtual DbSet<TbAttendanceLog> TbAttendanceLogs { get; set; }

    public virtual DbSet<TbAuditLog> TbAuditLogs { get; set; }

    public virtual DbSet<TbCity> TbCities { get; set; }

    public virtual DbSet<TbCompany> TbCompanies { get; set; }

    public virtual DbSet<TbDepartment> TbDepartments { get; set; }

    public virtual DbSet<TbEmployee> TbEmployees { get; set; }

    public virtual DbSet<TbEmployeeActivity> TbEmployeeActivities { get; set; }

    public virtual DbSet<TbEmployeeProject> TbEmployeeProjects { get; set; }

    public virtual DbSet<TbEmployeeWorkLocation> TbEmployeeWorkLocations { get; set; }

    public virtual DbSet<TbGov> TbGovs { get; set; }

    public virtual DbSet<TbGroup> TbGroups { get; set; }

    public virtual DbSet<TbJobLevel> TbJobLevels { get; set; }

    public virtual DbSet<TbJobTitle> TbJobTitles { get; set; }

    public virtual DbSet<TbLeaveBalance> TbLeaveBalances { get; set; }

    public virtual DbSet<TbLeaveRequest> TbLeaveRequests { get; set; }

    public virtual DbSet<TbLeaveType> TbLeaveTypes { get; set; }

    public virtual DbSet<TbProject> TbProjects { get; set; }

    public virtual DbSet<TbShift> TbShifts { get; set; }

    public virtual DbSet<TbShiftAssignment> TbShiftAssignments { get; set; }

    public virtual DbSet<TbShiftRule> TbShiftRules { get; set; }

    public virtual DbSet<TbWorkLocation> TbWorkLocations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=192.168.1.90;port=3306;database=db_hrsystem;user=hrsystem;password=hrsystem", ServerVersion.Parse("8.0.42-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Aspnetrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetroles");

            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<Aspnetroleclaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetroleclaims");

            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.Aspnetroleclaims)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_AspNetRoleClaims_AspNetRoles_RoleId");
        });

        modelBuilder.Entity<Aspnetuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetusers");

            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.CreatedAt).HasMaxLength(6);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.LastFailedLoginAt).HasMaxLength(6);
            entity.Property(e => e.LastLoginAt).HasMaxLength(6);
            entity.Property(e => e.LastPasswordChangedAt).HasMaxLength(6);
            entity.Property(e => e.LockoutEnd).HasMaxLength(6);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.PreferredLanguage).HasMaxLength(10);
            entity.Property(e => e.RowGuid)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.UserFullName).HasMaxLength(80);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Aspnetuserrole",
                    r => r.HasOne<Aspnetrole>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_AspNetUserRoles_AspNetRoles_RoleId"),
                    l => l.HasOne<Aspnetuser>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_AspNetUserRoles_AspNetUsers_UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("aspnetuserroles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Aspnetuserclaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetuserclaims");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserclaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Aspnetuserlogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("aspnetuserlogins");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserlogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Aspnetusertoken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("aspnetusertokens");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetusertokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Asppermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PRIMARY");

            entity.ToTable("asppermissions");

            entity.HasIndex(e => e.PermissionName, "IX_AspPermissions_PermissionName").IsUnique();

            entity.Property(e => e.CreatedAt).HasMaxLength(6);
            entity.Property(e => e.CreatedBy)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.PermissionCatagory).HasMaxLength(50);
            entity.Property(e => e.PermissionDescription).HasMaxLength(100);
            entity.Property(e => e.PermissionName).HasMaxLength(80);
        });

        modelBuilder.Entity<Asprolepermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("asprolepermissions");

            entity.HasIndex(e => e.PermissionId, "IX_AspRolePermissions_PermissionId");

            entity.Property(e => e.CreatedAt).HasMaxLength(6);
            entity.Property(e => e.CreatedBy)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.Permission).WithMany(p => p.Asprolepermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_AspRolePermissions_AspPermissions_PermissionId");

            entity.HasOne(d => d.Role).WithMany(p => p.Asprolepermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_AspRolePermissions_AspNetRoles_RoleId");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<TbActivityStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PRIMARY");

            entity.ToTable("tb_activity_status");

            entity.HasIndex(e => e.CompanyId, "FK_tb_activity_status_company_id");

            entity.HasIndex(e => e.StatusCode, "status_code").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsFinal).HasColumnName("is_final");
            entity.Property(e => e.StatusCode)
                .HasMaxLength(50)
                .HasColumnName("status_code");
            entity.Property(e => e.StatusName)
                .HasMaxLength(100)
                .HasColumnName("status_name");

            entity.HasOne(d => d.Company).WithMany(p => p.TbActivityStatuses)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_tb_activity_status_company_id");
        });

        modelBuilder.Entity<TbActivityType>(entity =>
        {
            entity.HasKey(e => e.ActivityTypeId).HasName("PRIMARY");

            entity.ToTable("tb_activity_type");

            entity.HasIndex(e => e.CompanyId, "FK_tb_activity_type_company_id");

            entity.HasIndex(e => e.ActivityCode, "type_code").IsUnique();

            entity.Property(e => e.ActivityTypeId).HasColumnName("activity_type_id");
            entity.Property(e => e.ActivityCode)
                .HasMaxLength(50)
                .HasColumnName("activity_code");
            entity.Property(e => e.ActivityDescription)
                .HasMaxLength(145)
                .HasColumnName("activity_description");
            entity.Property(e => e.ActivityName)
                .HasMaxLength(100)
                .HasColumnName("activity_name");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");

            entity.HasOne(d => d.Company).WithMany(p => p.TbActivityTypes)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_tb_activity_type_company_id");
        });

        modelBuilder.Entity<TbAttendanceLog>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PRIMARY");

            entity.ToTable("tb_attendance_log");

            entity.HasIndex(e => e.EmployeeId, "employee_id");

            entity.HasIndex(e => e.CompanyId, "fk_attendance_company");

            entity.Property(e => e.AttendanceId).HasColumnName("attendance_id");
            entity.Property(e => e.AttendanceDate).HasColumnName("attendance_date");
            entity.Property(e => e.CheckIn)
                .HasColumnType("time")
                .HasColumnName("check_in");
            entity.Property(e => e.CheckOut)
                .HasColumnType("time")
                .HasColumnName("check_out");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

            entity.HasOne(d => d.Company).WithMany(p => p.TbAttendanceLogs)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_attendance_company");

            entity.HasOne(d => d.Employee).WithMany(p => p.TbAttendanceLogs)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_attendance_log_ibfk_1");
        });

        modelBuilder.Entity<TbAuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PRIMARY");

            entity.ToTable("tb_audit_log");

            entity.HasIndex(e => e.CompanyId, "company_id");

            entity.Property(e => e.AuditId).HasColumnName("audit_id");
            entity.Property(e => e.ActionDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("action_datetime");
            entity.Property(e => e.ActionType)
                .HasColumnType("enum('INSERT','UPDATE','DELETE')")
                .HasColumnName("action_type");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.NewData)
                .HasColumnType("json")
                .HasColumnName("new_data");
            entity.Property(e => e.OldData)
                .HasColumnType("json")
                .HasColumnName("old_data");
            entity.Property(e => e.RecordId)
                .HasMaxLength(100)
                .HasColumnName("record_id");
            entity.Property(e => e.TableName)
                .HasMaxLength(100)
                .HasColumnName("table_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Company).WithMany(p => p.TbAuditLogs)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_audit_log_ibfk_1");
        });

        modelBuilder.Entity<TbCity>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PRIMARY");

            entity.ToTable("tb_city");

            entity.HasIndex(e => e.GovId, "fk_gov_idx");

            entity.Property(e => e.CityId)
                .ValueGeneratedNever()
                .HasColumnName("city_id");
            entity.Property(e => e.CityName)
                .HasMaxLength(45)
                .HasColumnName("city_name");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.GovId).HasColumnName("gov_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Gov).WithMany(p => p.TbCities)
                .HasForeignKey(d => d.GovId)
                .HasConstraintName("fk_gov");
        });

        modelBuilder.Entity<TbCompany>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PRIMARY");

            entity.ToTable("tb_company");

            entity.HasIndex(e => e.GroupId, "group_id");

            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(150)
                .HasColumnName("company_name");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Group).WithMany(p => p.TbCompanies)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_company_ibfk_1");
        });

        modelBuilder.Entity<TbDepartment>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PRIMARY");

            entity.ToTable("tb_department");

            entity.HasIndex(e => e.CompanyId, "fk_compantid_idx");

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(25)
                .HasColumnName("department_code");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(150)
                .HasColumnName("department_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Company).WithMany(p => p.TbDepartments)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("fk_compantid");
        });

        modelBuilder.Entity<TbEmployee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PRIMARY");

            entity.ToTable("tb_employee");

            entity.HasIndex(e => e.CompanyId, "fk_employee_company");

            entity.HasIndex(e => e.JobTitleId, "job_title_id");

            entity.HasIndex(e => e.ManagerId, "manager_id");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.ArabicFirstName)
                .HasMaxLength(100)
                .HasColumnName("arabic_first_name");
            entity.Property(e => e.ArabicFullName)
                .HasMaxLength(250)
                .HasColumnName("arabic_full_name");
            entity.Property(e => e.ArabicLastName)
                .HasMaxLength(100)
                .HasColumnName("arabic_last_name");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.BloodGroup)
                .HasColumnType("enum('A+','A-','B+','B-','AB+','AB-','O+','O-')")
                .HasColumnName("blood_group");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.EmployeeCodeFinance)
                .HasMaxLength(25)
                .HasColumnName("employee_code_finance");
            entity.Property(e => e.EmployeeCodeHr)
                .HasMaxLength(25)
                .HasColumnName("employee_code_hr");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasColumnType("enum('Male','Female','Other')")
                .HasColumnName("gender");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.JobTitleId).HasColumnName("job_title_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.MaritalStatus)
                .HasColumnType("enum('Single','Married','Divorced','Widowed')")
                .HasColumnName("marital_status");
            entity.Property(e => e.NationalId)
                .HasMaxLength(50)
                .HasColumnName("national_id");
            entity.Property(e => e.Nationality)
                .HasMaxLength(100)
                .HasColumnName("nationality");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(50)
                .HasColumnName("passport_number");
            entity.Property(e => e.PlaceOfBirth)
                .HasMaxLength(150)
                .HasColumnName("place_of_birth");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Company).WithMany(p => p.TbEmployees)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee_company");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.TbEmployees)
                .HasForeignKey(d => d.JobTitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_employee_ibfk_1");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("tb_employee_ibfk_2");
        });

        modelBuilder.Entity<TbEmployeeActivity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PRIMARY");

            entity.ToTable("tb_employee_activity");

            entity.HasIndex(e => e.CompanyId, "fk_activity_company");

            entity.HasIndex(e => e.EmployeeId, "fk_activity_employee");

            entity.HasIndex(e => e.StatusId, "fk_activity_status");

            entity.HasIndex(e => e.ActivityTypeId, "fk_activity_type");

            entity.Property(e => e.ActivityId).HasColumnName("activity_id");
            entity.Property(e => e.ActivityDate).HasColumnName("activity_date");
            entity.Property(e => e.ActivityTypeId).HasColumnName("activity_type_id");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.ApprovedDate)
                .HasColumnType("datetime")
                .HasColumnName("approved_date");
            entity.Property(e => e.CheckIn)
                .HasColumnType("time")
                .HasColumnName("check_in");
            entity.Property(e => e.CheckOut)
                .HasColumnType("time")
                .HasColumnName("check_out");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDatetime)
                .HasColumnType("datetime")
                .HasColumnName("end_datetime");
            entity.Property(e => e.RequestBy).HasColumnName("request_by");
            entity.Property(e => e.RequestDate)
                .HasColumnType("datetime")
                .HasColumnName("request_date");
            entity.Property(e => e.StartDatetime)
                .HasColumnType("datetime")
                .HasColumnName("start_datetime");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.ActivityType).WithMany(p => p.TbEmployeeActivities)
                .HasForeignKey(d => d.ActivityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_activity_type");

            entity.HasOne(d => d.Company).WithMany(p => p.TbEmployeeActivities)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_activity_company");

            entity.HasOne(d => d.Employee).WithMany(p => p.TbEmployeeActivities)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_activity_employee");

            entity.HasOne(d => d.Status).WithMany(p => p.TbEmployeeActivities)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_activity_status");
        });

        modelBuilder.Entity<TbEmployeeProject>(entity =>
        {
            entity.HasKey(e => e.EmployeeProjectId).HasName("PRIMARY");

            entity.ToTable("tb_employee_project");

            entity.HasIndex(e => e.EmployeeId, "FK_tb_employee_project_tb_employee_employee_id");

            entity.HasIndex(e => e.ProjectId, "FK_tb_employee_project_tb_projects_project_id");

            entity.HasIndex(e => e.CompanyId, "fk_employee_project_company");

            entity.Property(e => e.EmployeeProjectId).HasColumnName("employee_project_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Company).WithMany(p => p.TbEmployeeProjects)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee_project_company");

            entity.HasOne(d => d.Employee).WithMany(p => p.TbEmployeeProjects)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.TbEmployeeProjects)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<TbEmployeeWorkLocation>(entity =>
        {
            entity.HasKey(e => e.EmployeeWorkLocationId).HasName("PRIMARY");

            entity.ToTable("tb_employee_work_location");

            entity.HasIndex(e => e.EmployeeId, "FK_tb_employee_work_location_tb_employee_employee_id");

            entity.HasIndex(e => e.WorkLocationId, "FK_tb_employee_work_location_tb_work_location_work_location_id");

            entity.HasIndex(e => e.CompanyId, "fk_employee_location_company");

            entity.Property(e => e.EmployeeWorkLocationId).HasColumnName("employee_work_location_id");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.WorkLocationId).HasColumnName("work_location_id");

            entity.HasOne(d => d.Company).WithMany(p => p.TbEmployeeWorkLocations)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee_location_company");

            entity.HasOne(d => d.Employee).WithMany(p => p.TbEmployeeWorkLocations)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.WorkLocation).WithMany(p => p.TbEmployeeWorkLocations)
                .HasForeignKey(d => d.WorkLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<TbGov>(entity =>
        {
            entity.HasKey(e => e.GovId).HasName("PRIMARY");

            entity.ToTable("tb_gov");

            entity.Property(e => e.GovId)
                .ValueGeneratedNever()
                .HasColumnName("gov_id");
            entity.Property(e => e.GovArea)
                .HasMaxLength(45)
                .HasColumnName("gov_area");
            entity.Property(e => e.GovName)
                .HasMaxLength(45)
                .HasColumnName("gov_name");
            entity.Property(e => e.GoveCode)
                .HasMaxLength(25)
                .HasColumnName("gove_code");
        });

        modelBuilder.Entity<TbGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PRIMARY");

            entity.ToTable("tb_group");

            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.GroupName)
                .HasMaxLength(150)
                .HasColumnName("group_name");
        });

        modelBuilder.Entity<TbJobLevel>(entity =>
        {
            entity.HasKey(e => e.JobLevelId).HasName("PRIMARY");

            entity.ToTable("tb_job_level");

            entity.Property(e => e.JobLevelId)
                .ValueGeneratedNever()
                .HasColumnName("job_level_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.JobLevelCode)
                .HasMaxLength(5)
                .HasColumnName("job_level_code");
            entity.Property(e => e.JobLevelDesc)
                .HasMaxLength(45)
                .HasColumnName("job_level_desc");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<TbJobTitle>(entity =>
        {
            entity.HasKey(e => e.JobTitleId).HasName("PRIMARY");

            entity.ToTable("tb_job_title");

            entity.HasIndex(e => e.DepartmentId, "department_id");

            entity.HasIndex(e => e.JobLevelId, "fk_job_level_idx");

            entity.HasIndex(e => e.CompanyId, "fk_jobtitle_company");

            entity.Property(e => e.JobTitleId).HasColumnName("job_title_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.JobLevelId).HasColumnName("job_level_id");
            entity.Property(e => e.TitleName)
                .HasMaxLength(150)
                .HasColumnName("title_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Company).WithMany(p => p.TbJobTitles)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_jobtitle_company");

            entity.HasOne(d => d.Department).WithMany(p => p.TbJobTitles)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_job_title_ibfk_1");

            entity.HasOne(d => d.JobLevel).WithMany(p => p.TbJobTitles)
                .HasForeignKey(d => d.JobLevelId)
                .HasConstraintName("fk_job_level");
        });

        modelBuilder.Entity<TbLeaveBalance>(entity =>
        {
            entity.HasKey(e => e.BalanceId).HasName("PRIMARY");

            entity.ToTable("tb_leave_balance");

            entity.HasIndex(e => e.EmployeeId, "employee_id");

            entity.HasIndex(e => e.CompanyId, "fk_leavebalance_company");

            entity.HasIndex(e => e.LeaveTypeId, "leave_type_id");

            entity.Property(e => e.BalanceId).HasColumnName("balance_id");
            entity.Property(e => e.BalanceDays)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("balance_days");
            entity.Property(e => e.BalanceRemaining)
                .HasPrecision(5, 2)
                .HasColumnName("balance_remaining");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.LeaveTypeId).HasColumnName("leave_type_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Company).WithMany(p => p.TbLeaveBalances)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_leavebalance_company");

            entity.HasOne(d => d.Employee).WithMany(p => p.TbLeaveBalances)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_leave_balance_ibfk_1");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.TbLeaveBalances)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_leave_balance_ibfk_2");
        });

        modelBuilder.Entity<TbLeaveRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PRIMARY");

            entity.ToTable("tb_leave_request");

            entity.HasIndex(e => e.ApprovedBy, "approved_by");

            entity.HasIndex(e => e.EmployeeId, "employee_id");

            entity.HasIndex(e => e.CompanyId, "fk_leaverequest_company");

            entity.HasIndex(e => e.LeaveTypeId, "leave_type_id");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.ApprovedDate)
                .HasColumnType("datetime")
                .HasColumnName("approved_date");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.LeaveTypeId).HasColumnName("leave_type_id");
            entity.Property(e => e.RequestBy).HasColumnName("request_by");
            entity.Property(e => e.RequestDate)
                .HasColumnType("datetime")
                .HasColumnName("request_date");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Pending','Approved','Rejected')")
                .HasColumnName("status");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.TbLeaveRequestApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("tb_leave_request_ibfk_3");

            entity.HasOne(d => d.Company).WithMany(p => p.TbLeaveRequests)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_leaverequest_company");

            entity.HasOne(d => d.Employee).WithMany(p => p.TbLeaveRequestEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_leave_request_ibfk_1");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.TbLeaveRequests)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_leave_request_ibfk_2");
        });

        modelBuilder.Entity<TbLeaveType>(entity =>
        {
            entity.HasKey(e => e.LeaveTypeId).HasName("PRIMARY");

            entity.ToTable("tb_leave_type");

            entity.HasIndex(e => e.CompanyId, "fk_leavetype_company");

            entity.Property(e => e.LeaveTypeId).HasColumnName("leave_type_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .HasColumnName("type_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Company).WithMany(p => p.TbLeaveTypes)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_leavetype_company");
        });

        modelBuilder.Entity<TbProject>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PRIMARY");

            entity.ToTable("tb_projects");

            entity.HasIndex(e => e.CityId, "FK_tb_projects_tb_city_city_id");

            entity.HasIndex(e => e.CompanyId, "fk_project_company");

            entity.Property(e => e.ProjectId)
                .ValueGeneratedNever()
                .HasColumnName("project_id");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(25)
                .HasColumnName("project_code");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(45)
                .HasColumnName("project_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.WorkLocationId)
                .HasMaxLength(255)
                .HasColumnName("work_location_id");

            entity.HasOne(d => d.City).WithMany(p => p.TbProjects).HasForeignKey(d => d.CityId);

            entity.HasOne(d => d.Company).WithMany(p => p.TbProjects)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_project_company");
        });

        modelBuilder.Entity<TbShift>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("PRIMARY");

            entity.ToTable("tb_shift");

            entity.HasIndex(e => e.CompanyId, "fk_shift_company");

            entity.Property(e => e.ShiftId).HasColumnName("shift_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.GracePeriodMinutes).HasColumnName("grace_period_minutes");
            entity.Property(e => e.IsFlexible).HasColumnName("is_flexible");
            entity.Property(e => e.MaxStartTime)
                .HasColumnType("time")
                .HasColumnName("max_start_time");
            entity.Property(e => e.MinStartTime)
                .HasColumnType("time")
                .HasColumnName("min_start_time");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.RequiredWorkingHours)
                .HasPrecision(4, 2)
                .HasColumnName("required_working_hours");
            entity.Property(e => e.ShiftName)
                .HasMaxLength(100)
                .HasColumnName("shift_name");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Company).WithMany(p => p.TbShifts)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shift_company");
        });

        modelBuilder.Entity<TbShiftAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PRIMARY");

            entity.ToTable("tb_shift_assignment");

            entity.HasIndex(e => e.EmployeeId, "employee_id");

            entity.HasIndex(e => e.CompanyId, "fk_shiftassignment_company");

            entity.HasIndex(e => e.ShiftId, "shift_id");

            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.EffectiveDate).HasColumnName("effective_date");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.ShiftId).HasColumnName("shift_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Company).WithMany(p => p.TbShiftAssignments)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shiftassignment_company");

            entity.HasOne(d => d.Employee).WithMany(p => p.TbShiftAssignments)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_shift_assignment_ibfk_1");

            entity.HasOne(d => d.Shift).WithMany(p => p.TbShiftAssignments)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_shift_assignment_ibfk_2");
        });

        modelBuilder.Entity<TbShiftRule>(entity =>
        {
            entity.HasKey(e => e.RuleId).HasName("PRIMARY");

            entity.ToTable("tb_shift_rule");

            entity.HasIndex(e => e.CompanyId, "fk_shiftrule_company");

            entity.HasIndex(e => new { e.JobTitleId, e.WorkingLocationId, e.ProjectId, e.ShiftId }, "job_title_id").IsUnique();

            entity.HasIndex(e => e.ProjectId, "project_id");

            entity.HasIndex(e => e.ShiftId, "shift_id");

            entity.HasIndex(e => e.WorkingLocationId, "tb_shift_rule_ibfk_2");

            entity.Property(e => e.RuleId).HasColumnName("rule_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.JobTitleId).HasColumnName("job_title_id");
            entity.Property(e => e.Priority)
                .HasDefaultValueSql("'1'")
                .HasColumnName("priority");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.ShiftId).HasColumnName("shift_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.WorkingLocationId).HasColumnName("working_location_id");

            entity.HasOne(d => d.Company).WithMany(p => p.TbShiftRules)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shiftrule_company");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.TbShiftRules)
                .HasForeignKey(d => d.JobTitleId)
                .HasConstraintName("tb_shift_rule_ibfk_1");

            entity.HasOne(d => d.Project).WithMany(p => p.TbShiftRules)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("tb_shift_rule_ibfk_3");

            entity.HasOne(d => d.Shift).WithMany(p => p.TbShiftRules)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_shift_rule_ibfk_4");

            entity.HasOne(d => d.WorkingLocation).WithMany(p => p.TbShiftRules)
                .HasForeignKey(d => d.WorkingLocationId)
                .HasConstraintName("tb_shift_rule_ibfk_2");
        });

        modelBuilder.Entity<TbWorkLocation>(entity =>
        {
            entity.HasKey(e => e.WorkLocationId).HasName("PRIMARY");

            entity.ToTable("tb_work_location");

            entity.HasIndex(e => e.CompanyId, "company_id");

            entity.HasIndex(e => e.CityId, "fk_city_idx");

            entity.Property(e => e.WorkLocationId).HasColumnName("work_location_id");
            entity.Property(e => e.AllowedRadiusM).HasColumnName("allowed_radius_m");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Latitude)
                .HasPrecision(10, 7)
                .HasColumnName("latitude");
            entity.Property(e => e.LocationName)
                .HasMaxLength(150)
                .HasColumnName("location_name");
            entity.Property(e => e.Longitude)
                .HasPrecision(10, 7)
                .HasColumnName("longitude");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.WorkLocationCode)
                .HasMaxLength(25)
                .HasColumnName("work_location_code");

            entity.HasOne(d => d.City).WithMany(p => p.TbWorkLocations)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("fk_city");

            entity.HasOne(d => d.Company).WithMany(p => p.TbWorkLocations)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_work_location_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
