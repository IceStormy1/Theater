using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Theater.Sql.Migrations
{
    /// <inheritdoc />
    public partial class RecreateIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchasedUserTickets_TicketPriceEventsVersion_TicketPriceEv~",
                table: "PurchasedUserTickets");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedUserTickets_TicketPriceEventsVersion_TicketPriceEv~",
                table: "PurchasedUserTickets",
                columns: new[] { "TicketPriceEventsVersion", "TicketPriceEventsId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchasedUserTickets_TicketPriceEventsVersion_TicketPriceEv~",
                table: "PurchasedUserTickets");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedUserTickets_TicketPriceEventsVersion_TicketPriceEv~",
                table: "PurchasedUserTickets",
                columns: new[] { "TicketPriceEventsVersion", "TicketPriceEventsId" });
        }
    }
}
