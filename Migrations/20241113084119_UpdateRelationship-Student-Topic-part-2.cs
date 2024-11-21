using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VicemAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipStudentTopicpart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Department_DepartmentID",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Topic_DepartmentID",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Topic");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                table: "Topic",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Topic",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Topic_DepartmentID",
                table: "Topic",
                column: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Department_DepartmentID",
                table: "Topic",
                column: "DepartmentID",
                principalTable: "Department",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
