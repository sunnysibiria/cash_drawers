using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using CashManagment.Domain.Models;
using CashManagment.Domain.Enum;
using CashManagment.Domain.InfrastructureEntities;
using CashManagment.Infrastructure.DataBase.Configuration;

namespace CashManagment.Infrastructure.DataBase.Repositories
{
    public class StorageTransferRepository : IStorageTransferRepository
    {
        private readonly IRealContainerRepository _realRepo;
        private readonly IStrorageTransferProxy _storageTransferProxy;

        public StorageTransferRepository(IRealContainerRepository realRepo, IStrorageTransferProxy storageTransferProxy)
        {
            _realRepo = realRepo;
            _storageTransferProxy = storageTransferProxy;
        }

        public async void RealContainerTransferHoldAsync(int idCashRequest, int idRealContainer, int idUser)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = @"SELECT
                         det.idCashRequestDetails as Id
                        ,uwd.idUser as IdUserFrom
                        ,uwd.idRealContainer as IdRealContainer
                        ,uwd.idUserWorthDetails as IdUserWorthDetails
                        ,null as IdStorageDetails
                        ,uwd.iCount as ICountEdit
                        ,uwd.mSum as MSumEdit
                        ,uwd.idCurrency as IdCurrency
                        ,uwd.idBanknotes as IdBanknotes
                        ,uwd.idCashRequestDetailsCategory as IdCashRequestDetailsCategory
                    FROM Cash.ftCashRequestDetails det
                    JOIN Cash.ftCashContainer cont ON cont.idCashContainer = det.idCashContainer
                    JOIN Cash.ftUserWorthDetails uwd ON uwd.idUserWorthDetails = det.idUserWorthDetails
                    WHERE
                        uwd.idCashContainer is null
                        AND uwd.idCashRequest is null
                        AND uwd.idCashRequestDetails is null
                        AND det.idCashRequest = @idCashRequest
                        AND uwd.idUser = @idUser
                        AND uwd.idRealContainer = @idRealContainer";
                var transferDetails = (await sqlConnect.QueryAsync<TransferStorage>(sql, new
                {
                    idCashRequest,
                    idUser,
                    idRealContainer,
                })).ToList();

                if (!transferDetails.Any())
                {
                    return;
                }

                var error = _storageTransferProxy.ChangeState(transferDetails, idUser, sqlConnect, state: "Hold");

                if (!string.IsNullOrWhiteSpace(error))
                {
                    throw new Exception(error);
                }
            }
        }

        public async void RealContainerTransferUnHoldAsync(int idRealContainer, int idUser)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = @"select 
                            uwdh.idCashRequestDetails as Id
                            ,uwd.idUser as IdUserFrom
                            ,uwd.idRealContainer as IdRealContainer
                            ,uwd.idUserWorthDetails as IdUserWorthDetails
                            ,null as IdStorageDetails
                            ,uwd.iCount as ICountEdit
                            ,uwd.mSum as MSumEdit
                            ,uwd.idCurrency as IdCurrency
                            ,uwd.idBanknotes as IdBanknotes
                            ,uwd.idCashRequestDetailsCategory as IdCashRequestDetailsCategory
                        from Cash.ftUserWorthDetails uwd
                        join Cash.ftUserWorthDetailsHold uwdh on uwdh.idUserWorthDetails = uwd.idUserWorthDetails
		                where 
                            uwd.idCashContainer is null 
                            and uwd.idCashRequest is null 
                            and uwd.idCashRequestDetails is null
                            and uwd.idRealContainer = @idRealContainer";
                var transferDetails = (await sqlConnect.QueryAsync<TransferStorage>(sql, new { idRealContainer })).ToList();

                if (transferDetails.Any(m => m.IdUserFrom != idUser))
                {
                    throw new Exception("Ценности реального контейнера захолдированы на столе другого сотрудника");
                }

                // Берем только те записи, которые захолдированы
                if (!transferDetails.Any())
                {
                    return;
                }

                var error = _storageTransferProxy.ChangeState(transferDetails, idUser, sqlConnect, state: "UnHold");

                if (!string.IsNullOrWhiteSpace(error))
                {
                    throw new Exception(error);
                }
            }
        }

        public async Task<int> UnbindRealContainersAsync(int[] realContainersId, int userId)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                sqlConnect.Open();
                using (var tran = sqlConnect.BeginTransaction())
                {
                    var error = _storageTransferProxy.ContainerSet(realContainersId, userId, sqlConnect, tran);
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                            throw new Exception(error);
                    }

                    // Если технологическая сумка, то при расформировании - удаляем ее
                    // TODO: вынести справочники в отдельный сервис
                    var sql = @"select top 1
                                  idCashContainerType
                                from dbo.vdrCashContainerType
                                where sCode = 'CashContainerType_TechBag'";
                    var idCashContainerTypeTechBag = await sqlConnect.ExecuteScalarAsync<int>(sql, null, tran);

                    var sets = (await _realRepo.GetAsync(realContainersId))
                                .Where(m => m.RealContainerTypeId == idCashContainerTypeTechBag)
                                .ToList();
                    foreach(var m in sets)
                    {
                        int[] containersId = new int[] { m.RealContainerId };
                        await _realRepo.DeleteAsync(containersId, true, sqlConnect, tran);
                    }

                    var result = await _realRepo.UpdateStatusAsync(
                        realContainersId,
                        (int)RealContainerStatusEnum.Free, false, tran);
                    tran.Commit();

                    return result;
                }
            }
        }
    }
}
