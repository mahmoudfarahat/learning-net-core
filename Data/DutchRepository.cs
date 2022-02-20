﻿using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext ctx;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext ctx , ILogger<DutchRepository> logger)
        {
            this.ctx = ctx;
            this.logger = logger;
        }

        public void AddEntity(object model)
        {
            ctx.Add(model);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return ctx.Orders.Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToList();
        }

        public IEnumerable<Product> GetALLProduct()
        {
            logger.LogInformation("GetProducts was Called");
            return ctx.Products
                .OrderBy(p => p.Title)
                .ToList();  
        }

        public Order GetOrderById(int id)
        {
            return ctx.Orders.Include(o => o.Items)
               .ThenInclude(i => i.Product)
               .Where(o => o.Id ==id)
               .FirstOrDefault();
        }

        public IEnumerable<Product> GetProductsByCategory(string Category)
        {
            return ctx.Products
                .Where(p => p.Category == Category)
                .ToList();
        }

        public bool SaveAll()
        {
          return  ctx.SaveChanges() > 0; 
        }

    }
}