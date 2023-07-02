using HRManagementWebApi.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagementWebApi.Database
{
    public sealed class HRManagementDbContext : DbContext
    {
        public HRManagementDbContext()
        {
        }

        public HRManagementDbContext(DbContextOptions<HRManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
    }
}