using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class product_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductTypeCategoryId",
                table: "ProductTypes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductTypeCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsHair = table.Column<bool>(nullable: true),
                    IsRegimens = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypeCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_ProductTypeCategoryId",
                table: "ProductTypes",
                column: "ProductTypeCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypes_ProductTypeCategories_ProductTypeCategoryId",
                table: "ProductTypes",
                column: "ProductTypeCategoryId",
                principalTable: "ProductTypeCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypes_ProductTypeCategories_ProductTypeCategoryId",
                table: "ProductTypes");

            migrationBuilder.DropTable(
                name: "ProductTypeCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductTypes_ProductTypeCategoryId",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "ProductTypeCategoryId",
                table: "ProductTypes");
        }
    }
}
