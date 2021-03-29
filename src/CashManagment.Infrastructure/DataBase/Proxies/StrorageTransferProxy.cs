using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;
using CashManagment.Domain.Models;
using CashManagment.Domain.InfrastructureEntities;

namespace CashManagment.Infrastructure.DataBase.Proxies
{
    public class StrorageTransferProxy : IStrorageTransferProxy
    {
        public string ContainerSet(int[] realContainersId, int userId, SqlConnection sqlConnect, SqlTransaction tran = null)
        {
            foreach (var realContainerId in realContainersId)
            {
                var parms = new DynamicParameters();
                parms.Add("@idUserFrom", userId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@idUserTo", userId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@idEditUser", userId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@idRealContainer", realContainerId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@sAction", "ContainerBreak", DbType.String, ParameterDirection.Input);
                parms.Add("@sErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output,
                    size: int.MaxValue);

                sqlConnect.Execute("[Cash].[Transfer_Container_Set]", parms, tran,
                                   commandType: CommandType.StoredProcedure);

                var error = parms.Get<string>("@sErrorMessage");
                if (!string.IsNullOrWhiteSpace(error))
                {
                    return error;
                }
            }

            return string.Empty;
        }

        public string ChangeState(List<TransferStorage> transferDetails, int idUser, SqlConnection sqlConnect, SqlTransaction tran = null, string state = "Hold")
        {
            var strJson = JsonConvert.SerializeObject(new { Data = transferDetails.ToArray() });

            var parms = new DynamicParameters();
            parms.Add("@jsonWorth", strJson, DbType.String, ParameterDirection.Input);
            parms.Add("@idEditUser", idUser, DbType.Int32, ParameterDirection.Input);
            parms.Add("@sError", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

            var nameProcedure = state == "Hold" ? "[Cash].[Transfer_Hold]" : "[Cash].[Transfer_UnHold]";

            sqlConnect.Execute(nameProcedure, parms, tran, commandType: CommandType.StoredProcedure);
            return parms.Get<string>("@sError");
        }
    }
}
