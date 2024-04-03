using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class EditBusStopDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Managers_BusStops_BusStopId",
                schema: "Security",
                table: "Managers");

            migrationBuilder.AlterColumn<Guid>(
                name: "BusStopId",
                schema: "Security",
                table: "Managers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "managerId",
                schema: "App",
                table: "BusStops",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BusStops_managerId",
                schema: "App",
                table: "BusStops",
                column: "managerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BusStops_Managers_managerId",
                schema: "App",
                table: "BusStops",
                column: "managerId",
                principalSchema: "Security",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_BusStops_BusStopId",
                schema: "Security",
                table: "Managers",
                column: "BusStopId",
                principalSchema: "App",
                principalTable: "BusStops",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusStops_Managers_managerId",
                schema: "App",
                table: "BusStops");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_BusStops_BusStopId",
                schema: "Security",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_BusStops_managerId",
                schema: "App",
                table: "BusStops");

            migrationBuilder.DropColumn(
                name: "managerId",
                schema: "App",
                table: "BusStops");

            migrationBuilder.AlterColumn<Guid>(
                name: "BusStopId",
                schema: "Security",
                table: "Managers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_BusStops_BusStopId",
                schema: "Security",
                table: "Managers",
                column: "BusStopId",
                principalSchema: "App",
                principalTable: "BusStops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
