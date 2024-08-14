using OfficeOpenXml;
using PruebaCelsia.Data;
using PruebaCelsia.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PruebaCelsia.Service
{
    public class ExportService
    {
        private readonly BaseContext _context;

        public ExportService(BaseContext context)
        {
            _context = context;
        }

        public async Task<MemoryStream> ExportTransactionsToExcel()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Invoice)
                .ThenInclude(i => i.User)
                .Include(t => t.PaymentMethod)
                .ToListAsync();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Transactions");

                // Headers
                worksheet.Cells[1, 1].Value = "Transaction Code";
                worksheet.Cells[1, 2].Value = "Transaction Date";
                worksheet.Cells[1, 3].Value = "Transaction Amount";
                worksheet.Cells[1, 4].Value = "Transaction Status";
                worksheet.Cells[1, 5].Value = "Invoice Number";
                worksheet.Cells[1, 6].Value = "Invoice Period";
                worksheet.Cells[1, 7].Value = "Billed Amount";
                worksheet.Cells[1, 8].Value = "Paid Amount";
                worksheet.Cells[1, 9].Value = "Invoice Status";
                worksheet.Cells[1, 10].Value = "User Name";
                worksheet.Cells[1, 11].Value = "User Email";
                worksheet.Cells[1, 12].Value = "User Phone";
                worksheet.Cells[1, 13].Value = "Payment Method";

                // Data
                for (int i = 0; i < transactions.Count; i++)
                {
                    var transaction = transactions[i];
                    worksheet.Cells[i + 2, 1].Value = transaction.CodigoTransacción;
                    worksheet.Cells[i + 2, 2].Value = transaction.TransactionDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 3].Value = transaction.Amount;
                    worksheet.Cells[i + 2, 4].Value = transaction.TransactionStatus;
                    worksheet.Cells[i + 2, 5].Value = transaction.Invoice?.InvoiceNumber;
                    worksheet.Cells[i + 2, 6].Value = transaction.Invoice?.InvoicePeriod;
                    worksheet.Cells[i + 2, 7].Value = transaction.Invoice?.BilledAmount;
                    worksheet.Cells[i + 2, 8].Value = transaction.Invoice?.PaidAmount;
                    worksheet.Cells[i + 2, 9].Value = transaction.Invoice?.Status;
                    worksheet.Cells[i + 2, 10].Value = transaction.Invoice?.User?.Name;
                    worksheet.Cells[i + 2, 11].Value = transaction.Invoice?.User?.Email;
                    worksheet.Cells[i + 2, 12].Value = transaction.Invoice?.User?.Phone;
                    worksheet.Cells[i + 2, 13].Value = transaction.PaymentMethod?.MethodName;
                }

                package.SaveAs(stream);
            }

            stream.Position = 0;
            return stream;
        }
    }
}
