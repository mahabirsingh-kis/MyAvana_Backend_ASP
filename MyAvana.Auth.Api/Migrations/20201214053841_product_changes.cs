using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class product_changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HairChallenges",
                columns: table => new
                {
                    HairChallengeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairChallenges", x => x.HairChallengeId);
                });

            migrationBuilder.CreateTable(
                name: "HairTypes",
                columns: table => new
                {
                    HairTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairTypes", x => x.HairTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ProductClassification",
                columns: table => new
                {
                    ProductClassificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductClassification", x => x.ProductClassificationId);
                });

            migrationBuilder.CreateTable(
                name: "ProductIndicator",
                columns: table => new
                {
                    ProductIndicatorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIndicator", x => x.ProductIndicatorId);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    ProductTagsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => x.ProductTagsId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCommons",
                columns: table => new
                {
                    ProductCommonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    HairTypeId = table.Column<int>(nullable: true),
                    HairChallengeId = table.Column<int>(nullable: true),
                    ProductIndicatorId = table.Column<int>(nullable: true),
                    ProductTagsId = table.Column<int>(nullable: true),
                    ProductClassificationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCommons", x => x.ProductCommonId);
                    table.ForeignKey(
                        name: "FK_ProductCommons_HairChallenges_HairChallengeId",
                        column: x => x.HairChallengeId,
                        principalTable: "HairChallenges",
                        principalColumn: "HairChallengeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCommons_HairTypes_HairTypeId",
                        column: x => x.HairTypeId,
                        principalTable: "HairTypes",
                        principalColumn: "HairTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCommons_ProductClassification_ProductClassificationId",
                        column: x => x.ProductClassificationId,
                        principalTable: "ProductClassification",
                        principalColumn: "ProductClassificationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCommons_ProductIndicator_ProductIndicatorId",
                        column: x => x.ProductIndicatorId,
                        principalTable: "ProductIndicator",
                        principalColumn: "ProductIndicatorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCommons_ProductTags_ProductTagsId",
                        column: x => x.ProductTagsId,
                        principalTable: "ProductTags",
                        principalColumn: "ProductTagsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommons_HairChallengeId",
                table: "ProductCommons",
                column: "HairChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommons_HairTypeId",
                table: "ProductCommons",
                column: "HairTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommons_ProductClassificationId",
                table: "ProductCommons",
                column: "ProductClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommons_ProductIndicatorId",
                table: "ProductCommons",
                column: "ProductIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommons_ProductTagsId",
                table: "ProductCommons",
                column: "ProductTagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCommons");

            migrationBuilder.DropTable(
                name: "HairChallenges");

            migrationBuilder.DropTable(
                name: "HairTypes");

            migrationBuilder.DropTable(
                name: "ProductClassification");

            migrationBuilder.DropTable(
                name: "ProductIndicator");

            migrationBuilder.DropTable(
                name: "ProductTags");
        }
    }
}
