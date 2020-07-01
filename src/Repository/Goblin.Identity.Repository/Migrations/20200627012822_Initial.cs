using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Goblin.Identity.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sample",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    LastUpdatedBy = table.Column<long>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    SampleData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sample", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sample_CreatedTime",
                table: "Sample",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Sample_DeletedTime",
                table: "Sample",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Sample_Id",
                table: "Sample",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sample_LastUpdatedTime",
                table: "Sample",
                column: "LastUpdatedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sample");
        }
    }
}
