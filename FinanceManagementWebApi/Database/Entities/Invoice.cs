using System.ComponentModel.DataAnnotations;

namespace FinanceManagementWebApi.Database.Entities
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        public string? InvoiceBody { get; set; }
    }
}
