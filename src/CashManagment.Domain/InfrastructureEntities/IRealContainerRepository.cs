using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CashManagment.Domain.Models;
using CashManagment.Domain.Specifications;

namespace CashManagment.Domain.InfrastructureEntities
{
    public interface IRealContainerRepository
    {
        /// <summary>
        /// Получает контейнеры по их уникальными идентификаторам
        /// </summary>
        /// <param name="realContainersId">Список контейнеров</param>
        /// <returns>список контейнеров</returns>
        Task<List<RealContainer>> GetAsync(int[] realContainersId);

        /// <summary>
        /// Получает контейнеры по фильтрам
        /// </summary>
        /// <param name="specification">Фильтры</param>
        /// <param name="sortField">поле сотрировки</param>
        /// <param name="sortType">тип сортировки</param>
        /// <param name="offset">смещение</param>
        /// <param name="limit">ограничение</param>
        /// <returns>список контейнеров</returns>
        Task<List<RealContainer>> GetListAsync(ASpecification specification, string sortField, string sortType, int offset, int limit);

        /// <summary>
        /// Получает количество контейнеров по фильтрам
        /// </summary>
        /// <param name="specification">Фильтры</param>
        /// <returns>количество</returns>
        Task<int> GetCountAsync(ASpecification specification);

        /// <summary>
        /// Получает список возможных статусов контейнера
        /// </summary>
        /// <returns>Список статусов</returns>
        Task<List<RealContainerStatus>> GetStatusesAsync();

        /// <summary>
        /// Получает список загруженных городов
        /// </summary>
        /// <returns>Список городов</returns>
        Task<List<RealContainerCity>> GetCitiesAsync();

        /// <summary>
        /// Сохраняет новый город
        /// </summary>
        /// <param name="city"></param>
        /// <returns>Количество сохранненых</returns>
        Task<int> InsertCityAsync(RealContainerCity city);

        /// <summary>
        /// Удаляет контейнеры
        /// </summary>
        /// <param name="realContainersId">список контейнеров</param>
        /// <param name="force">признак блокировки ошибок</param>
        /// <param name="conn">соединение с базой</param>
        /// <param name="tran">транзакция</param>
        /// <returns>количество удаленных</returns>
        Task<int> DeleteAsync(int[] realContainersId, bool force, SqlConnection conn = null, IDbTransaction tran = null);

        /// <summary>
        /// Устанавливает дополнительные признаки контейнера
        /// </summary>
        /// <param name="realContainersId">спиоск контейнеров</param>
        /// <param name="bWroteOff">>признак списания контейнера</param>
        /// <param name="bNeedCheck">признак необходимости проверки контейнера в ИТ</param>
        /// <param name="tran">транзакция</param>
        /// <returns>количество установленных</returns>
        Task<int> SetPropertiesAsync(int[] realContainersId, bool? bWroteOff = null, bool? bNeedCheck = null, IDbTransaction tran = null);

        /// <summary>
        /// Устанавливает статус у контейнеров
        /// </summary>
        /// <param name="realContainersId">список конейнеров</param>
        /// <param name="statusId">иденификатор статуса</param>
        /// <param name="force">Признак блокировки проверок</param>
        /// <param name="tran">транзакция</param>
        /// <returns>Количество установленных статусов</returns>
        Task<int> UpdateStatusAsync(int[] realContainersId, int statusId, bool force, IDbTransaction tran = null);

        /// <summary>
        /// Обновляет контейнер
        /// </summary>
        /// <param name="realContainer">Параметры контейнера</param>
        /// <param name="force">Признак блокировки ошибок</param>
        /// <returns>количество обновленных</returns>
        Task<int> UpdateAsync(RealContainer realContainer, bool force);

        /// <summary>
        /// Добавляет контейнер
        /// </summary>
        /// <param name="realContainer">Контейнер</param>
        /// <returns>контейнер</returns>
        Task<RealContainer> InsertAsync(RealContainer realContainer);

        /// <summary>
        /// Добавляет заданное количество копий контейнера
        /// </summary>
        /// <param name="realContainer">параметры контейнера</param>
        /// <returns>количество добавленных</returns>
        Task<List<RealContainer>> InsertCopyAsync(RealContainer realContainer);

        /// <summary>
        /// Получает содержимое контейнеров пользователя
        /// </summary>
        /// <param name="realContainerId">список контейнеров</param>
        /// <param name="userId">идентификатор пользователя</param>
        /// <returns>спиоск контейнера</returns>
        Task<List<RealContainerDetail>> GetDetailsInWorthAsync(int realContainerId, int userId);

        /// <summary>
        /// Проверка наличия контейнера на столе
        /// </summary>
        /// <param name="realContainerId">Номер контейнера</param>
        /// <param name="userId">идентификатор пользователя</param>
        /// <returns>результат</returns>
        Task<bool> CheckInWorthAsync(int realContainerId, int? userId);
    }
}
