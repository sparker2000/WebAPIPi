using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiClient
{
   public partial class Form1 : Form
   {
      private string _baseUrl;

      public Form1()
      {
         InitializeComponent();
      }

      public Form1(string connectionString)
         : this()
      {
         _baseUrl = connectionString;
      }

      private void trackBar_Scroll(object sender, EventArgs e)
      {
         TrackBar tb = (sender as TrackBar);

         var client = new RestClient(new Uri(_baseUrl));
         var request = new RestRequest("api/GPIO", Method.POST);

         PinRequest req = new PinRequest() { Action = PinAction.AnalogWrite, PinNumber = int.Parse(tb.Name.Substring(2)), Value = tb.Value };
         request.AddJsonBody(req);
         client.ExecuteAsync<Result>(request, response =>
         {
            if(!response.Data.Success)
            {
               MessageBox.Show(response.Data.Message, "Error!");
            }
         });
      }

      private void CheckedChanged(object sender, EventArgs e)
      {
         CheckBox cb = (sender as CheckBox);

         var client = new RestClient(new Uri(_baseUrl));
         var request = new RestRequest("api/GPIO", Method.POST);

         PinRequest req = new PinRequest() { Action = PinAction.DigitalWrite, PinNumber = int.Parse(cb.Name.Substring(2)), Value = cb.Checked ? 1 : 0 };
         request.AddJsonBody(req);
         client.ExecuteAsync<Result>(request, response =>
         {
            if (!response.Data.Success)
            {
               MessageBox.Show(response.Data.Message, "Error!");
            }
         });
      }
   }

   /// <summary>
   /// Result object that will be returned when using a get request.
   /// </summary>
   public class Result
   {
      public bool Success { get; set; }
      public string Message { get; set; }
   }

   /// <summary>
   /// Request object that should be sent with a Get request
   /// </summary>
   public class PinRequest
   {
      public PinAction Action { get; set; }
      public int PinNumber { get; set; }
      public int Value { get; set; }
   }

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
