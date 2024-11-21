using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VicemAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateTopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Student_StudentID",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Topic_StudentID",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "StudentID",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "ApproveStatus",
                table: "Student");

            migrationBuilder.AddColumn<int>(
                name: "TopicID",
                table: "Student",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_TopicID",
                table: "Student",
                column: "TopicID",
                unique: true);

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
                name: "FK_Student_Topic_TopicID",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_TopicID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "TopicID",
                table: "Student");

            migrationBuilder.AddColumn<int>(
                name: "StudentID",
                table: "Topic",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApproveStatus",
                table: "Student",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topic_StudentID",
                table: "Topic",
                column: "StudentID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Student_StudentID",
                table: "Topic",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
