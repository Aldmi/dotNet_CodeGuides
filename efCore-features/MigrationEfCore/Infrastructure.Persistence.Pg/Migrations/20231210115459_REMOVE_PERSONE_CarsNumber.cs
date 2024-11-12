using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Pg.Migrations
{
    /// <inheritdoc />
    public partial class REMOVE_PERSONE_CarsNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Добавили Дефолтный адрес
            migrationBuilder.Sql("""
                                 INSERT INTO "Car" ("PersoneId", "ModelName", "RegisterNumber", "YearManufacture")
                                 SELECT
                                     "Id", 'Unknown', "CarsNumber", '1990-01-01 00:00:00'
                                     FROM "Persones"
                                 """);
            
            migrationBuilder.Sql("""
                                 ALTER TABLE "Persones" DROP COLUMN "CarsNumber"
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
