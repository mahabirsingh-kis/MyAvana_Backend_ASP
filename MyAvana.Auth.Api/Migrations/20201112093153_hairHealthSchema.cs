using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class hairHealthSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "City",
            //    table: "AspNetUsers",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "Healths",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Healths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HairHealths",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HairProfileId = table.Column<int>(nullable: false),
                    HealthId = table.Column<int>(nullable: false),
                    IsTopLeft = table.Column<bool>(nullable: false),
                    IsTopRight = table.Column<bool>(nullable: false),
                    IsBottomLeft = table.Column<bool>(nullable: false),
                    IsBottomRight = table.Column<bool>(nullable: false),
                    IsCrown = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairHealths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HairHealths_HairProfiles_HairProfileId",
                        column: x => x.HairProfileId,
                        principalTable: "HairProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HairHealths_Healths_HealthId",
                        column: x => x.HealthId,
                        principalTable: "Healths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HairHealths_HairProfileId",
                table: "HairHealths",
                column: "HairProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_HairHealths_HealthId",
                table: "HairHealths",
                column: "HealthId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HairHealths");

            migrationBuilder.DropTable(
                name: "Healths");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");
        }
    }
}
