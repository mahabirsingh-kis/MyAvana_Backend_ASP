using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class alter_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questionaires_Answers_AnswerId",
                table: "Questionaires");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "Questionaires",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Questionaires_Answers_AnswerId",
                table: "Questionaires",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questionaires_Answers_AnswerId",
                table: "Questionaires");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "Questionaires",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questionaires_Answers_AnswerId",
                table: "Questionaires",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
