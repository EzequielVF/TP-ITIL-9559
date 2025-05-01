using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_ITIL_9559.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationProblemRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfigurationItemId",
                table: "Problem",
                type: "integer",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_Problem_ConfigurationItemId",
                table: "Problem",
                column: "ConfigurationItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problem_ConfigurationItem_ConfigurationItemId",
                table: "Problem",
                column: "ConfigurationItemId",
                principalTable: "ConfigurationItem",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_Problem_ConfigurationItem_ConfigurationItemId",
                table: "Problem");

            migrationBuilder.DropIndex(
                name: "IX_Problem_ConfigurationItemId",
                table: "Problem");

            migrationBuilder.DropColumn(
                name: "ConfigurationItemId",
                table: "Problem");
        }
    }
}
