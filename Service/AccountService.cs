using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using PruebaCelsia.Data;
using PruebaCelsia.Interfaces.Account;
using PruebaCelsia.Models;
using PruebaCelsia.ViewModels;

namespace PruebaCelsia.Service
{
    public class AccountService: IAccountService
    {
        private readonly BaseContext _context;

        public AccountService(BaseContext context)
        {
            _context = context;
        }

        public async Task<User> Login(LoginVM user)
        {
            // Busca el usuario por correo electrónico
            User? userFind = await _context.Users
                .Where(u => u.Email == user.Email)
                .FirstOrDefaultAsync();

            // Verifica la contraseña del usuario
            if (userFind != null && BCrypt.Net.BCrypt.Verify(user.Password, userFind.Password))
            {
                return userFind;
            }
            return null!;
        }

        public async Task<User> Register(RegisterVM user)
        {
            // Verifica si el correo electrónico ya está en uso
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                throw new Exception("El correo ya esta en uso.");
            }

            // Crea un nuevo usuario
            User userRegistration = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                
            };

            // Agrega el nuevo usuario a la base de datos y guarda los cambios
            await _context.Users.AddAsync(userRegistration);
            await _context.SaveChangesAsync();
            return userRegistration;
        }



    }
}