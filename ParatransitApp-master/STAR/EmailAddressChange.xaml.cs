using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data.SqlClient;
using STAR.Utilities;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace STAR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailAddressChange : ContentPage
    {
        private string Email, DSTARID;
        string ONLINE_SID;
        public EmailAddressChange(string O_SID)
        {
            InitializeComponent();
            ONLINE_SID = O_SID;
        }


        bool ValidateEntry(string entry)
        {
            if (string.IsNullOrEmpty(entry) == true)
            {
                return false;
            }
            return true;
        }

        bool ValidateALL()
        {
            if (ValidateEntry(Email) == false)
            {
                DisplayAlert("Error", "Enter Your Email Address", "Go Back");
                return false;
            }
            return true;
        }

        async void ChangeEmialAddressClicked(object sender, EventArgs e)
        {
            Email = CHA_EMAIL.Text;
            DSTARID = ONLINE_SID;

            if (ValidateALL() == true)
            {
                string connectionString = "Data Source = beenotified.database.windows.net; Initial Catalog = ParatransitDB; Persist Security Info = True; User ID = BeeNotifiedTeam; Password = iCEN450!";
                string queryString = "UPDATE dbo.UserAccounts SET Email = ('" + Email + "') WHERE STARID = ('" + DSTARID + "')";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create the Command and Parameter objects.
                    SqlCommand command = new SqlCommand(queryString, connection);
                    // Open the connection in a try/catch block.
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        //Below displays a dialog pop-up window on the phone indicating success
                        await DisplayAlert("SUCCESS", "Your Email has been changed", "Ok");
                    }
                    catch (Exception ex)
                    {
                        //If an error occurred, the error message will be displayed to the 
                        await DisplayAlert("Submission Error", ex.Message, "Ok");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                _ = Application.Current.MainPage.Navigation.PopAsync();
            }
        }
    }
}
