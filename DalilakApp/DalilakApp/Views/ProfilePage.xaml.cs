using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace DalilakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private Services.DalilakapiService api = new Services.DalilakapiService();
        private byte[] avatarImg = null;
        private string placeImg = "";

        public ProfilePage()
        {
            InitializeComponent();
            if (App.user.user_type == true)
            {
                btn_Guider.Text = "Add new place, Click here!";
            }
            else
                btn_Guider.Text = "Post Guider CV ?";

                displayImage();
            SetLables();
        }
        private async void displayImage()
        {
            byte[] Base64Stream = Convert.FromBase64String(await api.getProfileImage(App.user.id)); 
            //img.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
        }

        private void btn_favorit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RecordPage(App.user.id,true));
        }
        private void btn_history_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RecordPage(App.user.id));
        }

        private async void btn_ok_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(Eage.Text, out int n))
            {
                Boolean state = await api.UpdateUser(App.user.id, Ename.Text, Convert.ToInt32(this.Eage.Text), Einformation.Text, Ecity.Text);
                if(avatarImg != null)
                    state = await api.PostProfileImg(App.user.id, avatarImg);

                if (state)
                {
                    await DisplayAlert("Note", " The data has been modified  ", "OK");
                    btn_edit.IsVisible = true;
                    btn_history.IsVisible = true;
                    btn_favorit.IsVisible = true;
                    noEdit.IsVisible = true;
                    btn_ok.IsVisible = false;
                    Edit.IsVisible = false;
                    App.user = await api.getUser(App.user.id);
                    SetLables();
                    displayImage();
                }
                else
                {
                    await DisplayAlert("ERROR", " Please verify the data entered ", "OK");
                }
            }
            else
                await DisplayAlert("ERROR", " Please verify the data entered ", "OK"); 
        }
        private void btn_edit_Clicked(object sender, EventArgs e)
        {
            noEdit.IsVisible = false;
            Edit.IsVisible = true;
            Ename.Placeholder = App.user.name;
            Eage.Placeholder += App.user.age;
            Einformation.Placeholder = App.user.information;
            Ecity.Placeholder = App.user.city_id;
            btn_edit.IsVisible = false;
            btn_history.IsVisible = false;
            btn_favorit.IsVisible = false;
            btn_ok.IsVisible = true;
            btn_Img.IsVisible = true;

            /*
            Boolean state = await api.UpdateUser(user.id, name.Text, Convert.ToInt32(this.age.Text), information.Text, city.Text);
            if (state)
            {
                await DisplayAlert("Note", " The data has been modified  ", "OK");
            }else
                await DisplayAlert("ERROR", " Please verify the data entered ", "OK");
            */

        }

        private async void btn_Img_Clicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync();
            if (result != null)
            {
                Stream stream = await result.OpenReadAsync();
                BinaryReader b_reader = new BinaryReader(stream);

                avatarImg = b_reader.ReadBytes((Int32)stream.Length);
            }
        }

        private async void btn_Guider_Clicked(object sender, EventArgs e)
        {
            if (App.user.user_type == false)
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    Stream stream = await result.OpenReadAsync();
                    BinaryReader b_reader = new BinaryReader(stream);

                    byte[] file = b_reader.ReadBytes((Int32)stream.Length);

                    bool isPosted = await api.PostCV(App.user.id, file);
                    if (isPosted)
                        await DisplayAlert("Note", "The CV has been Posted", "OK");
                    else
                        await DisplayAlert("Note", "CV has not been posted", "OK");
                }
            }
            else
            {
                form_addPlace.IsVisible = true;
                form_ViewInfo.IsVisible = false;
            }
        }

        private async void SetLables()
        {
            email.Text = App.user.email;
            phone_num.Text =App.user.phone_num;
            name.Text = App.user.name;
            age.Text = Convert.ToString(App.user.age);
            information.Text =  App.user.information;

            if (App.user.city_id.Length == 32)
                city.Text = await api.getCityName(App.user.city_id);
            else
                city.Text = " ";
            
        }

        private async void btn_addImagePlace_Clicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync();
            if (result != null)
            {
                btn_addImg.Text = result.FileName;

                Stream stream = await result.OpenReadAsync();
                BinaryReader b_reader = new BinaryReader(stream);

                byte[] Img = b_reader.ReadBytes((Int32)stream.Length);
                placeImg = Convert.ToBase64String(Img);
            }
        }

        private void btn_Cancel_Clicked(object sender, EventArgs e)
        {
            form_addPlace.IsVisible = false;

            form_ViewInfo.IsVisible = true;

        }

        private async void btn_add_Clicked(object sender, EventArgs e)
        {
            bool isFullInfo = false;
            isFullInfo = rdbtn_Nat.IsChecked | rdbtn_his.IsChecked | rdbtn_vnt.IsChecked;
            isFullInfo &= entry_plcName.Text.Length > 0;
            isFullInfo &= enty_plcLoc.Text.Length > 0;
            isFullInfo &= entry_plcDis.Text.Length > 0;
            isFullInfo &= entry_cityName.Text.Length > 0;
            isFullInfo &= placeImg.Length > 100;

            if (isFullInfo)
            {
                string placetype = rdbtn_Nat.IsChecked ? rdbtn_Nat.Value.ToString() : rdbtn_his.IsChecked ? rdbtn_his.Value.ToString() : rdbtn_vnt.Value.ToString();
                string temp_body = entry_plcName.Text+"|"+enty_plcLoc.Text+"|"+entry_plcDis.Text+"|"+placetype+"|"+entry_cityName.Text+"|"+placeImg;
                await api.PostModification(App.user.id, new string[] { temp_body });
                form_addPlace.IsVisible = false;

                form_ViewInfo.IsVisible = true;
                await DisplayAlert("Notify!", " You post new place to the administaros correctly, we will notify you soon by email ", "OK");

            }
            else
                await DisplayAlert("You'r form lacking some information!", " you must fill all fields to add a place to the system ", "OK");
        }
    }
}