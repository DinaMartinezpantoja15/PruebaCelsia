using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruebaCelsia.Models;
using PruebaCelsia.ViewModels;

namespace PruebaCelsia.Interfaces.Account
{
    public interface IAccountService
    {
        Task<User> Register(RegisterVM user);
        Task<User> Login(LoginVM user);
        
        
    }
}