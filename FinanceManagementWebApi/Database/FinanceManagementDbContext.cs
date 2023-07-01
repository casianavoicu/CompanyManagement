using FinanceManagementWebApi.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagementWebApi.Database
{
    public class FinanceManagementDbContext : DbContext
    {
        public FinanceManagementDbContext()
        {

        }

        public FinanceManagementDbContext(DbContextOptions<FinanceManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Invoice> Invoice { get; set; }
    }
}
