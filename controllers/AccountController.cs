﻿using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> logger;
        private readonly SignInManager<StoreUser> signinManger;
        private readonly UserManager<StoreUser> userManager;
        private readonly IConfiguration config;

        public AccountController(ILogger<AccountController> logger , SignInManager<StoreUser> signinManger ,
            UserManager<StoreUser> userManager , 
            IConfiguration config)
        {
            this.logger = logger;
            this.signinManger = signinManger;
            this.userManager = userManager;
            this.config = config;
        }
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","App");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewMOdel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signinManger.PasswordSignInAsync(model.Username, model.Password, model.RememberMe
                    , false);
                if (result.Succeeded)

                {
                    if(Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    else
                    {
                        RedirectToAction("shop", "App");

                    }
                }                    
                    };

            ModelState.AddModelError("", "Faild to login ");
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
        await    signinManger.SignOutAsync();
            return RedirectToAction("index", "App");
                    
        }

        [HttpPost]
        public async Task<IActionResult> CreatToken([FromBody] LoginViewMOdel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var result = await signinManger.CheckPasswordSignInAsync(user , model.Password ,false);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub , user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            config["Token:Issuer"],
                            config["Token:Audience"], 
                            claims,
                             signingCredentials: creds , 
                            expires: DateTime.UtcNow.AddMinutes(20));

                        return Created("", new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        }) ; 
                    }

                }
            }
            return BadRequest();
        }
    }
}
