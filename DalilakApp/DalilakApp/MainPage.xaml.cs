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
            string verificationTxt;
            int counter = 0;
            string verificationCode = await api.getRNG();
            do
            {

                verificationTxt = await DisplayPromptAsync("Enter verification code", verificationCode);
                if (verificationTxt == verificationCode)
                {
                    string result = await api.login(loginTxt);
                    if (result == "notExist")
                    {

                    }
                    else
                    {
                        user = await api.getUser(result);
                    }
                    test.Text = user.name;
                    break;
                }

                if (counter==3)
                {
                    await DisplayAlert("Notify", " Please log in again  ", "OK");
                    break;
                }
                counter++;
                await DisplayAlert("Notify", " Invalid verification code ", "OK");
            } while (verificationTxt != verificationCode);
           
              
            
           





        }

        private async void displayImages()
        {
            byte[] Base64Stream = Convert.FromBase64String(await api.image("b279738fb9a444e49c69173a9379c137"));
            img.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
        }
    }
}
