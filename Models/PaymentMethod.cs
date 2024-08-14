using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaCelsia.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Method name is required.")]
        public string? MethodName { get; set; }

        public ICollection<Transaction>? Transactions { get; set; } // Relación con la entidad Transaction
    
    }
}