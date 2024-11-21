using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VicemAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableAdvisorssds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "maxSlot",
                table: "Advisor",
                newName: "MaxSlot");

            migrationBuilder.RenameColumn(
                name: "currentSlot",
                table: "Advisor",
                newName: "CurrentSlot");

            migrationBuilder.AlterColumn<int>(
                name: "MaxSlot",
                table: "Advisor",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxSlot",
                table: "Advisor",
                newName: "maxSlot");

            migrationBuilder.RenameColumn(
                name: "CurrentSlot",
                table: "Advisor",
                newName: "currentSlot");

            migrationBuilder.AlterColumn<int>(
                name: "maxSlot",
                table: "Advisor",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
