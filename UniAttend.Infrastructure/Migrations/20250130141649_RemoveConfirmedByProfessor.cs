using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniAttend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConfirmedByProfessor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_Professors_ConfirmedByProfessorId",
                table: "AttendanceRecords");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRecords_ConfirmedByProfessorId",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "ConfirmedByProfessorId",
                table: "AttendanceRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfirmedByProfessorId",
                table: "AttendanceRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_ConfirmedByProfessorId",
                table: "AttendanceRecords",
                column: "ConfirmedByProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_Professors_ConfirmedByProfessorId",
                table: "AttendanceRecords",
                column: "ConfirmedByProfessorId",
                principalTable: "Professors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
