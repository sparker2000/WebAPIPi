using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebApplication2.Controllers
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

         Random rand = new Random();
         foreach (int key in pinMap.Keys)
         {
            int val = rand.Next(0, 255);
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
         return new Result()
         {
            Success = true,
            Message = "  Type: Raspberry Pi 3 Model B, Revision: 1.5, Memory: 32MB, Maker: Unknown"
         };
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

         Random rand = new Random();
         int val = rand.Next(0, 255);
         return new Result() { Success = true, Message = val.ToString() };
      }
      
      // POST api/values
      [HttpPost]
      public Result Post([FromBody]PinRequest req)
      {
         bool success = true;
         string message = "complete";

         Dictionary<int, int> pinMap = GetPinMap();

         if (!pinMap.ContainsKey(req.PinNumber))
         {
            return new Result() { Success = false, Message = string.Format("Pin {0} is invalid for WiringPi.", req.PinNumber) };
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

         Console.WriteLine("AnalogWriting to {0} value {1}", wiringPinNum, value);

      }

      private void DigitalWrite(int pinNumber, int value)
      {
         Dictionary<int, int> pinMap = GetPinMap();
         int wiringPinNum = pinMap[pinNumber];

         if (value == 0)
         {
            Console.WriteLine("DigitalWriting to {0} value {1}", wiringPinNum, 0);
         }
         else
         {
            Console.WriteLine("DigitalWriting to {0} value {1}", wiringPinNum, 1);
         }
      }

      private void Blink(int pinNumber)
      {
         Dictionary<int, int> pinMap = GetPinMap();
         int wiringPinNum = pinMap[pinNumber];
         
         for (int i = 0; i < 10; i++)
         {
            Console.WriteLine("Blinking pin {0}", pinNumber);
            System.Threading.Thread.Sleep(1000);
         }
      }
   }
}
