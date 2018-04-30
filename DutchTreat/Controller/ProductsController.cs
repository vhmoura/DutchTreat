using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controller
{
    [Route("api/[Controller]")]
    public class ProductsController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IDutchRepository _dutchRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IDutchRepository dutchRepository, ILogger<ProductsController> logger)
        {
            _dutchRepository = dutchRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_dutchRepository.GetAllProducts());
        }
    }
}