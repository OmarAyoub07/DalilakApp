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
using DalilakApp.Views;

namespace DalilakApp
{
    public partial class MainPage : ContentPage

    {
        private Services.DalilakapiService api = new Services.DalilakapiService();
        public MainPage()
        {
            InitializeComponent();

            // Function to disaply one image for Al-Hada
            displayImages();
        }

        // This function Executed when the user back to this page 
        protected override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            if (App.user != null)
                loginbtn.Text=App.user.name;
        }

        private async void btn_login_Clicked(object sender, EventArgs e)
        {
            if(App.user == null)
                login();
            else
            {
                // Open profiel
                await Navigation.PushAsync(new ProfilePage());
            }
        }


        private async void displayImages()
        {
            var imgs = await api.adsImage("2502dd29-90b6-11ec-8743-bc64bf92");

            //img1.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imgs[0])));//imgs[0];
            ad_Img.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imgs[1])));
        }

        /* Register function - called during login process in special case */
        private async void Register(string phone)
        {
            string name;
            do
            {
                name = await DisplayPromptAsync("Enter your name: ", "");

                if (name != null && name.Length > 0)
                {
                    string email;
                    bool isPosted = false;
                    do
                    {
                        email = await DisplayPromptAsync("Enter your eamil: ", "email@gmail.com");

                        if (email != null && email.Contains("@"))
                        {
                            isPosted = await api.postUser(name, phone, email);

                            if (isPosted == true)
                            {
                                string result = await api.login(phone);
                                App.user = new User();
                                App.user = await api.getUser(result);
                                loginbtn.Text = App.user.name;
                                break;
                            }
                            else
                                await DisplayAlert("Notify", " You Entered used email ", "OK");
                        }
                        else if (email == null)
                            break;
                        else
                            await DisplayAlert("Notify", " Invalid email format ", "OK");

                    }
                    while (!isPosted);
                    break;
                }
                else if (name == null)
                    break;

                await DisplayAlert("Notify", " You didn't enter a name ", "OK");

            } while (name.Length <= 0);
        }

        /* login function */
        private async void login()
        {
            string loginTxt = await DisplayPromptAsync("Enter your phone number", "+966");

            string verificationTxt;
            int counter = 0;
            string verificationCode = await api.getRNG();

            if (loginTxt != null)
            {
                do
                {
                    verificationTxt = await DisplayPromptAsync("Enter verification code", verificationCode);

                    if (verificationTxt == verificationCode)
                    {
                        string result = await api.login(loginTxt);
                        if (result == "notExist")
                        {
                            Register(loginTxt);
                        }
                        else
                        {
                            App.user = new User();
                            App.user = await api.getUser(result);
                            loginbtn.Text = App.user.name; // test button should be changed later
                        }
                        break;
                    }
                    else if (verificationTxt == null)
                        break;

                    if (counter==3)
                    {
                        await DisplayAlert("Notify", " Please log in again  ", "OK");
                        break;
                    }
                    counter++;
                    await DisplayAlert("Notify", " Invalid verification code ", "OK");
                } while (verificationTxt != verificationCode);
            }
        }

        private async void NPbtn_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            string place_type = button.TabIndex == 0 ? "HIS" : button.TabIndex == 1 ? "NAT" : "VNT";
            string place_nature = button.TabIndex == 0 ? "Historical Places" : button.TabIndex == 1 ? "Natural Places" : "Events";

            await Navigation.PushAsync(new SearchPage(place_type, place_nature));
        }

        private async void btn_schedualing_clicked(object sender, EventArgs e)
        {
            if(App.user == null)
                await DisplayAlert("Notify!", " You must Loin to use this feature ", "OK");
            else
                await Navigation.PushAsync(new SchedulPage());
        }
    }
}
