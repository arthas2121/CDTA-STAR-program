using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using STAR.Utilities;

namespace STAR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTripPage : ContentPage
    {
        private string DPUAddress, DPUDate, DPUTime, DDOAddress, DReturnAddress, DReturnDate, DReturnTime, DPCA, DTicket, DComments, DSTARID, DConfirmation, DTripID;

        Dictionary<string, string> TableInfo = new Dictionary<string, string>();
        string ONLINE_SID;
        public NewTripPage(string O_SID)
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

        bool ValidateAll()
        {
            if (ValidateEntry(DPUAddress) == false)
            {
                DisplayAlert("Error", "Enter Pick Up Address", "Go Back");
                return false;
            }
            if (ValidateEntry(DPUDate) == false)
            {
                DisplayAlert("Error", "Enter Pick Up Date", "Go Back");
                return false;
            }
            if (ValidateEntry(DPUTime) == false)
            {
                DisplayAlert("Error", "Enter Pick Up Time", "Go Back");
                return false;
            }
            if (ValidateEntry(DDOAddress) == false)
            {
                DisplayAlert("Error", "Enter Destination Address", "Go Back");
                return false;
            }
            return true;
        }

        string IDRandom()
        {
            string ID;
            var random = new Random();
            var num = random.Next(10000, 100000);
            ID = num.ToString();
            return ID;
        }

        bool exist(string ID)
        {
            string connectionString = "Data Source = beenotified.database.windows.net; Initial Catalog = ParatransitDB; Persist Security Info = True; User ID = ****; Password = ***
            string queryString = "SELECT TripID FROM dbo.TripInfo";

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
                        if (ID.Equals("" + reader[0], StringComparison.InvariantCultureIgnoreCase) == true)
                        {
                            reader.Close();
                            connection.Close();
                            return true;
                        }
                    }
                    reader.Close();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    //Display error as mobile pop-up with error message upon failure
                    DisplayAlert("Submission Error", ex.Message, "Ok");
                }
            }

            return false;
        }

        string homeaddress(string ID)
        {
            string homeaddress = "wrong";

            string connectionString = "Data Source = beenotified.database.windows.net; Initial Catalog = ParatransitDB; Persist Security Info = True; User ID = BeeNotifiedTeam; Password = iCEN450!";
            string queryString = "SELECT STARID, HomeAddress FROM dbo.UserAccounts";

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
                        if (ID.Equals("" + reader[0], StringComparison.InvariantCultureIgnoreCase) == true)
                        {
                            homeaddress = "" + reader[1];
                            reader.Close();
                            connection.Close();
                            return homeaddress;
                        }
                    }
                    reader.Close();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    //Display error as mobile pop-up with error message upon failure
                    DisplayAlert("Submission Error", ex.Message, "Ok");
                }
            }

            return homeaddress;
        }

        async void OnConfirmedAsync(object sender, EventArgs e)
        {
                DPUDate = PickupDateSelector.Date.ToShortDateString();
                DPUTime = PickupTimeSelector.Time.ToString();
                DDOAddress = DestAddress.Text;

                DComments = Comments.Text;
                DSTARID = ONLINE_SID;
                DConfirmation = "Pending";
                DTripID = IDRandom();

                if (HomePickUp.IsChecked)
                {
                    DPUAddress = homeaddress(DSTARID);
                }
                else
                {
                    DPUAddress = CurrAddress.Text;
                }

                if (ReturnNeed.IsChecked)
                {
                    if (SameReturn.IsChecked)
                    {
                        DReturnAddress = DPUAddress;
                    }
                    else
                    {
                        DReturnAddress = ReturnAddress.Text;
                    }
                    DReturnDate = ReturnDatePicked.Date.ToShortDateString();
                    DReturnTime = ReturnTimePicked.Time.ToString();
                }
                else
                {
                    DReturnAddress = "Null";
                    DReturnDate = "Null";
                    DReturnTime = "Null";
                }

                if (PCA.IsToggled)
                {
                    DPCA = "True";
                }
                else
                {
                    DPCA = "False";
                }
                if (Ticket.IsToggled)
                {
                    DTicket = "True";
                }
                else
                {
                    DTicket = "False";
                }

                while (exist(DTripID) == true)
                {
                    DTripID = IDRandom();
                }

            if (ValidateAll() == true)
            {
                string temp = PickupDateSelector.Date.ToShortDateString() + " - " + PickupTimeSelector.Time.ToString();
                string Rtemp = ReturnDatePicked.Date.ToShortDateString() + " - " + ReturnTimePicked.Time.ToString();
                string message;
                message = "Pick up: " + DPUAddress
                        + Environment.NewLine + Environment.NewLine
                        + "Drop off: " + DDOAddress
                        + Environment.NewLine + Environment.NewLine
                        + "Date: " + temp
                        + Environment.NewLine + Environment.NewLine
                        + "Return: " + DReturnAddress
                        + Environment.NewLine + Environment.NewLine
                        + "Additional Instructions: ";

                bool answer = await DisplayAlert("Verify Information:", message, "Confirm", "Cancel");

                if (answer)
                {
                    string connectionString = "Data Source = beenotified.database.windows.net; Initial Catalog = ParatransitDB; Persist Security Info = True; User ID = BeeNotifiedTeam; Password = iCEN450!";
                    string queryString = "INSERT INTO dbo.TripInfo (PUAddress, PUDate, PUTime, DOAddress, ReturnAddress, ReturnDate, ReturnTime, PCA, Ticket, Comments, STARID, Confirmation, TripID) VALUES ('" + DPUAddress + "'," +
                    " '" + DPUDate + "','" + DPUTime + "', '" + DDOAddress + "','" + DReturnAddress + "' , '" + DReturnDate + "', '" + DReturnTime + "', '" + DPCA + "', '" + DTicket + "', '" + DComments + "', '" + DSTARID + "', '" + DConfirmation + "', '" + DTripID + "')";
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
                            await DisplayAlert("SUCCESS", "You successfully create the trip", "Ok");
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
}
