using System;
using System.Collections.Generic;
using System.Text;

namespace STAR
{
    public class TripObject
    {
        public string PUAddress { get; set; } = "";
        public string PUDate { get; set; } = "";
        public string tempPUTime { get; set; } = "";
        public TimeSpan PUTime { get; set; }
        public string DOAddress { get; set; } = "";
        public string Return { get; set; } = "";
        public string ReturnAddress { get; set; } = "";
        public string ReturnDate { get; set; } = "";
        public string tempReturnTime { get; set; } = "";
        public TimeSpan ReturnTime { get; set; }
        public string PCA { get; set; } = "";
        public string Ticket { get; set; } = "";
        public string Comments { get; set; } = "";
        public string STARID { get; set; } = "";
        public string Confirmation { get; set; } = "";
        public string TripID { get; set; } = "";
    }
}
