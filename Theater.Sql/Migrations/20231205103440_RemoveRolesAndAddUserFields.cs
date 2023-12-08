using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Theater.Sql.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRolesAndAddUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRoles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_VkId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VkId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Users",
                newName: "Role");

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalUserId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Snils",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cd448464-2ec0-4b21-b5fa-9a3cc8547489"),
                columns: new[] { "ExternalUserId", "Role", "Snils" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000000"), 4, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e1f83d38-56a7-435b-94bd-fe891ed0f03a"),
                columns: new[] { "ExternalUserId", "Snils" },
                values: new object[] { new Guid("e1f83d38-56a7-435b-94bd-fe891ed0f03a"), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f2343d16-e610-4a73-a0f0-b9f63df511e6"),
                columns: new[] { "ExternalUserId", "Snils" },
                values: new object[] { new Guid("f2343d16-e610-4a73-a0f0-b9f63df511e6"), null });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExternalUserId",
                table: "Users",
                column: "ExternalUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName_ExternalUserId",
                table: "Users",
                columns: new[] { "UserName", "ExternalUserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_ExternalUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserName_ExternalUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExternalUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Snils",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.AddColumn<int>(
                name: "VkId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" },
                    { 3, "System" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cd448464-2ec0-4b21-b5fa-9a3cc8547489"),
                columns: new[] { "RoleId", "VkId" },
                values: new object[] { 3, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e1f83d38-56a7-435b-94bd-fe891ed0f03a"),
                column: "VkId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f2343d16-e610-4a73-a0f0-b9f63df511e6"),
                column: "VkId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_VkId",
                table: "Users",
                column: "VkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRoles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
