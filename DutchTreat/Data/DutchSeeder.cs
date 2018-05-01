﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext context, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
        {
            _context = context;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async  Task Seed()
        {
            _context.Database.EnsureCreated();

            var user = await _userManager.FindByEmailAsync("vhmoura@outlook.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Vini",
                    LastName = "Moura",
                    UserName = "vhmoura@outlook.com",
                    Email = "vhmoura@outlook.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("failed to create user manager");
                }
            }

            if (!_context.Products.Any())
            {
                var filePath = Path.Combine(_hosting.ContentRootPath,  "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);

                _context.Products.AddRange(products);
                var order = new Order()
                    {
                        OrderDate = DateTime.Now,
                        OrderNumber = "12345",
                        Items = new List<OrderItem>()
                        {
                            new OrderItem()
                            {
                                Product = products.First(),
                                Quantity = 5,
                                UnitPrice = products.First().Price
                            }
                        }
                    };

                _context.Orders.Add(order);

                _context.SaveChanges();
            }
        }
    }
}
