using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
   /// <summary>
   /// Action that can be performed for a post request
   /// </summary>
   public enum PinAction
   {
      Blink,
      AnalogWrite,
      DigitalWrite
   }
}
