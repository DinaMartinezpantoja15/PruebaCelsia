using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaCelsia.Models
{
    public class Transaction
    {
            public int Id { get; set; }
            public int InvoiceId { get; set; }
            public int PaymentMethodId { get; set; }
            public string CodigoTransacción { get; set; }
            public DateTime TransactionDate { get; set; }
            public int Amount { get; set; }
            public string TransactionStatus { get; set; }

            // Navigation properties
            public Invoice Invoice { get; set; }
            public PaymentMethod PaymentMethod { get; set; }
    }
}