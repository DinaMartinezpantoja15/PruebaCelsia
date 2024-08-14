using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaCelsia.ViewModels
{
    public class UserVM
    {
        
        public string? Name { get; set; }

     
        public string? Email { get; set; }

       
        public string? Password { get; set; }
        
       
        public DateTime? RegistrationDate { get; set; }

        public string? Phone { get; set; }
        public string? Role { get; set; }

        public DateTime? CreatedAt { get; set; }

    
        public string? Address { get; set; }
        
    }
}