using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Ticketing.Models;

namespace Grains.Interfaces
{
    public interface IShowInfo : IGrainWithStringKey
    {
        Task<List<ShowInformation>> GetShows();
        Task<ShowData> GetShow(string showId, string date);

    }
}