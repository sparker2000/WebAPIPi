using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiClient
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);

         ConnectionForm frm = new ConnectionForm();
         if(frm.ShowDialog() == DialogResult.OK)
         {
            Application.Run(new Form1(frm.ConnectionString));
         }
      }
   }
}
