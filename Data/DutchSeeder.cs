using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext ctx;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<StoreUser> userManager;

        public DutchSeeder(DutchContext ctx , IWebHostEnvironment env , UserManager<StoreUser> userManager) 
        {
            this.ctx = ctx;
            this.env = env;
            this.userManager = userManager;
        }

        public async Task SeedAsync()
        {
            ctx.Database.EnsureCreated();

            StoreUser user = await userManager.FindByEmailAsync("shawn@dutchtreat.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Shawn",
                    LastName = "Wildermuth",
                    Email = "Shawn@dutchtreat.com",
                    UserName = "Shawn@dutchtreat.com"
                };
                var result = await userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("could not create new user in seeder");
                }
            }
            if ( !ctx.Products.Any())
            {
                var filePath =Path.Combine(env.ContentRootPath,"Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
                ctx.Products.AddRange(products);
              
                var order = new Order()
                {
                    User = user,
                    OrderDate = DateTime.Today,
                    OrderNumber = "1000",


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
                ctx.Orders.Add(order);
                ctx.SaveChanges();
            }
        }
    }
}
