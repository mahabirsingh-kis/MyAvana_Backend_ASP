using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class socialmediatablefields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Link",
                table: "MediaLinkEntities",
                newName: "VideoId");

            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "MediaLinkEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "MediaLinkEntities");

            migrationBuilder.RenameColumn(
                name: "VideoId",
                table: "MediaLinkEntities",
                newName: "Link");
        }
    }
}
