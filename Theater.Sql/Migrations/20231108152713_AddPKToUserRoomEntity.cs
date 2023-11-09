using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Theater.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddPKToUserRoomEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRooms",
                schema: "chat",
                table: "UserRooms");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "chat",
                table: "UserRooms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql(@$"CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";
UPDATE chat.""UserRooms""
SET ""Id"" = uuid_generate_v4()
WHERE ""Id"" = '00000000-0000-0000-0000-000000000000'");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRooms",
                schema: "chat",
                table: "UserRooms",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRooms_UserId_RoomId",
                schema: "chat",
                table: "UserRooms",
                columns: new[] { "UserId", "RoomId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRooms",
                schema: "chat",
                table: "UserRooms");

            migrationBuilder.DropIndex(
                name: "IX_UserRooms_UserId_RoomId",
                schema: "chat",
                table: "UserRooms");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "chat",
                table: "UserRooms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRooms",
                schema: "chat",
                table: "UserRooms",
                columns: new[] { "UserId", "RoomId" });
        }
    }
}
