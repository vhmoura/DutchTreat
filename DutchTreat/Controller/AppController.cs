using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DutchTreat.Controller
{
    public class AppController: Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IMailService _mailService;
        private readonly IDutchRepository _dutchRepository;

        public AppController(IMailService mailService, IDutchRepository dutchRepository)
        {
            _mailService = mailService;
            _dutchRepository = dutchRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact us";

            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMessage("vhmoura@gmail.com", model.Subject,
                    $"From: {model.Email}, Message: {model.Message}");

                ViewBag.UserMessage = "mail sent";
                ModelState.Clear();
            }

            return View();
        }


        [HttpGet("about")]
        public IActionResult About()
        {
            ViewBag.Title = "About";
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }
    }
}
