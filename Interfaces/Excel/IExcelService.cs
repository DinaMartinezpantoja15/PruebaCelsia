using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaCelsia.Interfaces.Excel
{
    public interface IExcelService
    {
        Task ProcessExcelFile(IFormFile file);
    }
}