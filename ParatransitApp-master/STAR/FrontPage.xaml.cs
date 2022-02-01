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
//test
namespace STAR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrontPage : ContentPage
    {
        string Online_SID, Online_firstname, Online_lastname;
        public FrontPage(string SID0, string lastname0)
        {
            InitializeComponent();
            Online_SID = SID0;              // Initialize StarID
            Online_lastname = lastname0;    // Initialize Lastname
            RetrieveAccount();
            RetrieveTripInfo();
        }

        public FrontPage()
        {
        }
        async void OnNewClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTripPage(Online_SID));
        }
        async void OnRepeatClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TripHistory());
        }
        async void OnPendingClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PendingTripPage());
        }
        async void OnUpcomingClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Upcoming());
        }
        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CancelPage());
        }
        async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings(Online_SID, Online_lastname, Online_firstname));
        }
        async void OnCompletedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings(Online_SID, Online_lastname, Online_firstname));
        }
        async void OnDeniedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeniedPage());
        }

        public void RetrieveAccount()
        {
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
                        if ((Online_SID.Equals("" + reader[0], StringComparison.InvariantCultureIgnoreCase) == true) &&
                            (Online_lastname.Equals("" + reader[2], StringComparison.InvariantCultureIgnoreCase) == true))
                        {
                            Online_firstname = "" + reader[1];
                            Application.Current.Properties["firstname"] = Online_firstname;
                            Application.Current.Properties["lastname"] = Online_lastname;
                            break;

                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    DisplayAlert("Submission Error", ex.Message, "Ok");
                }
            }
            Display_FirstName.Text = Online_firstname.ToUpper();
            Display_LastName.Text = Online_lastname.ToUpper();
            Display_ID.Text = Online_SID.ToUpper();
        }

        public void RetrieveTripInfo()
        {
            int CancelCounter = 0, ConfirmedCounter = 0, PendingCounter = 0;
            string cancel = "Canceled"; string confirm = "Confirmed"; string pending = "Pending";
            string connectionString = "Data Source = beenotified.database.windows.net; Initial Catalog = ParatransitDB; Persist Security Info = True; User ID = BeeNotifiedTeam; Password = iCEN450!";
            string queryString = "SELECT STARID, Confirmation, TripID FROM dbo.TripInfo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string reader1;
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        reader1 = reader[1].ToString();
                        if (Online_SID.Equals("" + reader[0], StringComparison.InvariantCultureIgnoreCase) == true)
                        {
                            if (cancel.Equals("" + reader1.Trim(' '), StringComparison.InvariantCultureIgnoreCase) == true)
                            {
                                CancelCounter++;
                            }
                            else if (confirm.Equals("" + reader1.Trim(' '), StringComparison.InvariantCultureIgnoreCase) == true)
                            {
                                ConfirmedCounter++;
                            }
                            else if (pending.Equals("" + reader1.Trim(' '), StringComparison.InvariantCultureIgnoreCase) == true)
                            {
                                PendingCounter++;
                            }
                        }
                        if (CancelCounter >= 10 || (ConfirmedCounter >= 10 && PendingCounter >= 10))
                        {
                            break;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
                label_upcoming.Text = "UPCOMING: " + ConfirmedCounter.ToString();
                label_pending.Text = "PENDING: " + PendingCounter.ToString();
                label_canceled.Text = "CANCELED: " + CancelCounter.ToString();
            }
        }
    }
}