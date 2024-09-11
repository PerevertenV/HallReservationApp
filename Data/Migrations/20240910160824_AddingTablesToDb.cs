using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingTablesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Halls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    AdditionalOptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePerHour = table.Column<int>(type: "int", nullable: false),
                    reserved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hallId = table.Column<int>(type: "int", nullable: false),
                    dateTimeOfReserv = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reservTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    SelectedAddOpt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Halls_hallId",
                        column: x => x.hallId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Halls",
                columns: new[] { "Id", "AdditionalOptions", "Capacity", "Name", "PricePerHour", "reserved" },
                values: new object[] { 1, "projector/wifi/sound", 50, "Hall A", 2000, false });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_hallId",
                table: "Reservations",
                column: "hallId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Halls");
        }
    }
}
