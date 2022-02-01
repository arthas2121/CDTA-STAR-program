using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STAR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        private string starID, FirstName, LastName, Email, DoB, HomeAddress, PhoneNumber;

        public Registration()
        {
            InitializeComponent();
        }


        // Function for validating user entries
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
            if (ValidateEntry(starID) == false)
            {
                DisplayAlert("Error", "Enter Your StarID", "Go Back");
                return false;
            }
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
            if (ValidateEntry(DoB) == false)
            {
                DisplayAlert("Error", "Enter a correct Date of Birth", "Go Back");
                return false;
            }
            if (ValidateEntry(HomeAddress) == false)
            {
                DisplayAlert("Error", "Enter Your Home Address", "Go Back");
                return false;
            }
            if (ValidateEntry(PhoneNumber) == false)
            {
                DisplayAlert("Error", "Enter Your Phone Number", "Go Back");
                return false;
            }
            return true;
        }

        async void OnRegisterClicked(object sender, EventArgs e)
        {
            starID = REG_STARID.Text;
            FirstName = REG_FIRSTNAME.Text;
            LastName = REG_LASTNAME.Text;
            Email = REG_EMAIL.Text;
            DoB = REG_DOB.Date.ToShortDateString();
            HomeAddress = REG_ADD.Text;
            PhoneNumber = REG_PHONE.Text;

            // if all information is correct, create user account

            if (ValidateAll() == true)
            {
                string message;

                message = "STAR ID: " + starID
                        + Environment.NewLine + Environment.NewLine
                        + "First Name: " + FirstName
                        + Environment.NewLine + Environment.NewLine
                        + "Last Name: " + LastName
                        + Environment.NewLine + Environment.NewLine
                        + "Email Address: " + Email
                        + Environment.NewLine + Environment.NewLine
                        + "Date of Birth: " + DoB
                        + Environment.NewLine + Environment.NewLine
                        + "Phone Number: " + PhoneNumber
                        + Environment.NewLine + Environment.NewLine
                        + "Home Address: " + HomeAddress;


                bool answer = await DisplayAlert("Check Register Information: ", message, "Confirm", "Cancel");

                if (answer)
                {

                    string connectionString = "Data Source = beenotified.database.windows.net; Initial Catalog = ParatransitDB; Persist Security Info = True; User ID = BeeNotifiedTeam; Password = iCEN450!";
                    string queryString = "INSERT INTO dbo.UserAccounts (STARID, FirstName, LastName, Email, DoB, HomeAddress, PhoneNumber) VALUES ('" + starID + "', '" + FirstName + "', '" + LastName + "','" + Email + "' , '" + DoB + "', '" + HomeAddress + "', '" + PhoneNumber + "')";
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
                            await DisplayAlert("SUCCESS", "You Successfully Registrated", "Ok");
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
                }
                //Navigate back to LoginPage once account is created
                await Navigation.PushAsync(new LoginPage());
            }
        }
    }
}