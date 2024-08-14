using PruebaCelsia.Data;
using PruebaCelsia.Models;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using PruebaCelsia.Helpers;

namespace PruebaCelsia.Service
{
    public class TransactionService
    {
        private readonly BaseContext _context;

        public TransactionService(BaseContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Transaction>> GetPagedTransactionsAsync(string sortOrder, int pageNumber, int pageSize)
        {
            var transactions = _context.Transactions
                .Include(t => t.Invoice)
                .ThenInclude(i => i.User)
                .Include(t => t.PaymentMethod)
                .AsQueryable();

            switch (sortOrder)
            {
                case "code_desc":
                    transactions = transactions.OrderByDescending(t => t.CodigoTransacción);
                    break;
                case "date":
                    transactions = transactions.OrderBy(t => t.TransactionDate);
                    break;
                case "date_desc":
                    transactions = transactions.OrderByDescending(t => t.TransactionDate);
                    break;
                case "amount":
                    transactions = transactions.OrderBy(t => t.Amount);
                    break;
                case "amount_desc":
                    transactions = transactions.OrderByDescending(t => t.Amount);
                    break;
                default:
                    transactions = transactions.OrderBy(t => t.CodigoTransacción);
                    break;
            }

            int totalRecords = await transactions.CountAsync();
            var data = await transactions.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<Transaction>(data, totalRecords, pageNumber, pageSize);
        }
    }
}
