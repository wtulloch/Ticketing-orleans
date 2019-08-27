using System.Collections.Generic;
using Repositories;

namespace Silo.FakeData
{
    public class FakeShowRepository : IShowRepository
    {
        public List<string> GetShows()
        {
            return new List<string>
            {
                "Show 1",
                "Show 2",
                "Show 3",
                "Show 4"
            };
        }
    }
}
