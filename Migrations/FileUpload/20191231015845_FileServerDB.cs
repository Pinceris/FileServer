using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApp1.Migrations.FileUpload
{
    public partial class FileServerDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Downloads = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
