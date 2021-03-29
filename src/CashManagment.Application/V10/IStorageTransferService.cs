using System.Threading.Tasks;

namespace CashManagment.Application.V10
{
    public interface IStorageTransferService
    {
        /// <summary>
        /// Устанавливает hold
        /// </summary>
        /// <param name="idCashRequest">Идентификтор заявки</param>
        /// <param name="idRealContainer">идентификтор контейнера</param>
        /// <param name="idUser">идентификтора пользователя</param>
        void RealContainerTransferHold(int idCashRequest, int idRealContainer, int idUser);

        /// <summary>
        ///  Снимает hold
        /// </summary>
        /// <param name="idRealContainer">идентификтор контейнера</param>
        /// <param name="idUser">идентификтор пользователя</param>
        void RealContainerTransferUnHold(int idRealContainer, int idUser);

        /// <summary>
        /// Убирает контейнер со стола пользователя
        /// </summary>
        /// <param name="realContainersId">список конейнеров</param>
        /// <param name="userId">идентификатор пользователя</param>
        /// <returns>количестсо отработанных</returns>
        Task<int> UnbindRealContainersAsync(int[] realContainersId, int userId);
    }
}