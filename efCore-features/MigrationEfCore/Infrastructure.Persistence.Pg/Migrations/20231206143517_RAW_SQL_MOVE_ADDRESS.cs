using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Pg.Migrations
{
    /// <inheritdoc />
    public partial class RAW_SQL_MOVE_ADDRESS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 ALTER TABLE "Address" ADD "UserId" UUID
                                 """);
            
            migrationBuilder.Sql("""
                                 INSERT INTO "Address" ("City", "Country", "UserId") SELECT
                                 "City", "Country", "Id" FROM "Persones"
                                 """);
            migrationBuilder.Sql("""
                                 UPDATE "Persones" SET "AddressId" = (
                                 SELECT "Address"."Id" FROM "Address" WHERE "Address"."UserId" = "Persones"."Id"
                                 )
                                 """);
            
            migrationBuilder.Sql("""
                                 ALTER TABLE "Address" DROP COLUMN "UserId"
                                 """);
            
            migrationBuilder.Sql("""
                                 ALTER TABLE "Persones" DROP COLUMN "City"
                                 """);
            
            migrationBuilder.Sql("""
                                 ALTER TABLE "Persones" DROP COLUMN "Country"
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
