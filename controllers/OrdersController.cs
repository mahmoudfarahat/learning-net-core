using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<ProductController> logger;
        private readonly IMapper imapper;
        private readonly UserManager<StoreUser> userManager;

        public OrdersController(IDutchRepository repository, ILogger<ProductController> logger ,
            IMapper imapper , UserManager<StoreUser> userManager)
        {
            this.repository = repository;
            this.logger = logger;
            this.imapper = imapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;
                var result = repository.GetAllOrdersByUser(username , includeItems);
                return Ok(imapper.Map<IEnumerable<OrderViewModel>>(result));
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex}");
                return BadRequest("Failede");
            }
        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
               var order = repository.GetOrderById(User.Identity.Name , id);
                if (order != null) return Ok(imapper.Map<Order,OrderViewModel>(order));
                else return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex}");
                return BadRequest("Failede");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = imapper.Map<OrderViewModel, Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }
                    var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                    newOrder.User = currentUser;

                    repository.AddEntity(newOrder);
                    if (repository.SaveAll())  
                                      {
                         
                        return Created($"/api/oders/{newOrder.Id}", imapper.Map<Order, OrderViewModel>(newOrder));

                    }
                }
              else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                logger.LogError($"{ex}");
            }

            return BadRequest("Failed");
        }
    }
}
