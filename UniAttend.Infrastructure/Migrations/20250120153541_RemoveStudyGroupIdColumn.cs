using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniAttend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStudyGroupIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_StudyGroups_StudyGroupId",
                table: "GroupStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_StudyGroupId",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "StudyGroupId",
                table: "GroupStudents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudyGroupId",
                table: "GroupStudents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_StudyGroupId",
                table: "GroupStudents",
                column: "StudyGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_StudyGroups_StudyGroupId",
                table: "GroupStudents",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id");
        }
    }
}
