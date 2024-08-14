using Microsoft.EntityFrameworkCore;
using PruebaCelsia.Data;
using PruebaCelsia.Interfaces;
using PruebaCelsia.Models;
using PruebaCelsia.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaCelsia.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly BaseContext _context;

        public InvoiceService(BaseContext context)
        {
            _context = context;
        }

      public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
{
    return await _context.Invoices
        .Select(invoice => new Invoice
        {
            Id = invoice.Id,
            UserId = invoice.UserId,
            InvoicePeriod = invoice.InvoicePeriod ?? string.Empty, // Manejar nulos con un valor por defecto
            InvoiceNumber = invoice.InvoiceNumber ?? string.Empty,
            BilledAmount = invoice.BilledAmount,
            PaidAmount = invoice.PaidAmount,
            Status = invoice.Status ?? string.Empty,
            User = new User 
            {
                Name = invoice.User.Name ?? string.Empty, // Manejar nulos con un valor por defecto
                Email = invoice.User.Email ?? string.Empty
            }
        })
        .ToListAsync();
}

        public async Task<InvoiceVM> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _context.Invoices
                                        .Include(i => i.User)
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (invoice == null)
            {
                throw new KeyNotFoundException($"No se encontró una factura con el ID {id}.");
            }

            // Mapear la entidad al ViewModel, manejando valores NULL
            var invoiceViewModel = new InvoiceVM
            {
                Id = invoice.Id,
                UserId = invoice.UserId,
                InvoicePeriod = invoice.InvoicePeriod ?? "N/A",
                InvoiceNumber = invoice.InvoiceNumber ?? "N/A",
                BilledAmount = invoice.BilledAmount,
                PaidAmount = invoice.PaidAmount,
                Status = invoice.Status ?? "N/A",
                UserName = invoice.User?.Name ?? "Unknown",
                UserEmail = invoice.User?.Email ?? "Unknown"
            };

            return invoiceViewModel;
        }


        public async Task CreateInvoiceAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
        }

      public async Task UpdateInvoiceAsync(InvoiceVM invoiceVM)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceVM.Id);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"No se encontró una factura con el ID {invoiceVM.Id}.");
            }

            // Actualizar el modelo con datos del ViewModel
            invoice.InvoiceNumber = invoiceVM.InvoiceNumber;
            invoice.InvoicePeriod = invoiceVM.InvoicePeriod;
            invoice.BilledAmount = invoiceVM.BilledAmount;
            invoice.PaidAmount = invoiceVM.PaidAmount;
            invoice.Status = invoiceVM.Status;

            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }
    }
}
