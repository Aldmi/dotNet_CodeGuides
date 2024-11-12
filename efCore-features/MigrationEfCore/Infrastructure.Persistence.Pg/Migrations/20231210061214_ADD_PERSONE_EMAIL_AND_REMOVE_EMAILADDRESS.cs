using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Pg.Migrations
{
    /// <inheritdoc />
    public partial class ADD_PERSONE_EMAIL_AND_REMOVE_EMAILADDRESS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Persones",
                newName: "Email_Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email_Value",
                table: "Persones",
                newName: "EmailAddress");
        }
    }
}
