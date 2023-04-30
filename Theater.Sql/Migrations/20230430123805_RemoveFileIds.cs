using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Theater.Sql.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFileIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("060b1905-0d40-483d-8462-32193864db03"));

            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("3f60120a-ac2e-4636-ac65-85f01172b787"));

            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("973e5bca-1f6e-4c50-b988-872aa2db5377"));

            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("f08238c8-0f7f-467d-a8c7-468ca76c4235"));

            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("3f60120a-ac2e-4636-ac65-85f01172b787")); 
            
            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("973e5bca-1f6e-4c50-b988-872aa2db5377"));
            
            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("f08238c8-0f7f-467d-a8c7-468ca76c4235")); 
            
            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("060b1905-0d40-483d-8462-32193864db03"));

            migrationBuilder.AddColumn<Guid>(
                name: "MainPhotoId",
                table: "Pieces",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "WorkersPositions",
                columns: new[] { "Id", "PositionName", "PositionType" },
                values: new object[,]
                {
                    { new Guid("631c7430-2620-4523-b8a1-a2c0634e85bd"), "Художник", 3 },
                    { new Guid("67bb936a-3bd6-4c6c-aaa2-5e3a55d5fdc2"), "Режиссер-постановщик", 1 },
                    { new Guid("ebf8a51a-8dc3-4a46-8c15-ced2e1d15736"), "Гитарист", 4 },
                    { new Guid("f5b16125-f017-46a6-a6e3-feaa8ca43d68"), "Заслуженный актер", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pieces_MainPhotoId",
                table: "Pieces",
                column: "MainPhotoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pieces_Files_MainPhotoId",
                table: "Pieces",
                column: "MainPhotoId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pieces_Files_MainPhotoId",
                table: "Pieces");

            migrationBuilder.DropIndex(
                name: "IX_Pieces_MainPhotoId",
                table: "Pieces");

            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("631c7430-2620-4523-b8a1-a2c0634e85bd"));

            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("67bb936a-3bd6-4c6c-aaa2-5e3a55d5fdc2"));

            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("ebf8a51a-8dc3-4a46-8c15-ced2e1d15736"));

            migrationBuilder.DeleteData(
                table: "WorkersPositions",
                keyColumn: "Id",
                keyValue: new Guid("f5b16125-f017-46a6-a6e3-feaa8ca43d68"));

            migrationBuilder.DropColumn(
                name: "MainPhotoId",
                table: "Pieces");

            migrationBuilder.InsertData(
                table: "WorkersPositions",
                columns: new[] { "Id", "PositionName", "PositionType" },
                values: new object[,]
                {
                    { new Guid("060b1905-0d40-483d-8462-32193864db03"), "Заслуженный актер", 2 },
                    { new Guid("3f60120a-ac2e-4636-ac65-85f01172b787"), "Режиссер-постановщик", 1 },
                    { new Guid("973e5bca-1f6e-4c50-b988-872aa2db5377"), "Гитарист", 4 },
                    { new Guid("f08238c8-0f7f-467d-a8c7-468ca76c4235"), "Художник", 3 }
                });
        }
    }
}
