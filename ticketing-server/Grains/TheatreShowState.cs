using System.Collections.Generic;
using Ticketing.Models;

namespace Grains
{
    public class TheatreShowState
    {
        public HashSet<ShowInformation> ShowInformation { get; set; } = new HashSet<ShowInformation>();
    }
}