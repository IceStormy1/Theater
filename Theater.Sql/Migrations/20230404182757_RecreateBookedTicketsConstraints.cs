using Microsoft.EntityFrameworkCore.Migrations;

namespace Theater.Sql.Migrations
{
    public partial class RecreateBookedTicketsConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IX_BookedTickets_PiecesTicketId",
                table: "BookedTickets");

            migrationBuilder.RenameTable(
                name: "BookedTickets",
                newName: "BookedTickets");

            migrationBuilder.RenameIndex(
                name: "IX_BookedTickets_UserId_Id",
                table: "BookedTickets",
                newName: "IX_BookedTickets_UserId_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookedTickets",
                table: "BookedTickets",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookedTickets_PiecesTicketId",
                table: "BookedTickets",
                column: "PiecesTicketId",
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
                name: "IX_BookedTickets_PiecesTicketId",
                table: "BookedTickets");

            migrationBuilder.RenameTable(
                name: "BookedTickets",
                newName: "BookedTicket");

            migrationBuilder.RenameIndex(
                name: "IX_BookedTickets_UserId_Id",
                table: "BookedTicket",
                newName: "IX_BookedTicket_UserId_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookedTicket",
                table: "BookedTicket",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookedTicket_PiecesTicketId",
                table: "BookedTicket",
                column: "PiecesTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedTicket_PiecesTickets_PiecesTicketId",
                table: "BookedTicket",
                column: "PiecesTicketId",
                principalTable: "PiecesTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookedTicket_Users_UserId",
                table: "BookedTicket",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
