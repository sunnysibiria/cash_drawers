using System;
using System.Data;
using System.Threading.Tasks;
using CashManagment.Domain.InfrastructureEntities;

namespace CashManagment.Application.V10
{
    public class StorageTransferService : IStorageTransferService
    {
        private readonly IStorageTransferRepository _storageReal;

        public StorageTransferService(IStorageTransferRepository storageReal)
        {
            _storageReal = storageReal;
        }

        public void RealContainerTransferHold(int idCashRequest, int idRealContainer, int idUser)
        {
            _storageReal.RealContainerTransferHoldAsync(idCashRequest, idRealContainer, idUser);
        }

        public void RealContainerTransferUnHold(int idRealContainer, int idUser)
        {
            _storageReal.RealContainerTransferUnHoldAsync(idRealContainer, idUser);
        }

        public async Task<int> UnbindRealContainersAsync(int[] realContainersId, int userId)
        {
            return await _storageReal.UnbindRealContainersAsync(realContainersId, userId);
        }
    }
}
