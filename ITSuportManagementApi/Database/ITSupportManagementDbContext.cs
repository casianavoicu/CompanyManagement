using ITSuportManagementApi.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITSuportManagementApi.Database
{
    public class ITSupportManagementDbContext : DbContext
    {
        public ITSupportManagementDbContext()
        {
        }

        public ITSupportManagementDbContext(DbContextOptions<ITSupportManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Equipment> Equipment { get; set; }
    }
}