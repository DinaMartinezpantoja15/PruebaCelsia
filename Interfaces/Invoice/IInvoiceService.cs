using PruebaCelsia.Models;
using PruebaCelsia.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaCelsia.Interfaces
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<InvoiceVM> GetInvoiceByIdAsync(int id);
        Task CreateInvoiceAsync(Invoice invoice);
        Task UpdateInvoiceAsync(InvoiceVM invoiceVM);
        Task DeleteInvoiceAsync(int id);
    }
}
