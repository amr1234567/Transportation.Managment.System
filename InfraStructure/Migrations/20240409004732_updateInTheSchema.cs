using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class updateInTheSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SeaNum",
                schema: "App",
                table: "Tickets",
                newName: "SeatNum");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalTime",
                schema: "App",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DestinationId",
                schema: "App",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DestinationName",
                schema: "App",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LeavingTime",
                schema: "App",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StartBusStopId",
                schema: "App",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StartBusStopName",
                schema: "App",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TicketPrice",
                schema: "App",
                table: "Journeys",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalTime",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DestinationName",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "LeavingTime",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "StartBusStopId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "StartBusStopName",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketPrice",
                schema: "App",
                table: "Journeys");

            migrationBuilder.RenameColumn(
                name: "SeatNum",
                schema: "App",
                table: "Tickets",
                newName: "SeaNum");
        }
    }
}
