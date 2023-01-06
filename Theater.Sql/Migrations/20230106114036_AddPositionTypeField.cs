using Microsoft.EntityFrameworkCore.Migrations;

namespace Theater.Sql.Migrations
{
    public partial class AddPositionTypeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionType",
                table: "WorkersPositions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionType",
                table: "WorkersPositions");
        }
    }
}
