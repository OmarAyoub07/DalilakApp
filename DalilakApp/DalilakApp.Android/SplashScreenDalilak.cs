using Android.App;
using Android.OS;


namespace DalilakApp.Droid
{
    [Activity(Label = "SplashScreenDalilak",
        Theme = "@style/splashDalilak",
        MainLauncher =true,NoHistory =true)]
    public class SplashScreenDalilak : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            StartActivity(typeof(MainActivity));
        }
    }
}