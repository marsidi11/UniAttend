using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniAttend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveReaderDeviceIdUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Classrooms_ReaderDeviceId",
                table: "Classrooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_ReaderDeviceId",
                table: "Classrooms",
                column: "ReaderDeviceId",
                unique: true,
                filter: "[ReaderDeviceId] IS NOT NULL");
        }
    }
}
