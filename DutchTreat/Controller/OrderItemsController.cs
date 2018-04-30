﻿using System;
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
    [Route("/api/orders/{orderId}/items")]
    public class OrderItemsController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository repository, ILogger<OrderItemsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult Get(int orderId)
        {
            var order = _repository.GetOrderById(orderId);
            return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int orderId, int Id)
        {
            var order = _repository.GetOrderById(orderId);
            return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(order.Items.FirstOrDefault(i => i.Id == Id)));
        }
    }
}