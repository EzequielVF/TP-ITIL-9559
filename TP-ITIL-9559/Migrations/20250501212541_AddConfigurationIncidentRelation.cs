using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_ITIL_9559.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationIncidentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfigurationItemId",
                table: "Incident",
                type: "integer",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_Incident_ConfigurationItemId",
                table: "Incident",
                column: "ConfigurationItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_ConfigurationItem_ConfigurationItemId",
                table: "Incident",
                column: "ConfigurationItemId",
                principalTable: "ConfigurationItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_ConfigurationItem_ConfigurationItemId",
                table: "Incident");

            migrationBuilder.DropIndex(
                name: "IX_Incident_ConfigurationItemId",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "ConfigurationItemId",
                table: "Incident");
        }
    }
}
