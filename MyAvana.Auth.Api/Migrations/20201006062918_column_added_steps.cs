using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class column_added_steps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Step10Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step10Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step11Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step11Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step12Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step12Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step13Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step13Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step14Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step14Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step15Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step15Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step16Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step16Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step17Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step17Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step18Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step18Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step19Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step19Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step20Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step20Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step6Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step6Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step7Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step7Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step8Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step8Photo",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step9Instruction",
                table: "RegimenSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Step9Photo",
                table: "RegimenSteps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Step10Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step10Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step11Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step11Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step12Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step12Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step13Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step13Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step14Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step14Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step15Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step15Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step16Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step16Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step17Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step17Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step18Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step18Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step19Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step19Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step20Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step20Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step6Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step6Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step7Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step7Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step8Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step8Photo",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step9Instruction",
                table: "RegimenSteps");

            migrationBuilder.DropColumn(
                name: "Step9Photo",
                table: "RegimenSteps");
        }
    }
}
