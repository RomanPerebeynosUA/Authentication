﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        public HomeController(IHttpClientFactory HttpClientFactory)
        {
            _client = HttpClientFactory.CreateClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer { token}");

            var serverResponse = await _client.GetAsync("https://localhost:44316/secret/index");
            var apiResponse = await _client.GetAsync("https://localhost:44328/secret/index");
        
            return View();
        }
    }
}