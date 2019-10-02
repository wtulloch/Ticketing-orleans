using System;
using System.Collections.Generic;

namespace Ticketing.Models
{
    public class ShowInformation
    {
       public string BaseShowId { get; set; }
       public string Name { get; set; }
       public List<DateTime> Dates { get; set; }
       public int SeatingAllocation { get; set; }
    }
}
