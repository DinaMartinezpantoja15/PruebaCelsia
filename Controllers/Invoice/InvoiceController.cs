using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaCelsia.Interfaces;

using PruebaCelsia.Models;
using PruebaCelsia.ViewModels;
using System.Threading.Tasks;

namespace PruebaCelsia.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

       
        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return View(invoices);
        }

       
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

       
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,InvoiceNumber,InvoicePeriod,BilledAmount,PaidAmount,Status")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                await _invoiceService.CreateInvoiceAsync(invoice);
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
                }


         public async Task<IActionResult> Edit(int id)
        {
            var invoiceVM = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoiceVM == null)
            {
                return NotFound();
            }
            return View(invoiceVM); // Pasando InvoiceVM a la vista
        }

        // Acción POST para editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,InvoiceNumber,InvoicePeriod,BilledAmount,PaidAmount,Status")] InvoiceVM invoiceVM)
        {
            if (id != invoiceVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _invoiceService.UpdateInvoiceAsync(invoiceVM);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await InvoiceExists(invoiceVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(invoiceVM); // Devolver el ViewModel en caso de error de validación
        }

        // Método privado para verificar si la factura existe
        private async Task<bool> InvoiceExists(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            return invoice != null;
        }
    

       
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoiceService.DeleteInvoiceAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
