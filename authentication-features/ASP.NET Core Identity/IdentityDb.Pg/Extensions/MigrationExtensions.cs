using System.Formats.Asn1;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityDb.Pg.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigration(this IServiceProvider serviceProvider)
    {
        using var scope= serviceProvider.CreateScope();
        var context=scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        context.Database.Migrate();
    }
}