using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Models
{
    public class ShowData
    {
        public ShowData()
        {
            
        }
        public string ShowId { get; set; }
        public string ShowName { get; set; }
        public  DateTime Date { get; set; }
        public int SeatAllocation { get; set; }
    }
}
