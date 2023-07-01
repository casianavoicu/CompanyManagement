using HRManagementWebApi.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRManagementWebApi.Database
{
    sealed public class HRManagementDbContext : DbContext
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
