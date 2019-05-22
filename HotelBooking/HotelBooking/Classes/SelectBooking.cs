using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Classes
{
    class SelectBooking
    {
        public int Booking_number { get; set; }
        public string Room_code { get; set; }
        public string Full_name { get; set; }
        public string TypeOfFood_name { get; set; }
        public int Payment_number { get; set; }
        public DateTime DateOfBooking { get; set; }
        public DateTime Arrivall { get; set; }
        public DateTime Departuree { get; set; }
        public decimal Sum { get; set; }
    }
}
