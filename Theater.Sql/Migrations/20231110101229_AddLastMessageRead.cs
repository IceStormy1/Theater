using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Theater.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddLastMessageRead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LastReadMessageId",
                schema: "chat",
                table: "UserRooms",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReadMessageTime",
                schema: "chat",
                table: "UserRooms",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRooms_LastReadMessageId",
                schema: "chat",
                table: "UserRooms",
                column: "LastReadMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRooms_Messages_LastReadMessageId",
                schema: "chat",
                table: "UserRooms",
                column: "LastReadMessageId",
                principalSchema: "chat",
                principalTable: "Messages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRooms_Messages_LastReadMessageId",
                schema: "chat",
                table: "UserRooms");

            migrationBuilder.DropIndex(
                name: "IX_UserRooms_LastReadMessageId",
                schema: "chat",
                table: "UserRooms");

            migrationBuilder.DropColumn(
                name: "LastReadMessageId",
                schema: "chat",
                table: "UserRooms");

            migrationBuilder.DropColumn(
                name: "LastReadMessageTime",
                schema: "chat",
                table: "UserRooms");
        }
    }
}
