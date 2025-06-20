using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_ITIL_9559.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConfigFromProblems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfigurationItemId",
                table: "Problem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfigurationItemId",
                table: "Problem",
                type: "integer",
                nullable: true,
                defaultValue: null);
        }
    }
}
