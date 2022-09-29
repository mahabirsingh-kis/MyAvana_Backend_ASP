using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class prepopulatetypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAllStyling",
                table: "RecommendedProductsStyleRegimens",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAllEssential",
                table: "RecommendedProducts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "DailyRoutineTracker",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAllStyling",
                table: "RecommendedProductsStyleRegimens");

            migrationBuilder.DropColumn(
                name: "IsAllEssential",
                table: "RecommendedProducts");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "DailyRoutineTracker");
        }
    }
}
