using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class producttype_withForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEntities_ProductTypes_PTypeIdProductTypeId",
                table: "ProductEntities");

            migrationBuilder.RenameColumn(
                name: "PTypeIdProductTypeId",
                table: "ProductEntities",
                newName: "ProductTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductEntities_PTypeIdProductTypeId",
                table: "ProductEntities",
                newName: "IX_ProductEntities_ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEntities_ProductTypes_ProductTypeId",
                table: "ProductEntities",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "ProductTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEntities_ProductTypes_ProductTypeId",
                table: "ProductEntities");

            migrationBuilder.RenameColumn(
                name: "ProductTypeId",
                table: "ProductEntities",
                newName: "PTypeIdProductTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductEntities_ProductTypeId",
                table: "ProductEntities",
                newName: "IX_ProductEntities_PTypeIdProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEntities_ProductTypes_PTypeIdProductTypeId",
                table: "ProductEntities",
                column: "PTypeIdProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "ProductTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
