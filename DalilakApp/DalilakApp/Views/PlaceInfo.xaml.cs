using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DalilakApp.Models.Bindings;

namespace DalilakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceInfo : ContentPage
    {
        private Services.DalilakapiService api = new Services.DalilakapiService();
        string id = null;
        private List<CommentForm> CommentForm = new List<CommentForm>();
        public PlaceInfo(string id)
        {
            InitializeComponent();
            this.id = id;
            DisplayInfo();

        }
        private async void DisplayInfo()
        {
            var image = await api.image(id);
            img.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(image)));

            var placeName = await api.GetPlace(id);
            var placeDescription = await api.GetPlace(id);
            var placeLoaction = await api.GetPlace(id);

            this.placeName.Text = placeName.name;
            this.placeDescription.Text = placeDescription.description;
            this.placeLoaction.Text = placeLoaction.location;


            var rs = await api.GetComments(id);
            foreach (var r in rs)
            {
                //i.comment+i.time+i.date
                foreach (var i in r.reviews)
                {
                    CommentForm.Add(new CommentForm() { Name = r.user_id,Comment=i.comment,DateTime = i.time + " " + i.date});

                }
                
            }
            BindableLayout.SetItemsSource(SCommentForms, CommentForm);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            SCommentForms.IsVisible = true;
            btn_comment.IsVisible = false;
        }
    }
}