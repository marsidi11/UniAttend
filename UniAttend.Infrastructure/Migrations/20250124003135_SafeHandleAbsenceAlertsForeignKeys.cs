using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniAttend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SafeHandleAbsenceAlertsForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtpCodes");

            migrationBuilder.AddColumn<string>(
                name: "TotpSecret",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "StudyGroupId",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseSessionId",
                table: "AttendanceRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "AttendanceRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_ClassId",
                table: "AttendanceRecords",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_CourseSessions_ClassId",
                table: "AttendanceRecords",
                column: "ClassId",
                principalTable: "CourseSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_CourseSessions_ClassId",
                table: "AttendanceRecords");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRecords_ClassId",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "TotpSecret",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "AttendanceRecords");

            migrationBuilder.AlterColumn<int>(
                name: "StudyGroupId",
                table: "Schedules",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CourseSessionId",
                table: "AttendanceRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "OtpCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ExpiryTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsUsed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpCodes", x => x.Id);
                    table.CheckConstraint("CK_OtpCode_Format", "LENGTH(Code) = 6 AND Code REGEXP '^[0-9]+$'");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OtpCodes_ClassId_ExpiryTime",
                table: "OtpCodes",
                columns: new[] { "ClassId", "ExpiryTime" });

            migrationBuilder.CreateIndex(
                name: "IX_OtpCodes_Code_ClassId_StudentId",
                table: "OtpCodes",
                columns: new[] { "Code", "ClassId", "StudentId" },
                unique: true);
        }
    }
}
