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
            name.Text = user.name;
            age.Text = Convert.ToString(this.user.age) ;
            information.Text =  user.information;
            city.Text = user.city_id;
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

        private async void btn_ok_Clicked(object sender, EventArgs e)
        {
             

            if (int.TryParse(Eage.Text, out int n))
            {
                Boolean state = await api.UpdateUser(user.id, Ename.Text, Convert.ToInt32(this.Eage.Text), Einformation.Text, Ecity.Text);
                if (state)
                {
                    await DisplayAlert("Note", " The data has been modified  ", "OK");
                    btn_edit.IsVisible = true;
                    btn_history.IsVisible = true;
                    btn_favorit.IsVisible = true;
                    noEdit.IsVisible = true;
                    btn_ok.IsVisible = false;
                    Edit.IsVisible = false;
                }
                else
                {
                    await DisplayAlert("ERROR", " Please verify the data entered ", "OK");
                }
            }
            else
                await DisplayAlert("ERROR", " Please verify the data entered ", "OK");
            


            
        }
        private  void btn_edit_Clicked(object sender, EventArgs e)
        {
            noEdit.IsVisible = false;
            Edit.IsVisible = true;
            Ename.Placeholder = user.name;
            Eage.Placeholder += user.age;
            Einformation.Placeholder = user.information;
            Ecity.Placeholder = user.city_id;
            btn_edit.IsVisible = false;
            btn_history.IsVisible = false;
            btn_favorit.IsVisible = false;
            btn_ok.IsVisible = true;

            /*
            Boolean state = await api.UpdateUser(user.id, name.Text, Convert.ToInt32(this.age.Text), information.Text, city.Text);
            if (state)
            {
                await DisplayAlert("Note", " The data has been modified  ", "OK");
            }else
                await DisplayAlert("ERROR", " Please verify the data entered ", "OK");
            */

        }
    }
}