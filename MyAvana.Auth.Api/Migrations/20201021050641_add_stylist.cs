using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_stylist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StylistSpecialties",
                columns: table => new
                {
                    StylistSpecialtyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StylistSpecialties", x => x.StylistSpecialtyId);
                });

            migrationBuilder.CreateTable(
                name: "Stylists",
                columns: table => new
                {
                    StylistId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StylistName = table.Column<string>(nullable: true),
                    SalonName = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    Facebook = table.Column<string>(nullable: true),
                    Background = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    StylistSpecialtyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stylists", x => x.StylistId);
                    table.ForeignKey(
                        name: "FK_Stylists_StylistSpecialties_StylistSpecialtyId",
                        column: x => x.StylistSpecialtyId,
                        principalTable: "StylistSpecialties",
                        principalColumn: "StylistSpecialtyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stylists_StylistSpecialtyId",
                table: "Stylists",
                column: "StylistSpecialtyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stylists");

            migrationBuilder.DropTable(
                name: "StylistSpecialties");
        }
    }
}
