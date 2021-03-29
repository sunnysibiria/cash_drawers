using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CashManagment.Application.V10.Models;
using CashManagment.Domain.Models;

namespace CashManagment.Application.V10
{
    public interface ICassetteService
    {
        Task<CassetteList> GetAllAsync(int offset, int limit);
        Task<Cassette> GetAsync(string num);
        CassetteListByPid GetByPid(int pid);
        Task<CassetteProperties> UpdateAsync(CassetteProperties request);
        Task<int> UpdateCitiesAsync();
        Task<List<RealContainerCity>> GetCitiesAsync();
    }
}
