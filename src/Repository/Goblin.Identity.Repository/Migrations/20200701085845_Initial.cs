using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Goblin.Identity.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
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
                    AvatarUrl = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    GithubId = table.Column<string>(nullable: true),
                    SkypeId = table.Column<string>(nullable: true),
                    FacebookId = table.Column<string>(nullable: true),
                    WebsiteUrl = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    CompanyUrl = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PasswordLastUpdatedTime = table.Column<DateTimeOffset>(nullable: true),
                    SetPasswordToken = table.Column<string>(nullable: true),
                    SetPasswordTokenExpireTime = table.Column<DateTimeOffset>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmedTime = table.Column<DateTimeOffset>(nullable: true),
                    EmailConfirmToken = table.Column<string>(nullable: true),
                    EmailConfirmTokenExpireTime = table.Column<DateTimeOffset>(nullable: true),
                    RevokeTokenGeneratedBeforeTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedTime",
                table: "User",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedTime",
                table: "User",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_User_Id",
                table: "User",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastUpdatedTime",
                table: "User",
                column: "LastUpdatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
