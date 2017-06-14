using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
   /// <summary>
   /// Request object that should be sent with a Get request
   /// </summary>
    public class PinRequest
    {
      public PinAction Action { get; set; }
      public int PinNumber { get; set; }
      public int Value { get; set; }
   }
}
