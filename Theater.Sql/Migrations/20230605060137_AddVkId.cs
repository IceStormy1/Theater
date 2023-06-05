using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Theater.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddVkId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VkId",
                table: "Users",
                type: "integer",
                nullable: true);

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
                name: "IX_Users_VkId",
                table: "Users",
                column: "VkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_VkId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VkId",
                table: "Users");
        }
    }
}
