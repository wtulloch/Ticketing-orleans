using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Models
{
    public class ShowInfo
    {
        public ShowInfo()
        {
            
        }
        public string BaseShowId { get; set; }
        public string ShowName { get; set; }
        public  DateTime Date { get; set; }
        public int SeatsAvailable { get; set; }
    }
}
