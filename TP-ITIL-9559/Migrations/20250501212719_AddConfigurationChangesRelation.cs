using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_ITIL_9559.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationChangesRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfigurationItemId",
                table: "Change",
                type: "integer",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_Change_ConfigurationItemId",
                table: "Change",
                column: "ConfigurationItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Change_ConfigurationItem_ConfigurationItemId",
                table: "Change",
                column: "ConfigurationItemId",
                principalTable: "ConfigurationItem",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Change_ConfigurationItem_ConfigurationItemId",
                table: "Change");

            migrationBuilder.DropIndex(
                name: "IX_Change_ConfigurationItemId",
                table: "Change");

            migrationBuilder.DropColumn(
                name: "ConfigurationItemId",
                table: "Change");
        }
    }
}
