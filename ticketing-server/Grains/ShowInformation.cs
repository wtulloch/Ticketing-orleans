using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grains.Interfaces;
using Orleans;
using Repositories;

namespace Grains
{
    public class ShowInformation: Grain, IShowInfo
    {
        private readonly IShowRepository _repository;

        public ShowInformation(IShowRepository repository)
        {
            _repository = repository;
        }
        public Task<List<string>> GetShows()
        {
            var currentShows = this._repository.GetShows()
                    .Select( si => si.BaseShowId)
                    .ToList();

            return Task.FromResult(currentShows);
        }
    }
}