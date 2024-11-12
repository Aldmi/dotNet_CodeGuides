using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Pg.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    City = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persones", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Persones",
                columns: new[] { "Id", "Age", "City", "Country", "Name" },
                values: new object[,]
                {
                    { new Guid("dffee042-a984-4ebd-951d-f4ed6c2de973"), 58, "Washington", "USA", "Jone" },
                    { new Guid("f1c17088-cc68-4a86-8ede-a5b27a13e3c3"), 18, "Moscow", "Russia", "Alex" },
                    { new Guid("f51df220-6eed-440f-b40e-1bf0aea39149"), 28, "Bermangham", "England", "Peter" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persones");
        }
    }
}
