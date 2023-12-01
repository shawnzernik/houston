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
         */

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
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
