using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class remove_foreignKey_stylish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stylists_StylistSpecialties_StylistSpecialtyId",
                table: "Stylists");

            migrationBuilder.DropIndex(
                name: "IX_Stylists_StylistSpecialtyId",
                table: "Stylists");

            migrationBuilder.DropColumn(
                name: "StylistSpecialtyId",
                table: "Stylists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StylistSpecialtyId",
                table: "Stylists",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Stylists_StylistSpecialtyId",
                table: "Stylists",
                column: "StylistSpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stylists_StylistSpecialties_StylistSpecialtyId",
                table: "Stylists",
                column: "StylistSpecialtyId",
                principalTable: "StylistSpecialties",
                principalColumn: "StylistSpecialtyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
