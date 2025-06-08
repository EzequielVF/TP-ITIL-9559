using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_ITIL_9559.Migrations
{
    /// <inheritdoc />
    public partial class renameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "disabled",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "disabled",
                table: "Change");

            migrationBuilder.DropColumn(
                name: "disabled",
                table: "Problem");

            migrationBuilder.DropColumn(
                name: "disabled",
                table: "ConfigurationItem");

            migrationBuilder.AddColumn<bool>(
                name: "Disabled",
                table: "Problem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Disabled",
                table: "Incident",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Disabled",
                table: "ConfigurationItem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Disabled",
                table: "Change",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disabled",
                table: "Problem");

            migrationBuilder.DropColumn(
                name: "Disabled",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "Disabled",
                table: "ConfigurationItem");

            migrationBuilder.DropColumn(
                name: "Disabled",
                table: "Change");

            migrationBuilder.AddColumn<bool>(
                name: "disabled",
                table: "Incident",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "disabled",
                table: "Change",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "disabled",
                table: "Problem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "disabled",
                table: "ConfigurationItem",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
