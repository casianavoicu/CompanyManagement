namespace CompanyManagement.Common.Dto
{
    public class InvoiceDto
    {
        public string? Sender { get; set; }

        public string? Product { get; set; }

        public decimal Price { get; set; }
    }
}