using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class hairStrands_images_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HairStrandsImages",
                columns: table => new
                {
                    StrandsImagesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TopLeftImage = table.Column<string>(nullable: true),
                    TopRightImage = table.Column<string>(nullable: true),
                    BottomLeftImage = table.Column<string>(nullable: true),
                    BottomRightImage = table.Column<string>(nullable: true),
                    CrownImage = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairStrandsImages", x => x.StrandsImagesId);
                    table.ForeignKey(
                        name: "FK_HairStrandsImages_HairStrands_Id",
                        column: x => x.Id,
                        principalTable: "HairStrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HairStrandsImages_Id",
                table: "HairStrandsImages",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HairStrandsImages");
        }
    }
}
