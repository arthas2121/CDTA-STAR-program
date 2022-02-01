using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STAR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IDRecovery : ContentPage
    {

        private string FirstName, LastName, Email;

        public IDRecovery()
        {
            InitializeComponent();
        }

        bool ValidateEntry(string entry)
        {
            if (string.IsNullOrEmpty(entry) == true)
            {
                return false;
            }
            return true;
        }

        // Customizable function that calls ValidateEntry to Verify All targeted variables
        bool ValidateAll()
        {
            if (ValidateEntry(FirstName) == false)
            {
                DisplayAlert("Error", "Enter Your First Name", "Go Back");
                return false;
            }
            if (ValidateEntry(LastName) == false)
            {
                DisplayAlert("Error", "Enter Your Last Name", "Go Back");
                return false;
            }
            if (ValidateEntry(Email) == false)
            {
                DisplayAlert("Error", "Enter Your Email Address", "Go Back");
                return false;
            }
            return true;
        }

        bool IDRecovered(string firstName, string lastName, string email)
        {
            string connectionString = "Data Source = beenotified.database.windows.net; Initial Catalog = ParatransitDB; Persist Security Info = True; User ID = BeeNotifiedTeam; Password = iCEN450!";
            string queryString = "SELECT Email, FirstName, LastName, STARID FROM dbo.UserAccounts";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        if (email.Equals("" + reader[0], StringComparison.InvariantCultureIgnoreCase) == true)
                        {

                            if (firstName.Equals("" + reader[1], StringComparison.InvariantCultureIgnoreCase) == true && lastName.Equals("" + reader[2], StringComparison.InvariantCultureIgnoreCase) == true)
                            {

                                DisplayAlert("ID recovered!", "Your STARID is: " + reader[3].ToString(), "Ok");
                                reader.Close();
                                connection.Close();
                                return true;
                            }
                            else
                            {
                                DisplayAlert("Error", "FirstName or LastName not corresponded.", "Ok");
                                reader.Close();
                                connection.Close();
                                return false;
                            }

                        }
                    }

                    DisplayAlert("Error", "Email address not existed.", "Ok");
                    reader.Close();
                    connection.Close();
                    return false;
                }
                catch (Exception ex)
                {
                    DisplayAlert("Submission Error", ex.Message, "Ok");
                }
            }

            return true;

        }

        async void OnRecoveryClicked(object sender, EventArgs e)
        {
            Email = RECOVERY_EMAIL.Text;
            FirstName = RECOVERY_FIRSTNAME.Text;
            LastName = RECOVERY_LASTNAME.Text;

            if (IDRecovered(FirstName, LastName, Email) == false)
            {
                await Navigation.PushAsync(new IDRecovery());
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }

        }
    }
}