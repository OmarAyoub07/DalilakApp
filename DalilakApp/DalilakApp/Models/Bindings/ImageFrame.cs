using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DalilakApp.Models.Bindings
{
    public class ImageFrame
    {
        public string name { get; set; }

        public ImageSource source { get; set; }

        public int index { get; set; }

        public ImageFrame(string n, ImageSource s, int i)
        {
            name = n;
            source = s;
            index = i;
        }
    }
}
