using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Repositories;
using Repositories.Models;

namespace Silo.FakeData
{
    public class FakeShowRepository : IShowRepository
    {
        public List<ShowInfo> GetShows()
        {
            return new List<ShowInfo>
            {
                new ShowInfo
                {
                    BaseShowId = "show1",
                    ShowName = "Notes from the underground",
                    Date =  DateTime.Today.AddDays(2),
                    SeatsAvailable = 300
                },
                new ShowInfo
                {
                    BaseShowId = "show2",
                    ShowName = "The Third Policeman",
                    Date = DateTime.Today.AddDays(5),
                    SeatsAvailable = 400
                },
                new ShowInfo
                {
                    BaseShowId = "show3",
                    ShowName = "Fear and loathing in Las Vegas",
                    Date = DateTime.Today.AddDays(3),
                    SeatsAvailable = 250
                },
                new ShowInfo
                {
                    BaseShowId = "show4",
                    ShowName = "The Naked and the Dead",
                    Date = DateTime.Today.AddDays(5),
                    SeatsAvailable = 100
                }
            };
        }
    }

   
}
