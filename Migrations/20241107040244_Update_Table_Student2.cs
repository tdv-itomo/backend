using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VicemAPI.Migrations
{
    /// <inheritdoc />
    public partial class Update_Table_Student2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Advisor_AdvisorID",
                table: "Student");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Student",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "AdvisorID",
                table: "Student",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Advisor_AdvisorID",
                table: "Student",
                column: "AdvisorID",
                principalTable: "Advisor",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Advisor_AdvisorID",
                table: "Student");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Student",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdvisorID",
                table: "Student",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Advisor_AdvisorID",
                table: "Student",
                column: "AdvisorID",
                principalTable: "Advisor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
