using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DalilakApp
{
    public partial class App : Application
    {
        public static User user { get; set; }
        public App()
        {
            InitializeComponent();

            // The root page of your application
            user = null;
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
