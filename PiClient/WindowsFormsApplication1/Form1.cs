﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
   public partial class Form1 : Form
   {
      public Form1()
      {
         InitializeComponent();
      }

      private void trackBar1_Scroll(object sender, EventArgs e)
      {
         TrackBar tb = (sender as TrackBar);

      }

      private void trackBar2_Scroll(object sender, EventArgs e)
      {
         throw new NotImplementedException();
      }
   }
}
