using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VicemAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateColumnScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CharacterScore",
                table: "Student",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Score",
                table: "Student",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterScore",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Student");
        }
    }
}
