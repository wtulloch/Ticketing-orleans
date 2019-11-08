using System.Collections.Generic;
using Ticketing.Models;

namespace Grains
{
   /// <summary>
   /// Used for persisting the state of the TheatreShows grain
   /// </summary>
    public class TheatreShowState
    {
        public HashSet<ShowInformation> ShowInformation { get; set; } = new HashSet<ShowInformation>();
    }
}