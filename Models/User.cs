using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaCelsia.Models
{
     public class User
    {public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Role { get; set; }
    public string Address { get; set; }
    public string? IdentificationNumber { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }
}