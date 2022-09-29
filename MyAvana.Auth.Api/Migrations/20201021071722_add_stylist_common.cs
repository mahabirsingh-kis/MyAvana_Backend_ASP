using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_stylist_common : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StylishCommons",
                columns: table => new
                {
                    StylishCommonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    StylistId = table.Column<int>(nullable: false),
                    StylistSpecialtyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StylishCommons", x => x.StylishCommonId);
                    table.ForeignKey(
                        name: "FK_StylishCommons_Stylists_StylistId",
                        column: x => x.StylistId,
                        principalTable: "Stylists",
                        principalColumn: "StylistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StylishCommons_StylistSpecialties_StylistSpecialtyId",
                        column: x => x.StylistSpecialtyId,
                        principalTable: "StylistSpecialties",
                        principalColumn: "StylistSpecialtyId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StylishCommons_StylistId",
                table: "StylishCommons",
                column: "StylistId");

            migrationBuilder.CreateIndex(
                name: "IX_StylishCommons_StylistSpecialtyId",
                table: "StylishCommons",
                column: "StylistSpecialtyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StylishCommons");
        }
    }
}
