using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Managers_BusStopId",
                schema: "App",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_BusStopId",
                schema: "App",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "BusStopId",
                schema: "App",
                table: "Managers");

            migrationBuilder.CreateTable(
                name: "NodeHierarchy",
                schema: "App",
                columns: table => new
                {
                    StartBusStopId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DestinationStopId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeHierarchy", x => new { x.StartBusStopId, x.DestinationStopId });
                    table.ForeignKey(
                        name: "FK_BusStopsRelations_DestinationStop",
                        column: x => x.DestinationStopId,
                        principalSchema: "App",
                        principalTable: "Managers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusStopsRelations_StartBusStop",
                        column: x => x.StartBusStopId,
                        principalSchema: "App",
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NodeHierarchy_DestinationStopId",
                schema: "App",
                table: "NodeHierarchy",
                column: "DestinationStopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NodeHierarchy",
                schema: "App");

            migrationBuilder.AddColumn<string>(
                name: "BusStopId",
                schema: "App",
                table: "Managers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_BusStopId",
                schema: "App",
                table: "Managers",
                column: "BusStopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Managers_BusStopId",
                schema: "App",
                table: "Managers",
                column: "BusStopId",
                principalSchema: "App",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
