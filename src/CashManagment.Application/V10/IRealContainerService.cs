using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CashManagment.Domain.Models;

namespace CashManagment.Application.V10
{
    public interface IRealContainerService
    {
        /// <summary>
        /// Получает контейнеры по их уникальными идентификаторам
        /// </summary>
        /// <param name="realContainersId">Список контейнеров</param>
        /// <returns>список контейнеров</returns>
        public Task<List<RealContainer>> GetRealContainersByIdAsync(int[] realContainersId);

        /// <summary>
        /// Получает контейнеры по фильтрам
        /// </summary>
        /// <param name="qrCode">композитный или обычный qr код</param>
        /// <param name="creditOrgId">Иденфикатор кц контейнера</param>
        /// <param name="typeId">с данным типа контейнера</param>
        /// <param name="excludeTypeId">кроме контейнеров с типом excludeTypeId</param>
        /// <param name="method">метод соответствия</param>
        /// <param name="sortField">поле сортировки</param>
        /// <param name="sortType">тип сортировки</param>
        /// <param name="offset">смещение выборка</param>
        /// <param name="limit">ограничение порции результата</param>
        /// <returns>список контейнеров</returns>
        Task<List<RealContainer>> FindRealContainersAsync(string qrCode, int creditOrgId, int? typeId, int? excludeTypeId,
                                               string method, string sortField,
                                               string sortType, int offset, int limit);

        /// <summary>
        /// Получает количество контейнеров по фильтрам
        /// </summary>
        /// <param name="qrCode">композитный или обычный qr код</param>
        /// <param name="creditOrgId">Иденфикатор кц контейнера</param>
        /// <param name="typeId">с данным типа контейнера</param>
        /// <param name="excludeTypeId">кроме контейнеров с типом excludeTypeId</param>
        /// <param name="method">метод соответствия</param>
        /// <returns>количество контейнеров</returns>
        Task<int> GetCountRealContainersAsync(string qrCode, int creditOrgId, int? typeId, int? excludeTypeId, string method);

        /// <summary>
        /// Получает список возможных статусов контейнера
        /// </summary>
        /// <returns>Список статусов</returns>
        Task<List<RealContainerStatus>> GetRealContainerStatusesAsync();

        /// <summary>
        /// Устанавливает статус у контейнеров
        /// </summary>
        /// <param name="realContainersId">список конейнеров</param>
        /// <param name="statusId">иденификатор статуса</param>
        /// <param name="force">Признак блокировки проверок</param>
        /// <returns>Количество установленных статусов</returns>
        Task<int> UpdateRealContainersStatusAsync(int[] realContainersId, int statusId, bool force);

        /// <summary>
        /// Обновляет контейнер
        /// </summary>
        /// <param name="realContainer">Параметры контейнера</param>
        /// <param name="force">Признак блокировки ошибок</param>
        /// <returns>количество обновленных</returns>
        public Task<int> UpdateRealContainerAsync(RealContainer realContainer, bool force);

        /// <summary>
        /// Добавляет контейнер
        /// </summary>
        /// <param name="realContainer">Контейнер</param>
        /// <returns>контейнер</returns>
        public Task<RealContainer> InsertRealContainerAsync(RealContainer realContainer);

        /// <summary>
        /// Добавляет заданное количество копий контейнера
        /// </summary>
        /// <param name="realContainer">параметры контейнера</param>
        /// <returns>количество добавленных</returns>
        Task<List<RealContainer>> InsertRealContainerCopyAsync(RealContainer realContainer);

        /// <summary>
        /// Удаляет контейнеры
        /// </summary>
        /// <param name="realContainersId">список контейнеров</param>
        /// <param name="force">признак блокировки ошибок</param>
        /// <returns>количество удаленных</returns>
        Task<int> DeleteRealContainersAsync(int[] realContainersId, bool force);

       /// <summary>
       /// Устанавливает дополнительные признаки контейнера
       /// </summary>
       /// <param name="realContainersId">спиоск контейнеров</param>
       /// <param name="bWroteOff">признак списания контейнера</param>
       /// <param name="bNeedCheck">признак необходимости проверки контейнера в ИТ</param>
       /// <returns>количество обработанных контейнеров</returns>
        Task<int> SetRealContainersPropertiesAsync(int[] realContainersId, bool? bWroteOff = null, bool? bNeedCheck = null);

        /// <summary>
        /// Получает содержимое контейнеров пользователя
        /// </summary>
        /// <param name="realContainerId">список контейнеров</param>
        /// <param name="userId">идентификатор пользователя</param>
        /// <returns>спиоск контейнера</returns>
        Task<List<RealContainerDetail>> GetRealContainerDetailsInWorthAsync(int realContainerId, int userId);

        /// <summary>
        /// Проверка наличия контейнера на столе
        /// </summary>
        /// <param name="realContainerId">Номер контейнера</param>
        /// <param name="userId">идентификатор пользователя</param>
        /// <returns>результат</returns>
        Task<bool> CheckRealContainerInWorthAsync(int realContainerId, int? userId);
    }
}
