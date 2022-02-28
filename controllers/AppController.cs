
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailService;
        private readonly IDutchRepository repository;
       

        public AppController(IMailService mailService , IDutchRepository repository)
        {
            this.mailService = mailService;
            this.repository = repository;
        }
        public IActionResult Index()
        {
             
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            ViewBag.Title = "contact us";
          //  throw new InvalidProgramException("my Bad");
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
          if (ModelState.IsValid)
            {
                mailService.SendMessage("mahmoud.gmail.com", model.Name, $"From: {model.Email} , Message: {model.Name}");
                ViewBag.UserMessage = "Sent";
            }else
            {

            }
            return View();
        }
         
        [HttpGet("about")]
        public IActionResult About()
        {
            ViewBag.Title = "About us";
            return View();
        }

         
        public IActionResult shop()
        {
            var results = repository.GetALLProduct();
            return View(results);
        }
    }
}
