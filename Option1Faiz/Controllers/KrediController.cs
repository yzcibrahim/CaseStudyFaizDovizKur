using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Option1Faiz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Option1Faiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KrediController : ControllerBase
    {
        IConfiguration _configuration;
        public KrediController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // POST api/<KrediController>
        [HttpPost]
        public IActionResult Post([FromBody] KrediRequest krediRequest)
        {

            var dcMax = decimal.MaxValue;
            var dblMax = double.MaxValue;

            GetFromApi();

            KrediResponse krediResponse = new KrediResponse();
            //decimal faizOran = _configuration.GetValue<decimal>("FaizOran");
            double faizOran = GetFromApi();

            krediResponse.FaizMiktari = krediRequest.Tutar * faizOran / 100;

            krediResponse.ToplamTutar = krediRequest.Tutar + krediResponse.FaizMiktari;

            return Ok(krediResponse);

        }

        private double GetFromApi()
        {
            var client = new HttpClient();//"https://api.collectapi.com/credit/ihtiyacKredi");

            string apiKey = _configuration.GetValue<string>("FaizApiKey");

            client.DefaultRequestHeaders.Add("authorization", apiKey);

            var apiResponse=client.GetAsync("https://api.collectapi.com/credit/ihtiyacKredi").Result;

            dynamic resp = JsonConvert.DeserializeObject<dynamic>(apiResponse.Content.ReadAsStringAsync().Result);

            string faizOranDyn = resp.result[0].faiz;

            double faizOran = Convert.ToDouble(faizOranDyn.Replace("%","").Replace(",","."));
            return faizOran;
        }


    }
}
