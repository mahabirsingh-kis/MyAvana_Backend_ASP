using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class update_columnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductEntityId",
                table: "ProductCommons",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommons_ProductEntityId",
                table: "ProductCommons",
                column: "ProductEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCommons_ProductEntities_ProductEntityId",
                table: "ProductCommons",
                column: "ProductEntityId",
                principalTable: "ProductEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCommons_ProductEntities_ProductEntityId",
                table: "ProductCommons");

            migrationBuilder.DropIndex(
                name: "IX_ProductCommons_ProductEntityId",
                table: "ProductCommons");

            migrationBuilder.DropColumn(
                name: "ProductEntityId",
                table: "ProductCommons");
        }
    }
}
