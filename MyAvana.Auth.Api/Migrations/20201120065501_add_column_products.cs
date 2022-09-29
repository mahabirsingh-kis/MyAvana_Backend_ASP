using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_column_products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HairChallenges",
                table: "ProductEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductIndicator",
                table: "ProductEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductTags",
                table: "ProductEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HairChallenges",
                table: "ProductEntities");

            migrationBuilder.DropColumn(
                name: "ProductIndicator",
                table: "ProductEntities");

            migrationBuilder.DropColumn(
                name: "ProductTags",
                table: "ProductEntities");
        }
    }
}
