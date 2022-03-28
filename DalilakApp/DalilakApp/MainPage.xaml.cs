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
        private Services.DalilakapiService api = new Services.DalilakapiService();
        private User user=new User();
        public MainPage()
        {
            InitializeComponent();

            // Function to disaply one image for Al-Hada
            displayImages();
        }

        private async void btn_login_Clicked(object sender, EventArgs e)
        {
            
            string loginTxt = await DisplayPromptAsync("Enter your phone number", "+966");
            string verification = await DisplayPromptAsync("Enter verification code", "0000");

            if (verification == "0000")
            {
                string result = api.login(loginTxt).Result;
                if (result == "notExist")
                {

                }
                else
                {
                    user = api.getUser(result).Result;
                }
                test.Text = user.name;
            }
            else
            {
                 await DisplayAlert("ERROR", " Invalid verification code ", "OK");
            }





        }

        private async void displayImages()
        {
            byte[] Base64Stream = Convert.FromBase64String(await api.image("b279738fb9a444e49c69173a9379c137"));
            img.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
        }
    }
}
