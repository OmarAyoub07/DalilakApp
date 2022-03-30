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
            displayImage();

            
            email.Text = user.email;
            phone_num.Text =user.phone_num;
            name.Placeholder = user.name;
            age.Text +=  user.age;
            information.Placeholder =  user.information;
            city.Placeholder = user.city_id;
        }
        private async void displayImage()
        {
            byte[] Base64Stream = Convert.FromBase64String(await api.getProfileImage(user.id));
            img.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
        }

        private void btn_favorit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RecordPage(user.id,true));
        }
        private void btn_history_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RecordPage(user.id));
        }

        private async void btn_edit_Clicked(object sender, EventArgs e)
        {
            Boolean state = await api.UpdateUser(user.id, name.Text, Convert.ToInt32(this.age.Text), information.Text, city.Text);
            if (state)
            {
                await DisplayAlert("Note", " The data has been modified  ", "OK");
            }else
                await DisplayAlert("ERROR", " Please verify the data entered ", "OK");
        }
    }
}