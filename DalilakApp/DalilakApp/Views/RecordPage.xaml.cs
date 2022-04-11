using DalilakAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DalilakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordPage : ContentPage
    {

        private Services.DalilakapiService api = new Services.DalilakapiService();
        public RecordPage()
        {
            InitializeComponent();
        }
        public RecordPage(string id)
        {
            InitializeComponent();
            display(id);
        }
        public RecordPage(string id,Boolean s)
        {
            InitializeComponent();
            displayFavorite(id);

        }
        private async void displayFavorite(string id)
        {
            History hist = await api.getRecord(id);
            foreach (var record in hist.records)
            {
                if(record.favorite)
                {
                    x.Text += record.place_id + " " + record.rate + " " +  "\n";
                }
                
            }
        }
        private async void display(string id)
        {
            History hist= await api.getRecord(id);
            foreach (var record in hist.records) 
            {
                x.Text += record.place_id + " " + record.rate +" "+ record.favorite + "\n";
            }


        }
        
    }
}