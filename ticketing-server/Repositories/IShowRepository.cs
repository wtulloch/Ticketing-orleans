using System.Collections.Generic;
using Ticketing.Models;

namespace Repositories
{
    public interface IShowRepository
    {
        List<ShowInformation> GetShows();
    }
}

    