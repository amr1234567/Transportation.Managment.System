using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NodeHierarchy",
                schema: "App",
                table: "NodeHierarchy");

            migrationBuilder.RenameTable(
                name: "NodeHierarchy",
                schema: "App",
                newName: "BusStopsRelations",
                newSchema: "App");

            migrationBuilder.RenameIndex(
                name: "IX_NodeHierarchy_DestinationStopId",
                schema: "App",
                table: "BusStopsRelations",
                newName: "IX_BusStopsRelations_DestinationStopId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusStopsRelations",
                schema: "App",
                table: "BusStopsRelations",
                columns: new[] { "StartBusStopId", "DestinationStopId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BusStopsRelations",
                schema: "App",
                table: "BusStopsRelations");

            migrationBuilder.RenameTable(
                name: "BusStopsRelations",
                schema: "App",
                newName: "NodeHierarchy",
                newSchema: "App");

            migrationBuilder.RenameIndex(
                name: "IX_BusStopsRelations_DestinationStopId",
                schema: "App",
                table: "NodeHierarchy",
                newName: "IX_NodeHierarchy_DestinationStopId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NodeHierarchy",
                schema: "App",
                table: "NodeHierarchy",
                columns: new[] { "StartBusStopId", "DestinationStopId" });
        }
    }
}
