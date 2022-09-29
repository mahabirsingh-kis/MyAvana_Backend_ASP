using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
	public partial class promocodeupdate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "UpdatedDate",
				table: "PromoCodes",
				newName: "ExpireDate");

		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "ExpireDate",
				table: "PromoCodes",
				newName: "UpdatedDate");
		}
	}
}

