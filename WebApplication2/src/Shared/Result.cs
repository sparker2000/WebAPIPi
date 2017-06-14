using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
   /// <summary>
   /// Result object that will be returned when using a get request.
   /// </summary>
   public class Result
   {
      public bool Success { get; set; }
      public string Message { get; set; }
   }
}
