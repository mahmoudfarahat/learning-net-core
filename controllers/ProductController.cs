using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.controllers
{
        [Route("api/[Controller]")]
        [ApiController]
        [Produces("application/Json")]
    public class ProductController : Controller  
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<ProductController> logger;

        public ProductController(IDutchRepository repository , ILogger<ProductController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
                public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                return Ok(repository.GetALLProduct());

            }
            catch (Exception ex)
            {
                logger.LogError($"{ex}");
                return BadRequest("Bad request");
            }
        }
    }
}
