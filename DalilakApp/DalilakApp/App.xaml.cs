using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DalilakApp
{
    public partial class App : Application
    {
        public static User user { get; set; }
        public static string cityID { get; set; }

        public static string cityName { get; set; }
        public App()
        {
            InitializeComponent();

            // The root page of your application
            user = null;
            cityID = "2502dd29-90b6-11ec-8743-bc64bf92";// it must be null and detected when user enter the system;
            cityName = "Taif";                                         // we use Taif city as testing sample
            MainPage = new NavigationPage(new TabbedPageMain());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
