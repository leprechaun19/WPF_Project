using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Classes
{
    class ListItem
    {
        public string Path { get; set; }
        public string Room_code { get; set; }
        public string Category_name { get; set; }
        public string NumberOfSeats { get; set; }
        public string Servicess { get; set; }
        public decimal Cost { get; set; }
        public decimal AndCost { get; set; }
    }
}
