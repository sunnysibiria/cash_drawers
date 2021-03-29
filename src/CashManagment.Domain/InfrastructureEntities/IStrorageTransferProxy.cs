using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using CashManagment.Domain.Models;

namespace CashManagment.Domain.InfrastructureEntities
{
    public interface IStrorageTransferProxy
    {
        /// <summary>
        /// Устанавливает(снимает) hold
        /// </summary>
        /// <param name="transferDetails">содержимое</param>
        /// <param name="idUser">Идентификатор пользователя</param>
        /// <param name="sqlConnect">соединение с базой</param>
        /// <param name="tran">транзация</param>
        /// <param name="state">hold(Unhold)</param>
        /// <returns>ошибки</returns>
        string ChangeState(List<TransferStorage> transferDetails, int idUser, SqlConnection sqlConnect, SqlTransaction tran = null, string state = "Hold");

        /// <summary>
        /// Выполняет передачу ценностей
        /// </summary>
        /// <param name="realContainersId">список контейнеров</param>
        /// <param name="userId">идентификтор пользователя</param>
        /// <param name="sqlConnect">соединение с базой</param>
        /// <param name="tran">транзация</param>
        /// <returns>ошибки</returns>
        string ContainerSet(int[] realContainersId, int userId, SqlConnection sqlConnect, SqlTransaction tran = null);
    }
}
