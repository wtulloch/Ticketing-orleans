using System;
using System.Collections.Generic;
using Grains;
using Newtonsoft.Json;
using Repositories;
using Ticketing.Models;

namespace Silo.FakeData
{
    public class FakeShowRepository : IShowRepository
    {
        public List<ShowInformation> GetShows()
        {
            return new List<ShowInformation>
            {
                new ShowInformation
                {
                    BaseShowId = "show1",
                    Name = "Notes from the underground",
                    Dates = CreateDates(DateTime.Today.AddDays(2), 3),
                    SeatingAllocation = 300
                },
                new ShowInformation
                {
                    BaseShowId = "show2",
                    Name = "The Third Policeman",
                    Dates = CreateDates(DateTime.Today.AddDays(5), 2),
                    SeatingAllocation = 400
                },
                new ShowInformation
                {
                    BaseShowId = "show3",
                    Name = "Fear and loathing in Las Vegas",
                    Dates = CreateDates(DateTime.Today.AddDays(3), 5),
                    SeatingAllocation = 250
                },
                new ShowInformation
                {
                    BaseShowId = "show4",
                    Name = "The Naked and the Dead",
                    Dates = CreateDates(DateTime.Today.AddDays(4), 8),
                    SeatingAllocation = 100
                }
            };
        }

        private List<string> CreateDates(DateTime startDate, int numberOfShows)
        {
            var dates = new List<string>();
            for (int i = 0; i < numberOfShows; i++)
            {
                dates.Add(startDate.AddDays(i).ToString("dd/MM/yyyy"));
            }

            return dates;
        }
      
    }

   
}
