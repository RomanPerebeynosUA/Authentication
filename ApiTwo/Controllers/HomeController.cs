using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;


namespace ApiTwo.Controllers
{
    public class HomeController: Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [Route("/")]
        public async Task<IActionResult> Index()
        {
            //retrieve access token 
            var serverCLient = _httpClientFactory.CreateClient();

            var discoveryDocumemt = await serverCLient.
                GetDiscoveryDocumentAsync("https://localhost:44364/");

            var tokenResponce = await serverCLient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocumemt.TokenEndpoint,

                    ClientId = "client_id",
                    ClientSecret = "client_secret",

                    Scope = "ApiOne"
                });

            //retrieve secret data

            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(tokenResponce.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:44362/secret");

            var contet = await response.Content.ReadAsStringAsync();

            return Ok(new 
            { 
                access_token = tokenResponce.AccessToken,
                message = contet,
            });
        }
    }
}
