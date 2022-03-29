using Customer.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Data.DataAccess
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options)
        {

        }

        public DbSet<Entities.Customer> Customer { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<LGA> Lga { get; set; }
        public DbSet<UserOTP> UserOTP { get; set; }
    }
}
