using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class _questionaireDb_removeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Option1",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Option2",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Option3",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "Option4",
                table: "Answers",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "Questions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Question",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Answers",
                newName: "Option4");

            migrationBuilder.AddColumn<string>(
                name: "Option1",
                table: "Answers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option2",
                table: "Answers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option3",
                table: "Answers",
                nullable: true);
        }
    }
}
