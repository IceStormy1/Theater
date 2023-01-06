using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Theater.Entities.Theater;

namespace Theater.Sql.Migrations
{
    public partial class AddTheaterTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Money",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    FileStorageName = table.Column<string>(type: "text", nullable: true),
                    BucketId = table.Column<int>(type: "integer", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UploadAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PiecesGenres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    GenreName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PiecesGenres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    RoleName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkersPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    PositionName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    PositionType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkersPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pieces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PieceName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false),
                    PhotoIds = table.Column<Guid[]>(type: "uuid[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pieces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pieces_PiecesGenres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "PiecesGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TheaterWorkers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    PositionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheaterWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TheaterWorkers_WorkersPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "WorkersPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PieceDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PieceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PieceDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PieceDates_Pieces_PieceId",
                        column: x => x.PieceId,
                        principalTable: "Pieces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PieceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReviews_Pieces_PieceId",
                        column: x => x.PieceId,
                        principalTable: "Pieces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PiecesTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketRow = table.Column<int>(type: "integer", nullable: false),
                    TicketPlace = table.Column<int>(type: "integer", nullable: false),
                    PieceDateId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PiecesTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PiecesTickets_PieceDates_PieceDateId",
                        column: x => x.PieceDateId,
                        principalTable: "PieceDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PieceWorkers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TheaterWorkerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PieceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PieceWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PieceWorkers_PieceDates_PieceId",
                        column: x => x.PieceId,
                        principalTable: "PieceDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PieceWorkers_TheaterWorkers_TheaterWorkerId",
                        column: x => x.TheaterWorkerId,
                        principalTable: "TheaterWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookedTicketsEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PiecesTicketId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedTicketsEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookedTicketsEntity_PiecesTickets_PiecesTicketId",
                        column: x => x.PiecesTicketId,
                        principalTable: "PiecesTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookedTicketsEntity_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketPriceEvents",
                columns: table => new
                {
                    PiecesTicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<PiecesTicketEntity>(type: "jsonb", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPriceEvents", x => new { x.Version, x.PiecesTicketId });
                    table.ForeignKey(
                        name: "FK_TicketPriceEvents_PiecesTickets_PiecesTicketId",
                        column: x => x.PiecesTicketId,
                        principalTable: "PiecesTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedUserTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateOfPurchase = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketPriceEventsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketPriceEventsVersion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedUserTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchasedUserTickets_TicketPriceEvents_TicketPriceEventsVer~",
                        columns: x => new { x.TicketPriceEventsVersion, x.TicketPriceEventsId },
                        principalTable: "TicketPriceEvents",
                        principalColumns: new[] { "Version", "PiecesTicketId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchasedUserTickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "DateOfCreate", "Email", "FirstName", "Gender", "LastName", "MiddleName", "Money", "Password", "Phone", "PhotoId", "RoleId", "UserName" },
                values: new object[,]
                {
                    { new Guid("e1f83d38-56a7-435b-94bd-fe891ed0f03a"), new DateTime(2001, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "icestormyy-user@mail.ru", "Mikhail", 1, "Tolmachev", "Evgenievich", 1000m, "E10ADC3949BA59ABBE56E057F20F883E", "81094316687", null, 1, "IceStormy-user" },
                    { new Guid("f2343d16-e610-4a73-a0f0-b9f63df511e6"), new DateTime(2001, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "icestormyy-admin@mail.ru", "Mikhail", 1, "Tolmachev", "Evgenievich", 1000m, "E10ADC3949BA59ABBE56E057F20F883E", "81094316687", null, 2, "IceStormy-admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_BookedTicketsEntity_PiecesTicketId",
                table: "BookedTicketsEntity",
                column: "PiecesTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_BookedTicketsEntity_UserId",
                table: "BookedTicketsEntity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PieceDates_PieceId",
                table: "PieceDates",
                column: "PieceId");

            migrationBuilder.CreateIndex(
                name: "IX_Pieces_GenreId",
                table: "Pieces",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_PiecesTickets_PieceDateId",
                table: "PiecesTickets",
                column: "PieceDateId");

            migrationBuilder.CreateIndex(
                name: "IX_PieceWorkers_PieceId",
                table: "PieceWorkers",
                column: "PieceId");

            migrationBuilder.CreateIndex(
                name: "IX_PieceWorkers_TheaterWorkerId",
                table: "PieceWorkers",
                column: "TheaterWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedUserTickets_TicketPriceEventsVersion_TicketPriceEv~",
                table: "PurchasedUserTickets",
                columns: new[] { "TicketPriceEventsVersion", "TicketPriceEventsId" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedUserTickets_UserId",
                table: "PurchasedUserTickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TheaterWorkers_PositionId",
                table: "TheaterWorkers",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPriceEvents_PiecesTicketId",
                table: "TicketPriceEvents",
                column: "PiecesTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_PieceId",
                table: "UserReviews",
                column: "PieceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_UserId",
                table: "UserReviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRoles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRoles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "BookedTicketsEntity");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "PieceWorkers");

            migrationBuilder.DropTable(
                name: "PurchasedUserTickets");

            migrationBuilder.DropTable(
                name: "UserReviews");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "TheaterWorkers");

            migrationBuilder.DropTable(
                name: "TicketPriceEvents");

            migrationBuilder.DropTable(
                name: "WorkersPositions");

            migrationBuilder.DropTable(
                name: "PiecesTickets");

            migrationBuilder.DropTable(
                name: "PieceDates");

            migrationBuilder.DropTable(
                name: "Pieces");

            migrationBuilder.DropTable(
                name: "PiecesGenres");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e1f83d38-56a7-435b-94bd-fe891ed0f03a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f2343d16-e610-4a73-a0f0-b9f63df511e6"));

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Users");
        }
    }
}
