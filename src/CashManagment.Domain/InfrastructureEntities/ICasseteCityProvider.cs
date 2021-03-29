using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using CashManagment.Domain.Models;

namespace CashManagment.Domain.InfrastructureEntities
{
    public interface ICasseteCityProvider
    {
        Task<List<RealContainerCity>> GetAsync();
    }
}
