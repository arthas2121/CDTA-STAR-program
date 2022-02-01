using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STAR
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            bool isLoggedIn = Current.Properties.ContainsKey("IsLoggedIn") ? Convert.ToBoolean(Current.Properties["IsLoggedIn"]) : false;
            if (!isLoggedIn)
            {
                //Load if Not Logged In
                MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = Color.FromHex("#002b55"),
                    BarTextColor = Color.Gold,

                };
            }
            else
            {
                string SID = Application.Current.Properties["SID"] as string;
                string lastname = Application.Current.Properties["lastname"] as string;
                MainPage = new NavigationPage(new FrontPage(SID,lastname))
                {
                    BarBackgroundColor = Color.FromHex("#002b55"),
                    BarTextColor = Color.Gold,
                };

            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
