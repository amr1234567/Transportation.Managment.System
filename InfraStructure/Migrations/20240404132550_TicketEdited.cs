using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class TicketEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Buses_BusId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_BusId",
                schema: "App",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "BusId",
                schema: "App",
                table: "Tickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusId",
                schema: "App",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_BusId",
                schema: "App",
                table: "Tickets",
                column: "BusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Buses_BusId",
                schema: "App",
                table: "Tickets",
                column: "BusId",
                principalSchema: "App",
                principalTable: "Buses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
