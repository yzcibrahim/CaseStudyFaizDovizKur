using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Option2Kur.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Option2Kur.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        IConfiguration _configuration;
        public CurrencyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: api/<CurrencyController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<string> currencies = GetCurrencies();
            return currencies;
        }

        private List<string> GetCurrencies()
        {
            return _configuration.GetValue<string>("Currencies").Split(",").ToList();
        }

        // GET api/<CurrencyController>/5
        [HttpGet("{from}/{to}")]
        public IActionResult Get(string from, string to)
        {
            List<string> currencies = GetCurrencies();
            if(!(currencies.Contains(from) && currencies.Contains(to)))
            {
                ModelState.AddModelError("","Desteklenmeyen Döviz Tipi");
                return NotFound("Dööviz tipi desteklenmiyorrrrasdas");
            }

            return Ok(CallApi(from, to));
        }

        // POST api/<CurrencyController>
        [HttpPost]
        public IActionResult Post([FromBody] CurrencyReq value)
        {

            double rate = CallApi(value.From, value.To);

            return Ok(value.Amount * rate);
           
        }

        private double CallApi(string from,string to)
        {
            /*
             * https://api.collectapi.com/economy/currencyToAll?int=10&base=USD
             */

            var client = new HttpClient();

            string apiKey = _configuration.GetValue<string>("ApiKey");

            client.DefaultRequestHeaders.Add("authorization", apiKey);

            var apiResponse = client.GetAsync($"https://api.collectapi.com/economy/currencyToAll?int=1&base={from}").Result;


            CurrencyResult resp = JsonConvert.DeserializeObject<CurrencyResult>(apiResponse.Content.ReadAsStringAsync().Result);

            return resp.Result.Data.FirstOrDefault(c => c.Code == to).Calculated;

        }



        // DELETE api/<CurrencyController>/5

    }
}
