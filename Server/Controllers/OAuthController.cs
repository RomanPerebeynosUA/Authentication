using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Server.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public IActionResult Authorize(
            string responce_type,  // authorization flow type
            string client_id, // client id
            string redirect_uri, 
            string scope, //what info i want = email, grandma, tel...
            string state) // random string generated to confirm that we are going to back to the same client 
              
        {
            var query = new QueryBuilder();
            query.Add("redirectUri", redirect_uri);
            query.Add("state", state);

            return View(model: query.ToString());
        }
        [HttpPost]
        public IActionResult Authorize(
            string username,
            string redirect_uri,
            string state)
        {
            const string code = "qwerty1243";

            var query = new QueryBuilder();
            query.Add("code", code);
            query.Add("state", state);

            return Redirect($"{redirect_uri}");
        }
        public async Task<IActionResult> Token(
            string grant_type, // flow of access_token request 
            string code, // confi
            string redirect_uri,
            string client_id)
        {
            // some mechanism for validating the code 
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

            var acces_token = new JwtSecurityTokenHandler().WriteToken(token);

            var responseObject = new
            {
                acces_token,
                token_type = "Bearer",
                raw_claim = "oauthTutorial"
            };
            var responseJson = JsonConvert.SerializeObject(responseObject);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);

            return Redirect(redirect_uri);
        }

    }
}