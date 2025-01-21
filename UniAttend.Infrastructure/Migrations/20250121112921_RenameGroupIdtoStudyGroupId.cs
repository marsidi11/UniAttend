using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniAttend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameGroupIdtoStudyGroupId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbsenceAlerts_StudyGroups_GroupId",
                table: "AbsenceAlerts");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSessions_StudyGroups_GroupId",
                table: "CourseSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_StudyGroups_GroupId",
                table: "GroupStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_StudyGroups_GroupId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_StudyGroups_StudyGroupId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_GroupId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "GroupStudents",
                newName: "StudyGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupStudents_GroupId_StudentId",
                table: "GroupStudents",
                newName: "IX_GroupStudents_StudyGroupId_StudentId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "CourseSessions",
                newName: "StudyGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSessions_GroupId",
                table: "CourseSessions",
                newName: "IX_CourseSessions_StudyGroupId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "AbsenceAlerts",
                newName: "StudyGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_AbsenceAlerts_StudentId_GroupId",
                table: "AbsenceAlerts",
                newName: "IX_AbsenceAlerts_StudentId_StudyGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_AbsenceAlerts_GroupId",
                table: "AbsenceAlerts",
                newName: "IX_AbsenceAlerts_StudyGroupId");

            migrationBuilder.AlterColumn<int>(
                name: "StudyGroupId",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AbsenceAlerts_StudyGroups_StudyGroupId",
                table: "AbsenceAlerts",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSessions_StudyGroups_StudyGroupId",
                table: "CourseSessions",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_StudyGroups_StudyGroupId",
                table: "GroupStudents",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_StudyGroups_StudyGroupId",
                table: "Schedules",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbsenceAlerts_StudyGroups_StudyGroupId",
                table: "AbsenceAlerts");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSessions_StudyGroups_StudyGroupId",
                table: "CourseSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_StudyGroups_StudyGroupId",
                table: "GroupStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_StudyGroups_StudyGroupId",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "StudyGroupId",
                table: "GroupStudents",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupStudents_StudyGroupId_StudentId",
                table: "GroupStudents",
                newName: "IX_GroupStudents_GroupId_StudentId");

            migrationBuilder.RenameColumn(
                name: "StudyGroupId",
                table: "CourseSessions",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSessions_StudyGroupId",
                table: "CourseSessions",
                newName: "IX_CourseSessions_GroupId");

            migrationBuilder.RenameColumn(
                name: "StudyGroupId",
                table: "AbsenceAlerts",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_AbsenceAlerts_StudyGroupId",
                table: "AbsenceAlerts",
                newName: "IX_AbsenceAlerts_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_AbsenceAlerts_StudentId_StudyGroupId",
                table: "AbsenceAlerts",
                newName: "IX_AbsenceAlerts_StudentId_GroupId");

            migrationBuilder.AlterColumn<int>(
                name: "StudyGroupId",
                table: "Schedules",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_GroupId",
                table: "Schedules",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbsenceAlerts_StudyGroups_GroupId",
                table: "AbsenceAlerts",
                column: "GroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSessions_StudyGroups_GroupId",
                table: "CourseSessions",
                column: "GroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_StudyGroups_GroupId",
                table: "GroupStudents",
                column: "GroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_StudyGroups_GroupId",
                table: "Schedules",
                column: "GroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_StudyGroups_StudyGroupId",
                table: "Schedules",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id");
        }
    }
}
