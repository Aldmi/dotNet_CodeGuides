using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EfCoreAdvanced.Migrations
{
    /// <inheritdoc />
    public partial class SeedCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "courseId", "Name" },
                values: new object[,]
                {
                    { 1L, "Math" },
                    { 2L, "Chemistry" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "courseId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "courseId",
                keyValue: 2L);
        }
    }
}
