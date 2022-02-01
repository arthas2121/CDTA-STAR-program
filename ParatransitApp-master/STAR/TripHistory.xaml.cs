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
using Xamarin.Forms.Internals;

namespace STAR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripHistory : ContentPage
    {
        public IList<Triplist> Tripinfo { get; private set; }
        public TripHistory()
        {
            InitializeComponent();
            Tripinfo = new List<Triplist>();
            RetrieveTripInfo();
            Triplistview.ItemsSource = Tripinfo;
            BindingContext = this;
        }

        public void RetrieveTripInfo()
        {

            string completed = "Completed";
            string connectionString = "Data Source = beenotified.database.windows.net; Initial Catalog = ParatransitDB; Persist Security Info = True; User ID = BeeNotifiedTeam; Password = iCEN450!";
            string queryString = "SELECT STARID, Confirmation, TripID, PUAddress, PUTime, DOAddress FROM dbo.TripInfo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string reader1;
                string SID = Application.Current.Properties["SID"] as string;
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        reader1 = reader[1].ToString();
                        if (SID.Equals("" + reader[0], StringComparison.InvariantCultureIgnoreCase) == true)
                        {
                            if (completed.Equals("" + reader1.Trim(' '), StringComparison.InvariantCultureIgnoreCase) == true)
                            {
                                Tripinfo.Add(new Triplist
                                {
                                    Pickupaddress = "" + reader[3],
                                    PickupTime = "" + reader[4],
                                    Destination = "" + reader[5],
                                    TripID = "" + reader[2]

                                });
                            }
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
            }
        }

        void OnRepeat(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                Triplistview.ItemsSource = Tripinfo;
            }
            else
            {
                Triplistview.ItemsSource = Tripinfo.Where(x => x.Pickupaddress.ToLower().Contains(e.NewTextValue.ToLower()) || x.Destination.ToLower().Contains(e.NewTextValue.ToLower()));
            }
        }
    }
}