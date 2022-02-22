using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<ProductController> logger;
        private readonly IMapper imapper;

        public OrdersController(IDutchRepository repository, ILogger<ProductController> logger , IMapper imapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.imapper = imapper;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var result = repository.GetAllOrders(includeItems);
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
               var order = repository.GetOrderById(id);
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
        public IActionResult Post([FromBody]OrderViewModel model)
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
