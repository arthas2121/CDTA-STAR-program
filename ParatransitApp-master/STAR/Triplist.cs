using System;
using System.Collections.Generic;
using System.Text;


namespace STAR
{
    public class Triplist
    {
        public static IList<Triplist> ItemsSource { get; internal set; }
        public string Pickupaddress { get; set; }
        public string Destination { get; set; }
        public string PickupTime { get; set; }

        public bool IsReturn { get; set; }  // if Return = 1, if not = 0
        public string Returnaddress { get; set; }
        public string Returntime { get; set; }
        public string IsComment { get; set; }

        public string TripID { get; set; }
        public bool Status { get; set; } // comfirmed = 1 , pending = 0
        
       

        public override string ToString()
        {
            return TripID;
        }
    }
}
