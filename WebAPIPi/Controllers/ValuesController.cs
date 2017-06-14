using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Com.Enterprisecoding.RPI.GPIO;
using Com.Enterprisecoding.RPI.GPIO.Enums;

namespace WebAPIPi.Controllers
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// NOTE: THIS PINMAP ONLY WORKS ON THE RASPBERRY PI 3 MODEL B V1.2
        /// If you have a different pi model, make sure you visit the wiringpi website
        /// https://projects.drogon.net/raspberry-pi/wiringpi/pins/
        /// to figure out what your pinmap is.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, int> GetPinMap()
        {            
            // Incredibly confusing.  I'll create a map.  If you want pin 0 to go high,
            // You're actually asking for "wiringpi" pin 8 to go high. 8 -> 0
            // Pinmapping:
            // 0 : 17
            // 1 : 18
            // 2 : 13
            // 3 : 22
            // 4 : 23
            // 5 : 24
            // 6 : 25
            // 7 : 4
            // 8 : 0
            // 9 : 1
            // 10 : 8
            // 11 : 7
            // 12 : 10
            // 13 : 9
            // 14 : 11
            // 15 : 14
            // 16 : 15

            Dictionary<int, int> pinMap = new Dictionary<int, int>();
            pinMap.Add(0, 8);
            pinMap.Add(1, 9);
            // 2-3 is invalid
            pinMap.Add(4, 7);
            // 5-6 is invalid
            pinMap.Add(7, 11);
            pinMap.Add(8, 10);
            pinMap.Add(9, 13);
            pinMap.Add(10, 12);
            pinMap.Add(11, 14);
            // 12 is invalid
            pinMap.Add(13, 2);
            pinMap.Add(14, 15);
            pinMap.Add(15, 16);
            // 16 is invalid
            pinMap.Add(17, 0);
            pinMap.Add(18, 1);
            // Pin 19-21 is invalid
            pinMap.Add(22, 3);
            pinMap.Add(23, 4);
            pinMap.Add(24, 5);
            pinMap.Add(25, 6);

            return pinMap;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Result Get(int id)
        {
            Dictionary<int, int> pinMap = GetPinMap();

            if (!pinMap.ContainsKey(id))
            {
                return new Result() { Success = false, Message = string.Format("Pin {0} is invalid for WiringPi.", id) };
            }

            BoardInfo boardInfo = WiringPi.OnBoardHardware.PiBoardInfo();
            Console.WriteLine("  Type: {0}, Revision: {1}, Memory: {2}MB, Maker: {3} {4}", 
                boardInfo.ModelName, boardInfo.RevisionName, boardInfo.MemoryValue, boardInfo.Maker, boardInfo.OverVolted ? "[OV]" : "");
            
            LoopPin(id, pinMap[id]);
            
            return new Result() { Success = true, Message = "0" };
        }

        private void LoopPin(int actual, int wiringPinNum)
        {
            WiringPi.Core.PinMode(wiringPinNum, PinMode.Output);

            Console.WriteLine(string.Format("Looping Pin {0}...", actual));
            for (int i = 0; i < 10; i++)
            {
                WiringPi.Core.DigitalWrite(wiringPinNum, DigitalValue.High);
                System.Threading.Thread.Sleep(1000);

                WiringPi.Core.DigitalWrite(wiringPinNum, DigitalValue.Low);
                System.Threading.Thread.Sleep(1000);
            }
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
