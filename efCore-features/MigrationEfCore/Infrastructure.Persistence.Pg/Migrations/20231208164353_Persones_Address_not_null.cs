using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Pg.Migrations
{
    /// <inheritdoc />
    public partial class Persones_Address_not_null : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Добавили Дефолтный адрес
            migrationBuilder.Sql("""
                                 INSERT INTO "Address" ("City", "Country") VALUES ('Unknown', 'Unknown')
                                 """);
            
            //Добавили Дефолтный адрес ко всем Persones, где раньше был null
            migrationBuilder.Sql("""
                                 UPDATE "Persones" SET "AddressId" = (
                                     SELECT "Id" FROM "Address" WHERE "City" = 'Unknown'
                                     )
                                 where "AddressId" is null
                                 """);
            
            //Добавили not null ограничение на FK AddressId
            migrationBuilder.Sql("""
                                 ALTER TABLE "Persones" ALTER COLUMN "AddressId" set not null;
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
