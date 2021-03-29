using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using CashManagment.Domain.Models;
using CashManagment.Domain.Specifications;
using CashManagment.Domain.InfrastructureEntities;
using CashManagment.Application.V10.Models;
using AutoMapper;

namespace CashManagment.Application.V10
{
    public class CassetteService : ICassetteService
    {
        private readonly IRealContainerRepository _realContainerRepo;
        private readonly ISpecificationCreator _specificationCreator;
        private readonly ICasseteCityProvider _casseteCityProvider;
        private readonly IMapper _mapper;

        public CassetteService(IRealContainerRepository realContainerRepo, ISpecificationCreator spec, ICasseteCityProvider casseteCityProvider, IMapper mapper)
        {
            _realContainerRepo = realContainerRepo;
            _specificationCreator = spec;
            _casseteCityProvider = casseteCityProvider;
            _mapper = mapper;
        }

        public async Task<CassetteList> GetAllAsync(int offset,  int limit)
        {
            CassetteList response = new CassetteList() { Offset = offset, Limit = limit };
            var specification = _specificationCreator.CreateSpecification("All");
            response.Totalcount = await _realContainerRepo.GetCountAsync(specification);
            var res = await _realContainerRepo.GetListAsync(specification, "RealContainerId", string.Empty, offset, limit);
            response.Cassettes = _mapper.Map<List<RealContainer>, List<Cassette>>(res.ToList());
            return response;
        }

        public CassetteListByPid GetByPid(int pid)
        {
            throw new NotImplementedException();
        }

        public async Task<CassetteProperties> UpdateAsync(CassetteProperties request)
        {
            int id;
            if (int.TryParse(request.Num, out id))
            {
                int[] realContainersId = new int[] { id };
                var statuses = await _realContainerRepo.GetStatusesAsync();
                int? realContainerStatusId = statuses.ToList().Find(s => s.Name == request.Status)?.IdType;

                if (realContainerStatusId.HasValue)
                {
                    await _realContainerRepo.UpdateStatusAsync(realContainersId, realContainerStatusId.Value, false);
                }

                await _realContainerRepo.SetPropertiesAsync(realContainersId, request.AtRemove, request.AtCheck);
                var cas = await _realContainerRepo.GetAsync(realContainersId);
                return _mapper.Map < RealContainer, CassetteProperties>(cas?.FirstOrDefault());
            }

            return null;
        }

        public async Task<Cassette> GetAsync(string num)
        {
            int id;
            if (int.TryParse(num, out id))
            {
                int[] realContainersId = new int[] { id };
                var cas = await _realContainerRepo.GetAsync(realContainersId);
                return _mapper.Map<RealContainer, Cassette>(cas.FirstOrDefault());
            }

            return null;
        }

        public async Task<int> UpdateCitiesAsync()
        {
            var downloadCities = await _casseteCityProvider.GetAsync();
            var existsCities = await _realContainerRepo.GetCitiesAsync();
            var newCities = downloadCities.Where(l => !existsCities.Any(e => l.Guid == e.Guid)).ToList();

            foreach (var n in newCities)
            {
                await _realContainerRepo.InsertCityAsync(n);
            }

            return newCities.Count;
        }

        public async Task<List<RealContainerCity>> GetCitiesAsync()
        {
            var cities = await _realContainerRepo.GetCitiesAsync();
            return cities;
        }
    }
}
