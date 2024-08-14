using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaCelsia.Models
{
    public class Invoice
    { public int Id { get; set; }
    
    public string InvoicePeriod { get; set; }
    public string InvoiceNumber { get; set; }
    public int BilledAmount { get; set; }
    public int PaidAmount { get; set; }
    public string Status { get; set; }
public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
    }
}