using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.X86;

namespace webapi.Database
{
    public class DatabaseContext : DbContext
    {
        /*
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
         */

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        private string connectionString = "";
        public DatabaseContext() {
            // Data Source=LOCALHOST;Initial Catalog=Message Board;User ID=sa;Password=Welcome123
            connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
