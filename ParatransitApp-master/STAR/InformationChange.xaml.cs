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
    public partial class InformationChange : ContentPage

    {

        string ONLINE_SID;

        public InformationChange(string SID0)
        {
            InitializeComponent();
            ONLINE_SID = SID0;
        }




        async void ChangeEmailAddress(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EmailAddressChange(ONLINE_SID));
        }
        async void ChangeHomeAddress(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddressChange(ONLINE_SID));
        }

        async void ChangeContactNumber(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhoneNoChange(ONLINE_SID));
        }




    }
}

