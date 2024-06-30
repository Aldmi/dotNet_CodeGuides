using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastucture.DataAccess.Pg.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Cost", "Count", "CreateTime", "Name", "UpdateTime" },
                values: new object[,]
                {
                    { new Guid("1cd2133d-4c3c-4b01-915b-86c0a6132070"), 562m, 10, new DateTime(2024, 6, 29, 14, 52, 1, 671, DateTimeKind.Utc).AddTicks(6153), "Product 1", null },
                    { new Guid("57adef7d-7322-473a-9a34-a0381f2991ba"), 25m, 5469, new DateTime(2024, 6, 29, 14, 52, 1, 671, DateTimeKind.Utc).AddTicks(6162), "Product 2", null },
                    { new Guid("b2dd5f28-9c94-4e69-9e51-2b467793de6b"), 4158m, 100, new DateTime(2024, 6, 29, 14, 52, 1, 671, DateTimeKind.Utc).AddTicks(6160), "Product 2", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
