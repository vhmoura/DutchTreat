using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controller
{
    [Route("api/[Controller]")]
    public class OrdersController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IDutchRepository _dutchRepository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository dutchRepository, ILogger<OrdersController> logger, IMapper mapper)
        {
            _dutchRepository = dutchRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Get(bool includeItems = true)
        {
            var results = _dutchRepository.GetAllOrders(includeItems);
            return Json(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            return Json(_mapper.Map<Order, OrderViewModel>(_dutchRepository.GetOrderById(id)) );
        }

        [HttpPost]
        public IActionResult Post([FromBody]OrderViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var newModel = _mapper.Map<OrderViewModel, Order>(model);

                if (newModel.OrderDate == DateTime.MinValue)
                {
                    newModel.OrderDate = DateTime.Now;
                }

                _dutchRepository.AddEntity(newModel);
                if (_dutchRepository.SaveAll())
                {
                    var vm = _mapper.Map<Order, OrderViewModel>(newModel);
                    return Created($"/api/orders/{vm.OrderId}", vm);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

            return BadRequest("Failed to create model");
        }
    }
}