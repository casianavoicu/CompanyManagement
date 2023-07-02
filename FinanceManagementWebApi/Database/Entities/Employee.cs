using System.ComponentModel.DataAnnotations;

namespace FinanceManagementWebApi.Database.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public decimal Salary { get; set; }

    }
}
