using System;
using System.Collections.Generic;

namespace Ticketing.Models
{
    public class ShowInformation : IEquatable<ShowInformation>
    {
       public string BaseShowId { get; set; }
       public string Name { get; set; }
       public List<string> Dates { get; set; }
       public int SeatingAllocation { get; set; }

       public override bool Equals(object obj)
       {
           if (ReferenceEquals(null, obj)) return false;
           if (ReferenceEquals(this, obj)) return true;
           if (obj.GetType() != this.GetType()) return false;
           return Equals((ShowInformation) obj);
       }

       public bool Equals(ShowInformation other)
       {
           if (ReferenceEquals(null, other)) return false;
           if (ReferenceEquals(this, other)) return true;
           return BaseShowId == other.BaseShowId && Name == other.Name && Equals(Dates, other.Dates) && SeatingAllocation == other.SeatingAllocation;
       }

       public override int GetHashCode()
       {
           unchecked
           {
               var hashCode = (BaseShowId != null ? BaseShowId.GetHashCode() : 0);
               hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
               hashCode = (hashCode * 397) ^ (Dates != null ? Dates.GetHashCode() : 0);
               hashCode = (hashCode * 397) ^ SeatingAllocation;
               return hashCode;
           }
       }
    }
    
}
