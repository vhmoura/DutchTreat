using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _ctx.Products.OrderBy(p => p.Title).ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _ctx.Products.Where(p => p.Category == category).ToList();
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToList();
            }
            else
            {
                return _ctx.Orders.ToList();
            }

        }

        public Order GetOrderById(string userName, int id)
        {
            return _ctx.Orders.Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefault(a => a.User.UserName == userName && a.Id == id);
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

        public IEnumerable<Order> GetAllOrdersByUser(string userName, bool includeItems)
        {
            return _ctx.Orders.Include(o => o.Items).ThenInclude(i => i.Product).Where(a=>a.User.UserName == userName).ToList();
        }
    }
}
