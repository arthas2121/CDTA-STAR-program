using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data.SqlClient;

namespace STAR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        string SID, firstname, lastname;
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        // Login button function
        async void OnLoginClicked(object sender, EventArgs e)
        {
            bool found = false;
            SID = LOGIN_STARID.Text;
            lastname = LOGIN_LASTNAME.Text;
            string connectionString = "Data Source = beenotified.database.windows.net; Initial Catalog = ParatransitDB; Persist Security Info = True; User ID = BeeNotifiedTeam; Password = iCEN450!";
            string queryString = "SELECT STARID, FirstName, LastName FROM dbo.UserAccounts";
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();  //Runs SQL Query
                                                                     //Process results.  Each iteration is a row of the table
                    while (reader.Read())
                    {
                        if ((SID.Equals("" + reader[0], StringComparison.InvariantCultureIgnoreCase) == true) && 
                            (lastname.Equals("" + reader[2], StringComparison.InvariantCultureIgnoreCase) == true))
                        {
                            found = true;
                            string SID0 = "" + reader[0];
                            string firstname0 = "" + reader[0];
                            string lastname0 = "" + reader[2];
                            Application.Current.Properties["SID"] = SID0;
                            Application.Current.Properties["firstname"] = firstname0;
                            Application.Current.Properties["lastname"] = lastname0;
                            break;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    //Display error as mobile pop-up with error message upon failure
                    await DisplayAlert("Submission Error", ex.Message, "Ok");
                }
            }
            if(found)
            {
                Application.Current.Properties["IsLoggedIn"] = Boolean.TrueString;
                //await DisplayAlert("Login Success", "Account found - Redirecting to front page", "Ok");
                await Navigation.PushAsync(new FrontPage(SID, lastname));
            }
            else
            {
                await DisplayAlert("Login Failed", "Account Not Found - Returning to Login Page","Ok");
            }
        }

        // Create account button function
        async void OnCreateClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registration());
        }

        async void OnRecoveryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IDRecovery());
        }
    }
}