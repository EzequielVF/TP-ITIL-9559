using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_ITIL_9559.Migrations
{
    /// <inheritdoc />
    public partial class AddEmptyField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                table: "Incident",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Incident",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RootCause",
                table: "Incident",
                type: "text",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TrackingNumber",
                table: "Incident",
                type: "serial",
                nullable: false);


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

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Incident",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Change",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropForeignKey(
                name: "FK_Change_ConfigurationItem_ConfigurationItemId",
                table: "Change");

            migrationBuilder.DropForeignKey(
                name: "FK_Incident_ConfigurationItem_ConfigurationItemId",
                table: "Incident");

            migrationBuilder.DropForeignKey(
                name: "FK_Problem_ConfigurationItem_ConfigurationItemId",
                table: "Problem");

            migrationBuilder.AlterColumn<int>(
                name: "ConfigurationItemId",
                table: "Problem",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedUserId",
                table: "Problem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ConfigurationItemId",
                table: "Incident",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedUserId",
                table: "Incident",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ConfigurationItemId",
                table: "Change",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedUserId",
                table: "Change",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Problem_AssignedUserId",
                table: "Problem",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_AssignedUserId",
                table: "Incident",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Change_AssignedUserId",
                table: "Change",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Change_ConfigurationItem_ConfigurationItemId",
                table: "Change",
                column: "ConfigurationItemId",
                principalTable: "ConfigurationItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Change_User_AssignedUserId",
                table: "Change",
                column: "AssignedUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_ConfigurationItem_ConfigurationItemId",
                table: "Incident",
                column: "ConfigurationItemId",
                principalTable: "ConfigurationItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_User_AssignedUserId",
                table: "Incident",
                column: "AssignedUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Problem_ConfigurationItem_ConfigurationItemId",
                table: "Problem",
                column: "ConfigurationItemId",
                principalTable: "ConfigurationItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Problem_User_AssignedUserId",
                table: "Problem",
                column: "AssignedUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddColumn<string>(
                name: "Impact",
                table: "Problem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Problem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Impact",
                table: "Incident",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Incident",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Impact",
                table: "Change",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Change",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "Comments",
                table: "Problem",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "Comments",
                table: "Incident",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "Comments",
                table: "Change",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledDate",
                table: "Change",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VersionId",
                table: "ConfigurationItem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VersionHistory",
                table: "ConfigurationItem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "RootCause",
                table: "Incident",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "ChangeIncident (Dictionary<string, object>)",
                columns: table => new
                {
                    ChangesId = table.Column<int>(type: "integer", nullable: false),
                    IncidentsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeIncident (Dictionary<string, object>)", x => new { x.ChangesId, x.IncidentsId });
                    table.ForeignKey(
                        name: "FK_ChangeIncident (Dictionary<string, object>)_Change_ChangesId",
                        column: x => x.ChangesId,
                        principalTable: "Change",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeIncident (Dictionary<string, object>)_Incident_Incide~",
                        column: x => x.IncidentsId,
                        principalTable: "Incident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChangeProblem (Dictionary<string, object>)",
                columns: table => new
                {
                    ChangesId = table.Column<int>(type: "integer", nullable: false),
                    ProblemsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeProblem (Dictionary<string, object>)", x => new { x.ChangesId, x.ProblemsId });
                    table.ForeignKey(
                        name: "FK_ChangeProblem (Dictionary<string, object>)_Change_ChangesId",
                        column: x => x.ChangesId,
                        principalTable: "Change",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeProblem (Dictionary<string, object>)_Problem_Problems~",
                        column: x => x.ProblemsId,
                        principalTable: "Problem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncidentProblem (Dictionary<string, object>)",
                columns: table => new
                {
                    IncidentsId = table.Column<int>(type: "integer", nullable: false),
                    ProblemsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentProblem (Dictionary<string, object>)", x => new { x.IncidentsId, x.ProblemsId });
                    table.ForeignKey(
                        name: "FK_IncidentProblem (Dictionary<string, object>)_Incident_Incid~",
                        column: x => x.IncidentsId,
                        principalTable: "Incident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentProblem (Dictionary<string, object>)_Problem_Proble~",
                        column: x => x.ProblemsId,
                        principalTable: "Problem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeIncident (Dictionary<string, object>)_IncidentsId",
                table: "ChangeIncident (Dictionary<string, object>)",
                column: "IncidentsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeProblem (Dictionary<string, object>)_ProblemsId",
                table: "ChangeProblem (Dictionary<string, object>)",
                column: "ProblemsId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentProblem (Dictionary<string, object>)_ProblemsId",
                table: "IncidentProblem (Dictionary<string, object>)",
                column: "ProblemsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedDate",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "RootCause",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "ClientEmail",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "ClientEmail",
                table: "Change");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Change");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Change");

            migrationBuilder.DropForeignKey(
                name: "FK_Change_ConfigurationItem_ConfigurationItemId",
                table: "Change");

            migrationBuilder.DropForeignKey(
                name: "FK_Change_User_AssignedUserId",
                table: "Change");

            migrationBuilder.DropForeignKey(
                name: "FK_Incident_ConfigurationItem_ConfigurationItemId",
                table: "Incident");

            migrationBuilder.DropForeignKey(
                name: "FK_Incident_User_AssignedUserId",
                table: "Incident");

            migrationBuilder.DropForeignKey(
                name: "FK_Problem_ConfigurationItem_ConfigurationItemId",
                table: "Problem");

            migrationBuilder.DropForeignKey(
                name: "FK_Problem_User_AssignedUserId",
                table: "Problem");

            migrationBuilder.DropIndex(
                name: "IX_Problem_AssignedUserId",
                table: "Problem");

            migrationBuilder.DropIndex(
                name: "IX_Incident_AssignedUserId",
                table: "Incident");

            migrationBuilder.DropIndex(
                name: "IX_Change_AssignedUserId",
                table: "Change");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Problem");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Change");

            migrationBuilder.AlterColumn<int>(
                name: "ConfigurationItemId",
                table: "Problem",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ConfigurationItemId",
                table: "Incident",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ConfigurationItemId",
                table: "Change",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Change_ConfigurationItem_ConfigurationItemId",
                table: "Change",
                column: "ConfigurationItemId",
                principalTable: "ConfigurationItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_ConfigurationItem_ConfigurationItemId",
                table: "Incident",
                column: "ConfigurationItemId",
                principalTable: "ConfigurationItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Problem_ConfigurationItem_ConfigurationItemId",
                table: "Problem",
                column: "ConfigurationItemId",
                principalTable: "ConfigurationItem",
                principalColumn: "Id");

            migrationBuilder.DropColumn(
                name: "Impact",
                table: "Problem");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Problem");

            migrationBuilder.DropColumn(
                name: "Impact",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "Impact",
                table: "Change");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Change");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Problem");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Change");

            migrationBuilder.DropColumn(
                name: "ScheduledDate",
                table: "Change");

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "ConfigurationItem");

            migrationBuilder.DropColumn(
                name: "VersionHistory",
                table: "ConfigurationItem");

            migrationBuilder.AlterColumn<string>(
                name: "RootCause",
                table: "Incident",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.DropTable(
                name: "ChangeIncident (Dictionary<string, object>)");

            migrationBuilder.DropTable(
                name: "ChangeProblem (Dictionary<string, object>)");

            migrationBuilder.DropTable(
                name: "IncidentProblem (Dictionary<string, object>)");
        }
    }
}
