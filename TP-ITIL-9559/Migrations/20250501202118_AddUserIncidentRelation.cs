using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_ITIL_9559.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIncidentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Incident",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Incident_UserId",
                table: "Incident",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_User_UserId",
                table: "Incident",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_User_UserId",
                table: "Incident");

            migrationBuilder.DropIndex(
                name: "IX_Incident_UserId",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Incident");
        }
    }
}
