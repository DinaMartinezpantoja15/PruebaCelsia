using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using PruebaCelsia.Data;
using PruebaCelsia.Interfaces.Excel;
using PruebaCelsia.Models;

namespace PruebaCelsia.Service
{
    public class ExcelService : IExcelService
    {
        private readonly BaseContext _context;
        private readonly ILogger<ExcelService> _logger;

        public ExcelService(BaseContext context, ILogger<ExcelService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ProcessExcelFile(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        string userName = worksheet.Cells[row, 6].Text;
                        string userEmail = worksheet.Cells[row, 10].Text;
                        string userPhone = worksheet.Cells[row, 9].Text;
                        string userAddress = worksheet.Cells[row, 8].Text;
                        string invoiceNumber = worksheet.Cells[row, 12].Text;
                        string invoicePeriod = worksheet.Cells[row, 13].Text; 
                        string identificationNumber = worksheet.Cells[row, 7].Text;

                        // Limpieza de valores numéricos
                        string billedAmountText = worksheet.Cells[row, 15].Text.Replace(",", "").Replace(" ", "");
                        if (!int.TryParse(billedAmountText, out int billedAmount))
                        {
                            _logger.LogError("BilledAmount is not a valid integer at row {Row}. Value: {Value}", row, billedAmountText);
                            continue;
                        }

                        string paidAmountText = worksheet.Cells[row, 14].Text.Replace(",", "").Replace(" ", "");
                        int paidAmount = 0; // Default to 0 if the cell is empty or contains 0
                        if (!string.IsNullOrWhiteSpace(paidAmountText))
                        {
                            if (!int.TryParse(paidAmountText, out paidAmount))
                            {
                                _logger.LogError("PaidAmount is not a valid integer at row {Row}. Value: {Value}", row, paidAmountText);
                                continue;
                            }
                        }

                        
                        string paymentMethodName = worksheet.Cells[row, 11].Text;
                        string transactionCode = worksheet.Cells[row, 1].Text;
                        string transactionDateText = worksheet.Cells[row, 2].Text;

                        string transactionAmountText = worksheet.Cells[row, 3].Text.Replace(",", "").Replace(" ", "");
                        if (!int.TryParse(transactionAmountText, out int transactionAmount))
                        {
                            _logger.LogError("TransactionAmount is not a valid integer at row {Row}. Value: {Value}", row, transactionAmountText);
                            continue;
                        }

                        string transactionStatus = worksheet.Cells[row, 4].Text;

                        // Procesa el usuario
                        var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
                        if (user == null)
                        {
                            user = new User
                            {
                                Name = userName,
                                Email = userEmail,
                                Phone = userPhone,
                                Address = userAddress,
                                Password = "null",
                                Role = "User", 
                                IdentificationNumber =identificationNumber
                            };
                            _context.Users.Add(user);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation("Added user: {Email}", userEmail);
                        }

                        
                        // Procesa la factura
                        var invoice = _context.Invoices.FirstOrDefault(i => i.InvoiceNumber == invoiceNumber && i.UserId == user.Id);
                            if (invoice == null)
                            {
                                invoice = new Invoice
                                {
                                    UserId = user.Id,
                                    InvoiceNumber = invoiceNumber,
                                    InvoicePeriod = invoicePeriod,
                                    BilledAmount = billedAmount,
                                    PaidAmount = paidAmount,
                                    Status = "Active"
                                };
                                _context.Invoices.Add(invoice);
                                await _context.SaveChangesAsync();
                                _logger.LogInformation("Added invoice: {InvoiceNumber}", invoiceNumber);
                            }
                        // Procesa el método de pago
                        var paymentMethod = _context.PaymentMethods.FirstOrDefault(pm => pm.MethodName == paymentMethodName);
                        if (paymentMethod == null)
                        {
                            paymentMethod = new PaymentMethod
                            {
                                MethodName = paymentMethodName
                            };
                            _context.PaymentMethods.Add(paymentMethod);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation("Added payment method: {MethodName}", paymentMethodName);
                        }

                        // Procesa la transacción
                        if (!DateTime.TryParse(transactionDateText, out DateTime transactionDate))
                        {
                            _logger.LogError("TransactionDate is not a valid date at row {Row}. Value: {Value}", row, transactionDateText);
                            continue;
                        }

                        var transaction = _context.Transactions.FirstOrDefault(t => t.CodigoTransacción == transactionCode && t.InvoiceId == invoice.Id && t.PaymentMethodId == paymentMethod.Id);
                        if (transaction == null)
                        {
                            transaction = new Transaction
                            {
                                InvoiceId = invoice.Id,
                                PaymentMethodId = paymentMethod.Id,
                                CodigoTransacción = transactionCode,
                                TransactionDate = transactionDate,
                                Amount = transactionAmount,
                                TransactionStatus = transactionStatus
                            };
                            _context.Transactions.Add(transaction);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation("Added transaction: {TransactionCode}", transactionCode);
                        }
                    }
                }
            }
        }
    }
}
