using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.IO;

namespace DalilakApp
{
    public partial class MainPage : ContentPage
    {
        private string loginTxt;
        private Services.DalilakapiService api = new Services.DalilakapiService();

        public MainPage()
        {
            InitializeComponent();

            // Function to disaply one image for Al-Hada
            displayImages();
        }

        private async void btn_login_Clicked(object sender, EventArgs e)
        {
            //loginTxt = await DisplayPromptAsync("Enter your phone number", "+966");
            //Services.DalilakapiService api = new Services.DalilakapiService();
            
            //if(await api.login(loginTxt) == true)
            //{
            //    btn_login.Text = "test";
            //}

        }

        private async void displayImages()
        {
            byte[] Base64Stream = Convert.FromBase64String(await api.image("b279738fb9a444e49c69173a9379c137"));
            img.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
        }
    }
}
