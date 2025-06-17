using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_ITIL_9559.Migrations
{
    /// <inheritdoc />
    public partial class ClientInfoTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientEmail",
                table: "Change");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Change");

            migrationBuilder.DropColumn(
                name: "ClientEmail",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Incident");

            migrationBuilder.AddColumn<string>(
                name: "ClientEmail",
                table: "ConfigurationItem",
                type: "text",
                nullable: false,
                defaultValue: "Peter");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "ConfigurationItem",
                type: "text",
                nullable: false,
                defaultValue: "Peter@fiuba.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientEmail",
                table: "ConfigurationItem");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "ConfigurationItem");

            migrationBuilder.AddColumn<string>(
                name: "ClientEmail",
                table: "Incident",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Incident",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientEmail",
                table: "Change",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Change",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
