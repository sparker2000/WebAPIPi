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
    public class GPIOController : Controller
    {
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

        /// <summary>
        /// Get values from all available pins
        /// </summary>
        [HttpGet]
        public string[] Get()
        {
            Dictionary<int, int> pinMap = GetPinMap();

            List<string> resultStrings = new List<string>();

            foreach(int key in pinMap.Keys)
            {
                WiringPi.Core.PinMode(pinMap[key], PinMode.Input);
                int val = WiringPi.Core.AnalogRead(pinMap[key]);

                resultStrings.Add(string.Format("Pin {0}: {1}", key, val));
            }

            return resultStrings.ToArray();
        }

        /// <summary>
        /// Get information about the board you are connected to
        /// </summary>
        [HttpGet("GetBoardInfo")]
        public Result GetBoardInfo()
        {
            BoardInfo boardInfo = WiringPi.OnBoardHardware.PiBoardInfo();

            return new Result() { Success = true, Message = "  Type: {0}, Revision: {1}, Memory: {2}MB, Maker: {3} {4}", 
                    boardInfo.ModelName, boardInfo.RevisionName, boardInfo.MemoryValue, boardInfo.Maker, boardInfo.OverVolted ? "[OV]" : "" }
        }


        // GET api/values/5
        /// <summary>
        /// Get inputs from a specific pin pins
        /// </summary>        
        [HttpGet("{id}")]
        public Result Get(int id)
        {
            Dictionary<int, int> pinMap = GetPinMap();

            if (!pinMap.ContainsKey(id))
            {
                return new Result() { Success = false, Message = string.Format("Pin {0} is invalid for WiringPi.", id) };
            }

            WiringPi.Core.PinMode(pinMap[id], PinMode.Input);

            int val = WiringPi.Core.AnalogRead(pinMap[id]);
            return new Result() { Success = true, Message = val.ToString()};
        }

        // private void LoopPin(int actual, int wiringPinNum)
        // {
        //     WiringPi.Core.PinMode(wiringPinNum, PinMode.Output);

        //     Console.WriteLine(string.Format("Looping Pin {0}...", actual));
        //     for (int i = 0; i < 10; i++)
        //     {
        //         WiringPi.Core.DigitalWrite(wiringPinNum, DigitalValue.High);
        //         System.Threading.Thread.Sleep(1000);

        //         WiringPi.Core.DigitalWrite(wiringPinNum, DigitalValue.Low);
        //         System.Threading.Thread.Sleep(1000);
        //     }
        // }

        // POST api/values
        [HttpPost]
        public Result Post([FromBody]PinRequest req)
        {
            bool success = true;
            string message = "complete";
            Dictionary<int, int> pinMap = GetPinMap();

            if (!pinMap.ContainsKey(req.PinNumber))
            {
            return new Result() { Success = false, Message = string.Format("Pin {0} is invalid for WiringPi.", id) };
            }

            switch (req.Action)
            {
                case PinAction.AnalogWrite:
                    AnalogWrite(req.PinNumber, req.Value);
                    break;
                case PinAction.DigitalWrite:
                    DigitalWrite(req.PinNumber, req.Value);
                    break;
                case PinAction.Blink:
                    Blink(req.PinNumber);
                    break;
            }

            return new Result() { Success = success, Message = message };
        }

        private void AnalogWrite(int pinNumber, int value)
        {
            Dictionary<int, int> pinMap = GetPinMap();
            int wiringPinNum = pinMap[pinNumber];

            WiringPi.Core.PinMode(wiringPinNum, PinMode.Output);

            WiringPi.Core.AnalogWrite(wiringPinNum, value);

        }

        private void DigitalWrite(int pinNumber, int value)
        {
            Dictionary<int, int> pinMap = GetPinMap();
            int wiringPinNum = pinMap[pinNumber];

            WiringPi.Core.PinMode(wiringPinNum, PinMode.Output);

            if(value == 0)
            {
            WiringPi.Core.DigitalWrite(wiringPinNum, DigitalValue.Low);
            }
            else
            {
            WiringPi.Core.DigitalWrite(wiringPinNum, DigitalValue.High);
            }
        }

        private void Blink(int pinNumber)
        {
            Dictionary<int, int> pinMap = GetPinMap();
            int wiringPinNum = pinMap[pinNumber];

            WiringPi.Core.PinMode(wiringPinNum, PinMode.Output);

            Console.WriteLine(string.Format("Blinking Pin {0}...", pinNumber));
            for (int i = 0; i < 10; i++)
            {
            WiringPi.Core.DigitalWrite(wiringPinNum, DigitalValue.High);
            System.Threading.Thread.Sleep(1000);

            WiringPi.Core.DigitalWrite(wiringPinNum, DigitalValue.Low);
            System.Threading.Thread.Sleep(1000);
            }
        }
    }

    public class PinRequest
    {
        public PinAction Action { get; set; }
        public int PinNumber { get; set; }
        public int Value { get; set; }
    }

    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public enum PinAction
    {
        Blink,
        AnalogWrite,
        DigitalWrite
    }
}
