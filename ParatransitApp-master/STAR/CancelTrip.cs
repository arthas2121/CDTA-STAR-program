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
    public class CancelTrip : ContentPage
    {
        public CancelTrip(string TPID)
        {
            string connectionString = "Data Source = beenotified.database.windows.net; Initial Catalog = ParatransitDB; Persist Security Info = True; User ID = BeeNotifiedTeam; Password = iCEN450!";
            string queryString = "UPDATE dbo.TripInfo SET Confirmation = ('Canceled') WHERE TripId = ('" + TPID + "')";
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
                    DisplayAlert("SUCCESS", "You Trip has been canceled", "Ok");
                }
                catch (Exception ex)
                {
                    //If an error occurred, the error message will be displayed to the 
                    DisplayAlert("Submission Error", ex.Message, "Ok");
                }
                finally
                {
                    connection.Close();
                }
                Navigation.PushAsync(new PendingTripPage());
            }
            _ = Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}