using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class editNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Journeys_JourneyId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TimeTables_TimeTableId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "TimeTables",
                schema: "Security");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_JourneyId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TimeTableId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.AddColumn<Guid>(
                name: "JourneyHistoryId",
                schema: "App",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpcomingJourneyId",
                schema: "App",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UpcomingJourneys",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketPrice = table.Column<double>(type: "float", nullable: false),
                    DestinationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartBusStopId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LeavingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JourneyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumberOfAvailableTickets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpcomingJourneys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpcomingJourneys_Buses_BusId",
                        column: x => x.BusId,
                        principalSchema: "App",
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UpcomingJourneys_Managers_DestinationId",
                        column: x => x.DestinationId,
                        principalSchema: "App",
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UpcomingJourneys_Managers_StartBusStopId",
                        column: x => x.StartBusStopId,
                        principalSchema: "App",
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_JourneyHistoryId",
                schema: "App",
                table: "Tickets",
                column: "JourneyHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UpcomingJourneyId",
                schema: "App",
                table: "Tickets",
                column: "UpcomingJourneyId");

            migrationBuilder.CreateIndex(
                name: "IX_UpcomingJourneys_BusId",
                schema: "App",
                table: "UpcomingJourneys",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_UpcomingJourneys_DestinationId",
                schema: "App",
                table: "UpcomingJourneys",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_UpcomingJourneys_StartBusStopId",
                schema: "App",
                table: "UpcomingJourneys",
                column: "StartBusStopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Journeys_JourneyHistoryId",
                schema: "App",
                table: "Tickets",
                column: "JourneyHistoryId",
                principalSchema: "App",
                principalTable: "Journeys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_UpcomingJourneys_UpcomingJourneyId",
                schema: "App",
                table: "Tickets",
                column: "UpcomingJourneyId",
                principalSchema: "App",
                principalTable: "UpcomingJourneys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Journeys_JourneyHistoryId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_UpcomingJourneys_UpcomingJourneyId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "UpcomingJourneys",
                schema: "App");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_JourneyHistoryId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_UpcomingJourneyId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "JourneyHistoryId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UpcomingJourneyId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.CreateTable(
                name: "TimeTables",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartBusStopId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JourneyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeavingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfAvailableTickets = table.Column<int>(type: "int", nullable: false),
                    TicketPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeTables_Buses_BusId",
                        column: x => x.BusId,
                        principalSchema: "App",
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeTables_Managers_DestinationId",
                        column: x => x.DestinationId,
                        principalSchema: "App",
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeTables_Managers_StartBusStopId",
                        column: x => x.StartBusStopId,
                        principalSchema: "App",
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_JourneyId",
                schema: "App",
                table: "Tickets",
                column: "JourneyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TimeTableId",
                schema: "App",
                table: "Tickets",
                column: "TimeTableId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_BusId",
                schema: "Security",
                table: "TimeTables",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_DestinationId",
                schema: "Security",
                table: "TimeTables",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_StartBusStopId",
                schema: "Security",
                table: "TimeTables",
                column: "StartBusStopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Journeys_JourneyId",
                schema: "App",
                table: "Tickets",
                column: "JourneyId",
                principalSchema: "App",
                principalTable: "Journeys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TimeTables_TimeTableId",
                schema: "App",
                table: "Tickets",
                column: "TimeTableId",
                principalSchema: "Security",
                principalTable: "TimeTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
