using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class editTicket : Migration
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

            migrationBuilder.AlterColumn<Guid>(
                name: "TimeTableId",
                schema: "App",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "JourneyId",
                schema: "App",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Journeys_JourneyId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TimeTables_TimeTableId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.AlterColumn<Guid>(
                name: "TimeTableId",
                schema: "App",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "JourneyId",
                schema: "App",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Journeys_JourneyId",
                schema: "App",
                table: "Tickets",
                column: "JourneyId",
                principalSchema: "App",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TimeTables_TimeTableId",
                schema: "App",
                table: "Tickets",
                column: "TimeTableId",
                principalSchema: "Security",
                principalTable: "TimeTables",
                principalColumn: "Id");
        }
    }
}
