using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.Data.SqlClient;

namespace STAR.Utilities
{
    public class HTTPClientInstance
    {
        public static readonly HttpClient client;
        
        static HTTPClientInstance()
        {
            client = new HttpClient();
        }
    }
}
