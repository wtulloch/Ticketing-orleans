using System.Collections.Generic;
using Repositories.Models;
using Ticketing.Models;

namespace Repositories
{
    public interface IShowRepository
    {
        List<ShowInformation> GetShows();
    }
}

    