using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class ALL_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RowGuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserFullName = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsToChangePassword = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastPasswordChangedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastFailedLoginAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ForceLogout = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: false),
                    PreferredLanguage = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspPermissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PermissionCatagory = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermissionName = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermissionDescription = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspPermissions", x => x.PermissionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Gov",
                columns: table => new
                {
                    GovId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GoveCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GovName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GovArea = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Gov", x => x.GovId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Group",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Group", x => x.GroupId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Job_Level",
                columns: table => new
                {
                    JobLevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JobLevelDesc = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JobLevelCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Job_Level", x => x.JobLevelId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Vacation_Type",
                columns: table => new
                {
                    VacationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VacationName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPaid = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    RequiresHrApproval = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Vacation_Type", x => x.VacationTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspRolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspRolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_AspRolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspRolePermissions_AspPermissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "AspPermissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_City",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GovId = table.Column<int>(type: "int", nullable: true),
                    CityName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_City", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Tb_City_Tb_Gov_GovId",
                        column: x => x.GovId,
                        principalTable: "Tb_Gov",
                        principalColumn: "GovId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Company", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_Tb_Company_Tb_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Tb_Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Vacation_Rule",
                columns: table => new
                {
                    RuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VacationTypeId = table.Column<int>(type: "int", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    MaxAge = table.Column<int>(type: "int", nullable: true),
                    MinServiceYears = table.Column<int>(type: "int", nullable: true),
                    MaxServiceYears = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Religion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    YearlyBalance = table.Column<int>(type: "int", nullable: false),
                    Prorate = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Vacation_Rule", x => x.RuleId);
                    table.ForeignKey(
                        name: "FK_Tb_Vacation_Rule_Tb_Vacation_Type_VacationTypeId",
                        column: x => x.VacationTypeId,
                        principalTable: "Tb_Vacation_Type",
                        principalColumn: "VacationTypeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Activity_Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StatusCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatusName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsFinal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Activity_Status", x => x.StatusId);
                    table.ForeignKey(
                        name: "FK_Tb_Activity_Status_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Activity_Type",
                columns: table => new
                {
                    ActivityTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActivityName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActivityDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Activity_Type", x => x.ActivityTypeId);
                    table.ForeignKey(
                        name: "FK_Tb_Activity_Type_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Audit_Log",
                columns: table => new
                {
                    AuditId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ActionDatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TableName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActionType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecordId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OldData = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewData = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Audit_Log", x => x.AuditId);
                    table.ForeignKey(
                        name: "FK_Tb_Audit_Log_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DepartmentCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DepartmentName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Department", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_Tb_Department_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Project",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    WorkLocationId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Project", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Tb_Project_Tb_City_CityId",
                        column: x => x.CityId,
                        principalTable: "Tb_City",
                        principalColumn: "CityId");
                    table.ForeignKey(
                        name: "FK_Tb_Project_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Shift",
                columns: table => new
                {
                    ShiftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShiftName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartTime = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    IsFlexible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MinStartTime = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    MaxStartTime = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    GracePeriodMinutes = table.Column<int>(type: "int", nullable: false),
                    RequiredWorkingHours = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Shift", x => x.ShiftId);
                    table.ForeignKey(
                        name: "FK_Tb_Shift_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Work_Location",
                columns: table => new
                {
                    WorkLocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    WorkLocationCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LocationName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    AllowedRadiusM = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Work_Location", x => x.WorkLocationId);
                    table.ForeignKey(
                        name: "FK_Tb_Work_Location_Tb_City_CityId",
                        column: x => x.CityId,
                        principalTable: "Tb_City",
                        principalColumn: "CityId");
                    table.ForeignKey(
                        name: "FK_Tb_Work_Location_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Job_Title",
                columns: table => new
                {
                    JobTitleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    TitleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JobLevelId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Job_Title", x => x.JobTitleId);
                    table.ForeignKey(
                        name: "FK_Tb_Job_Title_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Job_Title_Tb_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Tb_Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Job_Title_Tb_Job_Level_JobLevelId",
                        column: x => x.JobLevelId,
                        principalTable: "Tb_Job_Level",
                        principalColumn: "JobLevelId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeCodeFinance = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployeeCodeHr = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JobTitleId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArabicFirstName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArabicLastName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArabicFullName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Birthdate = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nationality = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NationalId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PassportNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaritalStatus = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlaceOfBirth = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BloodGroup = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FullName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArabicUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrivateMobile = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BuisnessMobile = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SerialMobile = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsTopmanager = table.Column<sbyte>(type: "tinyint", nullable: true),
                    IsFulldocument = table.Column<sbyte>(type: "tinyint", nullable: true),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    ShiftId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Tb_Employee_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Tb_Employee",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Tb_Job_Title_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "Tb_Job_Title",
                        principalColumn: "JobTitleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Shift_Rule",
                columns: table => new
                {
                    RuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JobTitleId = table.Column<int>(type: "int", nullable: true),
                    WorkingLocationId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Shift_Rule", x => x.RuleId);
                    table.ForeignKey(
                        name: "FK_Tb_Shift_Rule_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Shift_Rule_Tb_Job_Title_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "Tb_Job_Title",
                        principalColumn: "JobTitleId");
                    table.ForeignKey(
                        name: "FK_Tb_Shift_Rule_Tb_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Tb_Project",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_Tb_Shift_Rule_Tb_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Tb_Shift",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Shift_Rule_Tb_Work_Location_WorkingLocationId",
                        column: x => x.WorkingLocationId,
                        principalTable: "Tb_Work_Location",
                        principalColumn: "WorkLocationId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Activity",
                columns: table => new
                {
                    ActivityId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ActivityTypeId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    RequestBy = table.Column<long>(type: "bigint", nullable: false),
                    ApprovedBy = table.Column<long>(type: "bigint", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Activity", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Activity_Tb_Activity_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Tb_Activity_Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Activity_Tb_Activity_Type_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "Tb_Activity_Type",
                        principalColumn: "ActivityTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Activity_Tb_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Tb_Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Project",
                columns: table => new
                {
                    EmployeeProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Project", x => x.EmployeeProjectId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Project_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Project_Tb_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Tb_Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Project_Tb_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Tb_Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Shift",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    EffectiveDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Shift", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Shift_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Shift_Tb_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Tb_Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Shift_Tb_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Tb_Shift",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Vacation_Balance",
                columns: table => new
                {
                    BalanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    VacationTypeId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    TotalDays = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    UsedDays = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    RemainingDays = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Vacation_Balance", x => x.BalanceId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Vacation_Balance_Tb_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Tb_Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Vacation_Balance_Tb_Vacation_Type_VacationTypeId",
                        column: x => x.VacationTypeId,
                        principalTable: "Tb_Vacation_Type",
                        principalColumn: "VacationTypeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Work_Location",
                columns: table => new
                {
                    EmployeeWorkLocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    WorkLocationId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Work_Location", x => x.EmployeeWorkLocationId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Work_Location_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Work_Location_Tb_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Tb_Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Work_Location_Tb_Work_Location_WorkLocationId",
                        column: x => x.WorkLocationId,
                        principalTable: "Tb_Work_Location",
                        principalColumn: "WorkLocationId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Activity_Approval",
                columns: table => new
                {
                    ApprovalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    ChangedBy = table.Column<long>(type: "bigint", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Activity_Approval", x => x.ApprovalId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Activity_Approval_Tb_Activity_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Tb_Activity_Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Activity_Approval_Tb_Employee_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Tb_Employee_Activity",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Attendance",
                columns: table => new
                {
                    AttendanceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    AttendanceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FirstPuchin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastPuchout = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TotalHours = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Attendance", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Attendance_Tb_Employee_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Tb_Employee_Activity",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Excuse",
                columns: table => new
                {
                    ExcuseId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    ExcuseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    ExcuseReason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Excuse", x => x.ExcuseId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Excuse_Tb_Employee_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Tb_Employee_Activity",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Mission",
                columns: table => new
                {
                    MissionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    StartDatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MissionLocation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MissionReason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Mission", x => x.MissionId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Mission_Tb_Employee_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Tb_Employee_Activity",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Vacation",
                columns: table => new
                {
                    VacationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    VacationTypeId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DaysCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Vacation", x => x.VacationId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Vacation_Tb_Employee_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Tb_Employee_Activity",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Vacation_Tb_Vacation_Type_VacationTypeId",
                        column: x => x.VacationTypeId,
                        principalTable: "Tb_Vacation_Type",
                        principalColumn: "VacationTypeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Attendance_Punch",
                columns: table => new
                {
                    PunchId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AttendanceId = table.Column<long>(type: "bigint", nullable: false),
                    PunchIn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PunchOut = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    DeviceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Attendance_Punch", x => x.PunchId);
                    table.ForeignKey(
                        name: "FK_Tb_Employee_Attendance_Punch_Tb_Employee_Attendance_Attendan~",
                        column: x => x.AttendanceId,
                        principalTable: "Tb_Employee_Attendance",
                        principalColumn: "AttendanceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, "0b2f3b2f-a5d1-4d3f-be8b-db04070caed1", "SystemAdmin", "SYSTEMADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CompanyId", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "Email", "EmailConfirmed", "FailedLoginCount", "ForceLogout", "IsActive", "IsToChangePassword", "LastFailedLoginAt", "LastLoginAt", "LastPasswordChangedAt", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PreferredLanguage", "RowGuid", "SecurityStamp", "TwoFactorEnabled", "UserFullName", "UserName" },
                values: new object[] { 1, 0, 1, "2cc3da7b-b1d4-43fc-b129-4e706e02ac96", new DateTime(2025, 9, 8, 13, 3, 38, 309, DateTimeKind.Local).AddTicks(6082), null, "systemadmin@example.com", false, 0, false, true, false, null, null, new DateTime(2025, 9, 8, 10, 3, 38, 309, DateTimeKind.Utc).AddTicks(8818), false, null, "SYSTEMADMIN@EXAMPLE.COM", "BOLES", "AQAAAAIAAYagAAAAEHuYA7U5KAgI1iuzqry/7jPmIBrciy7nyILnyLHLuOwz3plNoiOeAavDPyJliZul9A==", "01200000000", true, "en", new Guid("fcac65de-74c4-4d49-acd7-4fa50e02ee85"), "6QVLU2WHQVYOV4FRB6EFKIGE2KJJICGL", false, "Boles Lewis Boles", "Boles" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspPermissions_PermissionName",
                table: "AspPermissions",
                column: "PermissionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspRolePermissions_PermissionId",
                table: "AspRolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Activity_Status_CompanyId",
                table: "Tb_Activity_Status",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Activity_Type_CompanyId",
                table: "Tb_Activity_Type",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Audit_Log_CompanyId",
                table: "Tb_Audit_Log",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_City_GovId",
                table: "Tb_City",
                column: "GovId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Company_GroupId",
                table: "Tb_Company",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Department_CompanyId",
                table: "Tb_Department",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_CompanyId",
                table: "Tb_Employee",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_JobTitleId",
                table: "Tb_Employee",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_ManagerId",
                table: "Tb_Employee",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Activity_ActivityTypeId",
                table: "Tb_Employee_Activity",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Activity_EmployeeId",
                table: "Tb_Employee_Activity",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Activity_StatusId",
                table: "Tb_Employee_Activity",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Activity_Approval_ActivityId",
                table: "Tb_Employee_Activity_Approval",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Activity_Approval_StatusId",
                table: "Tb_Employee_Activity_Approval",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Attendance_ActivityId",
                table: "Tb_Employee_Attendance",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Attendance_Punch_AttendanceId",
                table: "Tb_Employee_Attendance_Punch",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Excuse_ActivityId",
                table: "Tb_Employee_Excuse",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Mission_ActivityId",
                table: "Tb_Employee_Mission",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Project_CompanyId",
                table: "Tb_Employee_Project",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Project_EmployeeId",
                table: "Tb_Employee_Project",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Project_ProjectId",
                table: "Tb_Employee_Project",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Shift_CompanyId",
                table: "Tb_Employee_Shift",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Shift_EmployeeId",
                table: "Tb_Employee_Shift",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Shift_ShiftId",
                table: "Tb_Employee_Shift",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Vacation_ActivityId",
                table: "Tb_Employee_Vacation",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Vacation_VacationTypeId",
                table: "Tb_Employee_Vacation",
                column: "VacationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Vacation_Balance_EmployeeId",
                table: "Tb_Employee_Vacation_Balance",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Vacation_Balance_VacationTypeId",
                table: "Tb_Employee_Vacation_Balance",
                column: "VacationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Work_Location_CompanyId",
                table: "Tb_Employee_Work_Location",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Work_Location_EmployeeId",
                table: "Tb_Employee_Work_Location",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_Work_Location_WorkLocationId",
                table: "Tb_Employee_Work_Location",
                column: "WorkLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Job_Title_CompanyId",
                table: "Tb_Job_Title",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Job_Title_DepartmentId",
                table: "Tb_Job_Title",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Job_Title_JobLevelId",
                table: "Tb_Job_Title",
                column: "JobLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Project_CityId",
                table: "Tb_Project",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Project_CompanyId",
                table: "Tb_Project",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_CompanyId",
                table: "Tb_Shift",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_Rule_CompanyId",
                table: "Tb_Shift_Rule",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_Rule_JobTitleId",
                table: "Tb_Shift_Rule",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_Rule_ProjectId",
                table: "Tb_Shift_Rule",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_Rule_ShiftId",
                table: "Tb_Shift_Rule",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_Rule_WorkingLocationId",
                table: "Tb_Shift_Rule",
                column: "WorkingLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Vacation_Rule_VacationTypeId",
                table: "Tb_Vacation_Rule",
                column: "VacationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Work_Location_CityId",
                table: "Tb_Work_Location",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Work_Location_CompanyId",
                table: "Tb_Work_Location",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspRolePermissions");

            migrationBuilder.DropTable(
                name: "Tb_Audit_Log");

            migrationBuilder.DropTable(
                name: "Tb_Employee_Activity_Approval");

            migrationBuilder.DropTable(
                name: "Tb_Employee_Attendance_Punch");

            migrationBuilder.DropTable(
                name: "Tb_Employee_Excuse");

            migrationBuilder.DropTable(
                name: "Tb_Employee_Mission");

            migrationBuilder.DropTable(
                name: "Tb_Employee_Project");

            migrationBuilder.DropTable(
                name: "Tb_Employee_Shift");

            migrationBuilder.DropTable(
                name: "Tb_Employee_Vacation");

            migrationBuilder.DropTable(
                name: "Tb_Employee_Vacation_Balance");

            migrationBuilder.DropTable(
                name: "Tb_Employee_Work_Location");

            migrationBuilder.DropTable(
                name: "Tb_Shift_Rule");

            migrationBuilder.DropTable(
                name: "Tb_Vacation_Rule");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspPermissions");

            migrationBuilder.DropTable(
                name: "Tb_Employee_Attendance");

            migrationBuilder.DropTable(
                name: "Tb_Project");

            migrationBuilder.DropTable(
                name: "Tb_Shift");

            migrationBuilder.DropTable(
                name: "Tb_Work_Location");

            migrationBuilder.DropTable(
                name: "Tb_Vacation_Type");

            migrationBuilder.DropTable(
                name: "Tb_Employee_Activity");

            migrationBuilder.DropTable(
                name: "Tb_City");

            migrationBuilder.DropTable(
                name: "Tb_Activity_Status");

            migrationBuilder.DropTable(
                name: "Tb_Activity_Type");

            migrationBuilder.DropTable(
                name: "Tb_Employee");

            migrationBuilder.DropTable(
                name: "Tb_Gov");

            migrationBuilder.DropTable(
                name: "Tb_Job_Title");

            migrationBuilder.DropTable(
                name: "Tb_Department");

            migrationBuilder.DropTable(
                name: "Tb_Job_Level");

            migrationBuilder.DropTable(
                name: "Tb_Company");

            migrationBuilder.DropTable(
                name: "Tb_Group");
        }
    }
}
