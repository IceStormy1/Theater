using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Theater.Sql.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cd448464-2ec0-4b21-b5fa-9a3cc8547489"),
                column: "Password",
                value: "E10ADC3949BA59ABBE56E057F20F883E");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e1f83d38-56a7-435b-94bd-fe891ed0f03a"),
                column: "Password",
                value: "E10ADC3949BA59ABBE56E057F20F883E");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f2343d16-e610-4a73-a0f0-b9f63df511e6"),
                column: "Password",
                value: "E10ADC3949BA59ABBE56E057F20F883E");
        }
    }
}
