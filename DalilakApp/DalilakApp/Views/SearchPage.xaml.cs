using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DalilakApp.Models.Bindings;

namespace DalilakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        private Services.DalilakapiService api = new Services.DalilakapiService();
        private List<Place> places = new List<Place>();
        private List<ImageFrame> imgFrames = new List<ImageFrame>();

        public SearchPage()
        {
            InitializeComponent();
        }
        public SearchPage(string Placetype, string placeNature)
        {
            InitializeComponent();
            Title = placeNature;
            DisplayPlaces(App.cityID, Placetype);
        }

        private async void btn_images_clicked(object sender, EventArgs e)
        {
            var b = sender as ImageButton;
            //await DisplayAlert("Notify", places[b.TabIndex].id, "OK");
            await Navigation.PushAsync(new PlaceInfo(places[b.TabIndex].id));

        }

        private async void DisplayPlaces(string cityId, string Placetype)
        {
            // get places from api (store them in list)
            this.places = await api.GetPlaces(cityId, Placetype);

            int counter = 0;
            foreach (var place in places)
            {
                // convert string to array of bytes
                byte[] Base64Streams =   Convert.FromBase64String(await api.image(place.id));

                // Add binded information (to view them in xaml view)
                imgFrames.Add(new ImageFrame(place.name, ImageSource.FromStream(() => new MemoryStream(Base64Streams)), counter));

                counter++;
            }


            BindableLayout.SetItemsSource(imagesStackLayout, imgFrames);
        }
    }
}