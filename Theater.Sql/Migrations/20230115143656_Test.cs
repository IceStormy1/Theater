using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Theater.Sql.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedTicketsEntity_PiecesTickets_PiecesTicketId",
                table: "BookedTicketsEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BookedTicketsEntity_Users_UserId",
                table: "BookedTicketsEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookedTicketsEntity",
                table: "BookedTicketsEntity");

            migrationBuilder.DropIndex(
                name: "IX_BookedTicketsEntity_UserId",
                table: "BookedTicketsEntity");

            migrationBuilder.RenameTable(
                name: "BookedTicketsEntity",
                newName: "BookedTickets");

            migrationBuilder.RenameIndex(
                name: "IX_BookedTicketsEntity_PiecesTicketId",
                table: "BookedTickets",
                newName: "IX_BookedTickets_PiecesTicketId");

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                table: "TheaterWorkers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketPrice",
                table: "PiecesTickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookedTickets",
                table: "BookedTickets",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookedTickets_UserId_Id",
                table: "BookedTickets",
                columns: new[] { "UserId", "Id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookedTickets_PiecesTickets_PiecesTicketId",
                table: "BookedTickets",
                column: "PiecesTicketId",
                principalTable: "PiecesTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookedTickets_Users_UserId",
                table: "BookedTickets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedTickets_PiecesTickets_PiecesTicketId",
                table: "BookedTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_BookedTickets_Users_UserId",
                table: "BookedTickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookedTickets",
                table: "BookedTickets");

            migrationBuilder.DropIndex(
                name: "IX_BookedTickets_UserId_Id",
                table: "BookedTickets");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "TheaterWorkers");

            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "PiecesTickets");

            migrationBuilder.RenameTable(
                name: "BookedTickets",
                newName: "BookedTicketsEntity");

            migrationBuilder.RenameIndex(
                name: "IX_BookedTickets_PiecesTicketId",
                table: "BookedTicketsEntity",
                newName: "IX_BookedTicketsEntity_PiecesTicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookedTicketsEntity",
                table: "BookedTicketsEntity",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookedTicketsEntity_UserId",
                table: "BookedTicketsEntity",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedTicketsEntity_PiecesTickets_PiecesTicketId",
                table: "BookedTicketsEntity",
                column: "PiecesTicketId",
                principalTable: "PiecesTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookedTicketsEntity_Users_UserId",
                table: "BookedTicketsEntity",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
