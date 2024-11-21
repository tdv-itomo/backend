using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VicemAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn3Score : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterScore",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Student",
                newName: "ThirdScore");

            migrationBuilder.AddColumn<float>(
                name: "FirstScore",
                table: "Student",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "SecondScore",
                table: "Student",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstScore",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "SecondScore",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "ThirdScore",
                table: "Student",
                newName: "Score");

            migrationBuilder.AddColumn<string>(
                name: "CharacterScore",
                table: "Student",
                type: "TEXT",
                nullable: true);
        }
    }
}
