using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DalilakApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLogup : ContentPage
    {
        public PageLogup()
        {
            InitializeComponent();
        }
        private async void btn_logup_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("HELLO", " The account has been added ", "OK");
        }

    }
}