using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaCelsia.ViewModels
{
   public class InvoiceVM
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? InvoicePeriod { get; set; }
    public string? InvoiceNumber { get; set; }
    public int BilledAmount { get; set; }
    public int PaidAmount { get; set; }
    public string? Status { get; set; }

    // Propiedades del usuario
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
}

}