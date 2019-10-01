using System.Collections.Generic;
using Repositories.Models;

namespace Repositories
{
    public interface IShowRepository
    {
        List<ShowInfo> GetShows();
    }
}

    