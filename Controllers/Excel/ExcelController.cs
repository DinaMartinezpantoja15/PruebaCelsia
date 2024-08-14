using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PruebaCelsia.Interfaces.Excel;
using PruebaCelsia.Service;

namespace PruebaCelsia.Controllers.Excel
{
    [Route("[controller]")]
    public class ExcelController : Controller
    {
      
        private readonly IExcelService _excelService;
        private readonly TransactionService _transactionService;
        private readonly ExportService _exportService;
        private readonly ILogger<ExcelController> _logger;

        public ExcelController(IExcelService excelService, TransactionService transactionService, ExportService exportService, ILogger<ExcelController> logger)
        {
            _excelService = excelService;
            _transactionService = transactionService;
            _exportService = exportService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("Invalid file uploaded.");
                return BadRequest("Please upload a valid Excel file.");
            }

            _logger.LogInformation("Processing uploaded file: {FileName}", file.FileName);
            await _excelService.ProcessExcelFile(file);
            _logger.LogInformation("File processed successfully.");

            return Ok("File processed successfully.");
        }

        [HttpGet("view-data")]
        public async Task<IActionResult> ViewExcelData(string sortOrder, int pageNumber = 1, int pageSize = 10)
        {
            var paginatedSubjects = await _transactionService.GetPagedTransactionsAsync(sortOrder, pageNumber, pageSize);
            return View(paginatedSubjects);
        }

        [HttpGet("export-data")]
        public async Task<IActionResult> ExportData()
        {
            var stream = await _exportService.ExportTransactionsToExcel();
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "subjects.xlsx";

            return File(stream, contentType, fileName);
        }
    }
}