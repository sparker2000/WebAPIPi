using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Com.Enterprisecoding.RPI.GPIO;
using Com.Enterprisecoding.RPI.GPIO.Enums;

namespace WebAPIPi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            BoardInfo boardInfo = WiringPi.OnBoardHardware.PiBoardInfo();
            Console.WriteLine("  Type: {0}, Revision: {1}, Memory: {2}MB, Maker: {3} {4}", 
                boardInfo.ModelName, boardInfo.RevisionName, boardInfo.MemoryValue, boardInfo.Maker, boardInfo.OverVolted ? "[OV]" : "");

            int result = WiringPi.Core.Setup();

            if (result == -1)
            {
                return ("WiringPi init failed!");
            }

            WiringPi.Core.PinMode(0, PinMode.Output);

            Console.WriteLine("Looping Pin 0...");
            for (int i = 0; i < 10; i++)
            {
                WiringPi.Core.DigitalWrite(0, DigitalValue.High);
                System.Threading.Thread.Sleep(1000);

                WiringPi.Core.DigitalWrite(0, DigitalValue.Low);
                System.Threading.Thread.Sleep(1000);
            }

            return "0";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
