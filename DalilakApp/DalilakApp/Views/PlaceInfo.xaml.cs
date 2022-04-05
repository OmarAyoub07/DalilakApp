using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DalilakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceInfo : ContentPage
    {
        private Services.DalilakapiService api = new Services.DalilakapiService();
        string id = null;
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

            var place = await api.GetPlace(id);
            this.place.Text = place.name+"\n"+place.description+"\n"+place.location+"\n\n\n";
            var rs = await api.GetComments(id);
            foreach(var r in rs)
            {
                foreach(var i in r.reviews)
                {
                    rviws.Text += r.user_id+"\t"+i.comment+"\t"+i.time+"\t"+i.date+"\n\n\n";
                }
            }
        }
    }
}