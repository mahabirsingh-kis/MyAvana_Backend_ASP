using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    AccountNo = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    LoginAlert = table.Column<bool>(nullable: false),
                    TwoFactorTrans = table.Column<bool>(nullable: false),
                    IsBlocked = table.Column<bool>(nullable: false),
                    Subscribe = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    StripeCustomerId = table.Column<string>(nullable: true),
                    HubSpotContactId = table.Column<string>(nullable: true),
                    TicketUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    OpCode = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    TemplateCode = table.Column<string>(nullable: false),
                    TemplateType = table.Column<string>(nullable: true),
                    TemplateName = table.Column<string>(nullable: true),
                    SMTPUsername = table.Column<string>(nullable: true),
                    SenderEmail = table.Column<string>(nullable: true),
                    SenderName = table.Column<string>(nullable: true),
                    SMTPPassword = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    HostPort = table.Column<int>(nullable: false),
                    EnableSSL = table.Column<bool>(nullable: false),
                    TimeOut = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.TemplateCode);
                });

            migrationBuilder.CreateTable(
                name: "GenericSettings",
                columns: table => new
                {
                    SettingID = table.Column<Guid>(nullable: false),
                    AdminAccountId = table.Column<string>(nullable: false),
                    SettingName = table.Column<string>(nullable: false),
                    SubSettingName = table.Column<string>(nullable: false),
                    DefaultTextValue20_1 = table.Column<string>(maxLength: 20, nullable: true),
                    DefaultTextValue20_2 = table.Column<string>(maxLength: 20, nullable: true),
                    DefaultTextValue50_1 = table.Column<string>(maxLength: 50, nullable: true),
                    DefaultTextValue50_2 = table.Column<string>(maxLength: 50, nullable: true),
                    DefaultTextValue100_1 = table.Column<string>(maxLength: 100, nullable: true),
                    DefaultTextValue100_2 = table.Column<string>(maxLength: 100, nullable: true),
                    DefaultTextValue250_1 = table.Column<string>(maxLength: 250, nullable: true),
                    DefaultTextValue250_2 = table.Column<string>(maxLength: 250, nullable: true),
                    DefaultTextMax = table.Column<string>(nullable: true),
                    DefaultTextMax1 = table.Column<string>(nullable: true),
                    DefaultTextMax2 = table.Column<string>(nullable: true),
                    DefaultTextMax3 = table.Column<string>(nullable: true),
                    DefaultTextMax4 = table.Column<string>(nullable: true),
                    DefalutInteger1 = table.Column<int>(nullable: false),
                    DefalutInteger2 = table.Column<int>(nullable: false),
                    DefalutInteger3 = table.Column<int>(nullable: false),
                    DefalutInteger4 = table.Column<int>(nullable: false),
                    DefalutInteger5 = table.Column<int>(nullable: false),
                    DefaultDecimal1 = table.Column<decimal>(nullable: false),
                    DefaultDecimal2 = table.Column<decimal>(nullable: false),
                    DefaultDecimal3 = table.Column<decimal>(nullable: false),
                    DefaultDecimal4 = table.Column<decimal>(nullable: false),
                    DefaultDecimal5 = table.Column<decimal>(nullable: false),
                    DefaultDateTime1 = table.Column<DateTime>(nullable: true),
                    DefaultDateTime2 = table.Column<DateTime>(nullable: true),
                    DefaultDateTime3 = table.Column<DateTime>(nullable: true),
                    DefaultDateTime4 = table.Column<DateTime>(nullable: true),
                    DefaultDateTime5 = table.Column<DateTime>(nullable: true),
                    DefaultBool1 = table.Column<bool>(nullable: false),
                    DefaultBool2 = table.Column<bool>(nullable: false),
                    DefaultBool3 = table.Column<bool>(nullable: false),
                    DefaultBool4 = table.Column<bool>(nullable: false),
                    DefaultBool5 = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericSettings", x => new { x.SettingID, x.AdminAccountId, x.SettingName, x.SubSettingName });
                    table.UniqueConstraint("AK_GenericSettings_AdminAccountId_SettingID_SettingName_SubSettingName", x => new { x.AdminAccountId, x.SettingID, x.SettingName, x.SubSettingName });
                });

            migrationBuilder.CreateTable(
                name: "MediaLinkEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VideoId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    ImageLink = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Header = table.Column<string>(nullable: true),
                    IsFeatured = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaLinkEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentEntities",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(nullable: false),
                    PaymentAmount = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    CCNumber = table.Column<string>(nullable: true),
                    ProviderId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ProviderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentEntities", x => x.PaymentId);
                });

            migrationBuilder.CreateTable(
                name: "ProductEntities",
                columns: table => new
                {
                    guid = table.Column<Guid>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    ActualName = table.Column<string>(nullable: true),
                    BrandName = table.Column<string>(nullable: true),
                    TypeFor = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    Ingredients = table.Column<string>(nullable: true),
                    ProductDetails = table.Column<string>(nullable: true),
                    ProductLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEntities", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionsEntities",
                columns: table => new
                {
                    StripePlanId = table.Column<string>(nullable: false),
                    PlanName = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    Validity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionsEntities", x => x.StripePlanId);
                });

            migrationBuilder.CreateTable(
                name: "UserEmails",
                columns: table => new
                {
                    UserEmailId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEmails", x => x.UserEmailId);
                });

            migrationBuilder.CreateTable(
                name: "UserHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    AccessTime = table.Column<DateTime>(nullable: true),
                    UsedCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersTicketsEntities",
                columns: table => new
                {
                    TicketId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<string>(nullable: true),
                    Priority = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTicketsEntities", x => x.TicketId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

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
                name: "CodeEntities");

            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "GenericSettings");

            migrationBuilder.DropTable(
                name: "MediaLinkEntities");

            migrationBuilder.DropTable(
                name: "PaymentEntities");

            migrationBuilder.DropTable(
                name: "ProductEntities");

            migrationBuilder.DropTable(
                name: "PromoCodes");

            migrationBuilder.DropTable(
                name: "SubscriptionsEntities");

            migrationBuilder.DropTable(
                name: "UserEmails");

            migrationBuilder.DropTable(
                name: "UserHistories");

            migrationBuilder.DropTable(
                name: "UsersTicketsEntities");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
