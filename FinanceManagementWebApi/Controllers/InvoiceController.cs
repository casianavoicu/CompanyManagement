using FinanceManagementWebApi.BusinessLogic;
using FinanceManagementWebApi.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagementWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceBusinessLogic _invoiceBusinessLogic;

        public InvoiceController(IInvoiceBusinessLogic invoiceBusinessLogic)
        {
            _invoiceBusinessLogic = invoiceBusinessLogic;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<Invoice>>> GetAllAsync()
        {
            return await _invoiceBusinessLogic.GetAllAsync();
        }
    }
}