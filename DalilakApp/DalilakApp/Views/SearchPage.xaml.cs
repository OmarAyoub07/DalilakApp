using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using DalilakApp.Models.Binding;

namespace DalilakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        private Services.DalilakapiService api = new Services.DalilakapiService();
        private List<Place> places = new List<Place>();
        //private List<ImageButton> imgs = new List<ImageButton>();
        public ObservableCollection<ImgsBind> imgs = new ObservableCollection<ImgsBind>();
      

    public SearchPage(string cityId, string Placetype)
        {
            InitializeComponent();
            getPlaces(cityId, Placetype);

        }

        private async void getPlaces(string cityId, string Placetype)
        {
            this.places = await api.GetPlaces(cityId, Placetype);
            int count = 0;
            foreach (var place in places)
            {
                result.Text += place.name + "\n";
                byte[] Base64Stream = Convert.FromBase64String(await api.image(place.id));
                imgs.Add(new ImgsBind()
                {
                    source = ImageSource.FromStream(() => new MemoryStream(Base64Stream)),
                    index = count
                });
                count++;
            }
            
        }


    }
}