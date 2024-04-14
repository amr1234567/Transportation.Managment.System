using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class editDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journeys_Users_ApplicationUserId",
                schema: "App",
                table: "Journeys");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_UpcomingJourneys_UpcomingJourneyId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_ApplicationUserId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ApplicationUserId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Journeys_ApplicationUserId",
                schema: "App",
                table: "Journeys");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                schema: "App",
                table: "Tickets");

            //migrationBuilder.DropColumn(
            //    name: "TimeTableId",
            //    schema: "App",
            //    table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                schema: "App",
                table: "Journeys");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpcomingJourneyId",
                schema: "App",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_UpcomingJourneys_UpcomingJourneyId",
                schema: "App",
                table: "Tickets",
                column: "UpcomingJourneyId",
                principalSchema: "App",
                principalTable: "UpcomingJourneys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_UpcomingJourneys_UpcomingJourneyId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpcomingJourneyId",
                schema: "App",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                schema: "App",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true);

            //migrationBuilder.AddColumn<Guid>(
            //    name: "TimeTableId",
            //    schema: "App",
            //    table: "Tickets",
            //    type: "uniqueidentifier",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                schema: "App",
                table: "Journeys",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ApplicationUserId",
                schema: "App",
                table: "Tickets",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Journeys_ApplicationUserId",
                schema: "App",
                table: "Journeys",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Journeys_Users_ApplicationUserId",
                schema: "App",
                table: "Journeys",
                column: "ApplicationUserId",
                principalSchema: "Security",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_UpcomingJourneys_UpcomingJourneyId",
                schema: "App",
                table: "Tickets",
                column: "UpcomingJourneyId",
                principalSchema: "App",
                principalTable: "UpcomingJourneys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_ApplicationUserId",
                schema: "App",
                table: "Tickets",
                column: "ApplicationUserId",
                principalSchema: "Security",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
