using Microsoft.EntityFrameworkCore.Migrations;

namespace Theater.Sql.Migrations
{
    public partial class ChangeForeignKeyForPieceWorkers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PieceWorkers_PieceDates_PieceId",
                table: "PieceWorkers");

            migrationBuilder.AddForeignKey(
                name: "FK_PieceWorkers_Pieces_PieceId",
                table: "PieceWorkers",
                column: "PieceId",
                principalTable: "Pieces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PieceWorkers_Pieces_PieceId",
                table: "PieceWorkers");

            migrationBuilder.AddForeignKey(
                name: "FK_PieceWorkers_PieceDates_PieceId",
                table: "PieceWorkers",
                column: "PieceId",
                principalTable: "PieceDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
