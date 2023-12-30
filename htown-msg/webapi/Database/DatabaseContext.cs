using Microsoft.EntityFrameworkCore;

namespace webapi.Database;

public class DatabaseContext : DbContext
{
    private static readonly Logger logger = new Logger(typeof(DatabaseContext));
    private static readonly string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

    /********************************************************************************
     *
     * Entity Framework 8 Documentation
     * https://learn.microsoft.com/en-us/ef/core/
     * 
     * You need to make sure the CSPROJ file has the following:
     * <Project Sdk="Microsoft.NET.Sdk.Web">
     *     <PropertyGroup>
     *          <InvariantGlobalization>false</InvariantGlobalization>
     *     </PropertyGroup>         
     * </Project>         
     * 
     ********************************************************************************/

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<MessageEntity> Messages { get; set; }

    public DatabaseContext()
    {
        logger.Trace("DatabaseContext()");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        logger.Trace("OnConfiguring(DbContextOptionsBuilder optionsBuilder)");

        optionsBuilder.UseSqlServer(connectionString);
    }
}
