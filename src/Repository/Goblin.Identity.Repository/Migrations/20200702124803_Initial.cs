using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Goblin.Identity.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
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
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
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
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

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
                    PasswordLastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
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

            migrationBuilder.CreateTable(
                name: "RolePermission",
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
                    RoleId = table.Column<long>(nullable: false),
                    PermissionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
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
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_CreatedTime",
                table: "Permission",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_DeletedTime",
                table: "Permission",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Id",
                table: "Permission",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_LastUpdatedTime",
                table: "Permission",
                column: "LastUpdatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Name",
                table: "Permission",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedTime",
                table: "Role",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Role_DeletedTime",
                table: "Role",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Id",
                table: "Role",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Role_LastUpdatedTime",
                table: "Role",
                column: "LastUpdatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_CreatedTime",
                table: "RolePermission",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_DeletedTime",
                table: "RolePermission",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_Id",
                table: "RolePermission",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_LastUpdatedTime",
                table: "RolePermission",
                column: "LastUpdatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");

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

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_CreatedTime",
                table: "UserRole",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_DeletedTime",
                table: "UserRole",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Id",
                table: "UserRole",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_LastUpdatedTime",
                table: "UserRole",
                column: "LastUpdatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
