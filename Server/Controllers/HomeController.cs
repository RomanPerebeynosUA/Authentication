﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }
        public IActionResult Authenticate()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                new Claim("granny",  "cookie"),
            };
            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);

            var key = new SymmetricSecurityKey(secretBytes);

            var algoritm = SecurityAlgorithms.HmacSha256;

            var siningCredentials = new SigningCredentials(key, algoritm);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audiance,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                siningCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);


            return Ok(new { acces_token = tokenJson });

        }
        public IActionResult Decode(string part)
        {
          var bytes =   Convert.FromBase64String(part);

            return Ok(Encoding.UTF8.GetString(bytes));
        }
    }
}