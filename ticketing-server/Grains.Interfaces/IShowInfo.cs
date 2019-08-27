using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Grains.Interfaces
{
    public interface IShowInfo : IGrainWithStringKey
    {
        Task<List<string>> GetShows();

    }
}