using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VicemAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationShipAdvisorCouncil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advisor_Council_CouncilID",
                table: "Advisor");

            migrationBuilder.DropIndex(
                name: "IX_Advisor_CouncilID",
                table: "Advisor");

            migrationBuilder.DropColumn(
                name: "CouncilID",
                table: "Advisor");

            migrationBuilder.CreateTable(
                name: "AdvisorCouncil",
                columns: table => new
                {
                    AdvisorsID = table.Column<int>(type: "INTEGER", nullable: false),
                    CouncilsID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvisorCouncil", x => new { x.AdvisorsID, x.CouncilsID });
                    table.ForeignKey(
                        name: "FK_AdvisorCouncil_Advisor_AdvisorsID",
                        column: x => x.AdvisorsID,
                        principalTable: "Advisor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvisorCouncil_Council_CouncilsID",
                        column: x => x.CouncilsID,
                        principalTable: "Council",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvisorCouncil_CouncilsID",
                table: "AdvisorCouncil",
                column: "CouncilsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvisorCouncil");

            migrationBuilder.AddColumn<int>(
                name: "CouncilID",
                table: "Advisor",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advisor_CouncilID",
                table: "Advisor",
                column: "CouncilID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advisor_Council_CouncilID",
                table: "Advisor",
                column: "CouncilID",
                principalTable: "Council",
                principalColumn: "ID");
        }
    }
}
