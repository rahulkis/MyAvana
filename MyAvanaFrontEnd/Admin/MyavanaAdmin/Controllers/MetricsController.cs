using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyavanaAdmin.Factory;

namespace MyavanaAdmin.Controllers
{
    public class MetricsController : Controller
    {
        public async Task<IActionResult> QAMetrics()
        {
                return View();
        }
    }
}