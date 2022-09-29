using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_observation_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HairObservations_Observations_ObservationId",
                table: "HairObservations");

            migrationBuilder.AlterColumn<int>(
                name: "ObservationId",
                table: "HairObservations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ObsBreakageId",
                table: "HairObservations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObsChemicalProductId",
                table: "HairObservations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObsDamageId",
                table: "HairObservations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObsElasticityId",
                table: "HairObservations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObsPhysicalProductId",
                table: "HairObservations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObsSplittingId",
                table: "HairObservations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ObsBreakage",
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
                    table.PrimaryKey("PK_ObsBreakage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObsChemicalProducts",
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
                    table.PrimaryKey("PK_ObsChemicalProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObsDamage",
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
                    table.PrimaryKey("PK_ObsDamage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObsElasticities",
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
                    table.PrimaryKey("PK_ObsElasticities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObsPhysicalProducts",
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
                    table.PrimaryKey("PK_ObsPhysicalProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObsSplitting",
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
                    table.PrimaryKey("PK_ObsSplitting", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_HairObservations_Observations_ObservationId",
                table: "HairObservations",
                column: "ObservationId",
                principalTable: "Observations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HairObservations_Observations_ObservationId",
                table: "HairObservations");

            migrationBuilder.DropTable(
                name: "ObsBreakage");

            migrationBuilder.DropTable(
                name: "ObsChemicalProducts");

            migrationBuilder.DropTable(
                name: "ObsDamage");

            migrationBuilder.DropTable(
                name: "ObsElasticities");

            migrationBuilder.DropTable(
                name: "ObsPhysicalProducts");

            migrationBuilder.DropTable(
                name: "ObsSplitting");

            migrationBuilder.DropColumn(
                name: "ObsBreakageId",
                table: "HairObservations");

            migrationBuilder.DropColumn(
                name: "ObsChemicalProductId",
                table: "HairObservations");

            migrationBuilder.DropColumn(
                name: "ObsDamageId",
                table: "HairObservations");

            migrationBuilder.DropColumn(
                name: "ObsElasticityId",
                table: "HairObservations");

            migrationBuilder.DropColumn(
                name: "ObsPhysicalProductId",
                table: "HairObservations");

            migrationBuilder.DropColumn(
                name: "ObsSplittingId",
                table: "HairObservations");

            migrationBuilder.AlterColumn<int>(
                name: "ObservationId",
                table: "HairObservations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HairObservations_Observations_ObservationId",
                table: "HairObservations",
                column: "ObservationId",
                principalTable: "Observations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
