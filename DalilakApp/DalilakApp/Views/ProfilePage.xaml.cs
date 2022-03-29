using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DalilakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private Services.DalilakapiService api = new Services.DalilakapiService();
        private User user = new User();
        public ProfilePage(User u)
        {
            InitializeComponent();
            user =u;
            displayImages();

            x.Text = user.name;
            x.Text += "\n" + user.email;
            x.Text += "\n" + user.phone_num;
            x.Text += "\n" + user.age;
            x.Text += "\n" + user.information;
            phone_num.Placeholder = user.phone_num;
        }
        private async void displayImages()
        {
            byte[] Base64Stream = Convert.FromBase64String(await api.getProfileImage(user.id));
            img.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
        }

        private void btn_history_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RecordPage(user.id));
        }
    }
}