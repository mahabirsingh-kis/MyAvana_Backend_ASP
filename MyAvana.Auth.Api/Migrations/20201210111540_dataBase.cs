using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class dataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ObsChemicalProductsId",
                table: "HairObservations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObsPhysicalProductsId",
                table: "HairObservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HairObservations_ObsBreakageId",
                table: "HairObservations",
                column: "ObsBreakageId");

            migrationBuilder.CreateIndex(
                name: "IX_HairObservations_ObsChemicalProductsId",
                table: "HairObservations",
                column: "ObsChemicalProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_HairObservations_ObsDamageId",
                table: "HairObservations",
                column: "ObsDamageId");

            migrationBuilder.CreateIndex(
                name: "IX_HairObservations_ObsElasticityId",
                table: "HairObservations",
                column: "ObsElasticityId");

            migrationBuilder.CreateIndex(
                name: "IX_HairObservations_ObsPhysicalProductsId",
                table: "HairObservations",
                column: "ObsPhysicalProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_HairObservations_ObsSplittingId",
                table: "HairObservations",
                column: "ObsSplittingId");

            migrationBuilder.AddForeignKey(
                name: "FK_HairObservations_ObsBreakage_ObsBreakageId",
                table: "HairObservations",
                column: "ObsBreakageId",
                principalTable: "ObsBreakage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HairObservations_ObsChemicalProducts_ObsChemicalProductsId",
                table: "HairObservations",
                column: "ObsChemicalProductsId",
                principalTable: "ObsChemicalProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HairObservations_ObsDamage_ObsDamageId",
                table: "HairObservations",
                column: "ObsDamageId",
                principalTable: "ObsDamage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HairObservations_ObsElasticities_ObsElasticityId",
                table: "HairObservations",
                column: "ObsElasticityId",
                principalTable: "ObsElasticities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HairObservations_ObsPhysicalProducts_ObsPhysicalProductsId",
                table: "HairObservations",
                column: "ObsPhysicalProductsId",
                principalTable: "ObsPhysicalProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HairObservations_ObsSplitting_ObsSplittingId",
                table: "HairObservations",
                column: "ObsSplittingId",
                principalTable: "ObsSplitting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HairObservations_ObsBreakage_ObsBreakageId",
                table: "HairObservations");

            migrationBuilder.DropForeignKey(
                name: "FK_HairObservations_ObsChemicalProducts_ObsChemicalProductsId",
                table: "HairObservations");

            migrationBuilder.DropForeignKey(
                name: "FK_HairObservations_ObsDamage_ObsDamageId",
                table: "HairObservations");

            migrationBuilder.DropForeignKey(
                name: "FK_HairObservations_ObsElasticities_ObsElasticityId",
                table: "HairObservations");

            migrationBuilder.DropForeignKey(
                name: "FK_HairObservations_ObsPhysicalProducts_ObsPhysicalProductsId",
                table: "HairObservations");

            migrationBuilder.DropForeignKey(
                name: "FK_HairObservations_ObsSplitting_ObsSplittingId",
                table: "HairObservations");

            migrationBuilder.DropIndex(
                name: "IX_HairObservations_ObsBreakageId",
                table: "HairObservations");

            migrationBuilder.DropIndex(
                name: "IX_HairObservations_ObsChemicalProductsId",
                table: "HairObservations");

            migrationBuilder.DropIndex(
                name: "IX_HairObservations_ObsDamageId",
                table: "HairObservations");

            migrationBuilder.DropIndex(
                name: "IX_HairObservations_ObsElasticityId",
                table: "HairObservations");

            migrationBuilder.DropIndex(
                name: "IX_HairObservations_ObsPhysicalProductsId",
                table: "HairObservations");

            migrationBuilder.DropIndex(
                name: "IX_HairObservations_ObsSplittingId",
                table: "HairObservations");

            migrationBuilder.DropColumn(
                name: "ObsChemicalProductsId",
                table: "HairObservations");

            migrationBuilder.DropColumn(
                name: "ObsPhysicalProductsId",
                table: "HairObservations");
        }
    }
}
