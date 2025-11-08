using Microsoft.EntityFrameworkCore.Migrations;

namespace Exato.Back.Extensions;

public static class MigrationExtensions
{
    public static void RawSql(this MigrationBuilder migrationBuilder, string sqlFileName)
    {
        var baseDirectory = Path.Combine(AppContext.BaseDirectory, "Raw");
        var path = Path.Combine(baseDirectory, sqlFileName);
        var sql = File.ReadAllText(path);
        migrationBuilder.Sql(sql);
    }

    // migrationBuilder.RawSql("000BeforeIntelligenceBootstrap.sql");
    // migrationBuilder.RawSql("001AfterIntelligenceBootstrap.sql");

    // TRIGGERS LISTEN/NOTIFY
}
