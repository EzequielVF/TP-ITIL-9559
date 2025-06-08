using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TP_ITIL_9559.Migrations
{
    /// <inheritdoc />
    public partial class disableColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
