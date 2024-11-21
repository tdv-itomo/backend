using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VicemAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "Student",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CouncilID",
                table: "Student",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Student",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TopicID",
                table: "Student",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CouncilID",
                table: "Advisor",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Council",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DepartmentID = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Council", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Council_Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Department",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DepartmentID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Topic_Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Department",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_CouncilID",
                table: "Student",
                column: "CouncilID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_TopicID",
                table: "Student",
                column: "TopicID");

            migrationBuilder.CreateIndex(
                name: "IX_Advisor_CouncilID",
                table: "Advisor",
                column: "CouncilID");

            migrationBuilder.CreateIndex(
                name: "IX_Council_DepartmentID",
                table: "Council",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_DepartmentID",
                table: "Topic",
                column: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advisor_Council_CouncilID",
                table: "Advisor",
                column: "CouncilID",
                principalTable: "Council",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Council_CouncilID",
                table: "Student",
                column: "CouncilID",
                principalTable: "Council",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Topic_TopicID",
                table: "Student",
                column: "TopicID",
                principalTable: "Topic",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advisor_Council_CouncilID",
                table: "Advisor");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Council_CouncilID",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Topic_TopicID",
                table: "Student");

            migrationBuilder.DropTable(
                name: "Council");

            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Student_CouncilID",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_TopicID",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Advisor_CouncilID",
                table: "Advisor");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "CouncilID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "TopicID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "CouncilID",
                table: "Advisor");
        }
    }
}
