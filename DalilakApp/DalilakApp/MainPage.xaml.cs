using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;


namespace DalilakApp
{
    public partial class MainPage : ContentPage
    {
        private string loginTxt;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btn_login_Clicked(object sender, EventArgs e)
        {
            loginTxt = await DisplayPromptAsync("Enter your phone number", "+966");
            Services.DalilakapiService api = new Services.DalilakapiService();
            
            if(await api.login(loginTxt) == true)
            {
                btn_login.Text = "test";
            }

        }
    }
}
