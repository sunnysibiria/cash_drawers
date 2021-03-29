namespace CashManagment.Api.Models
{
    public class RealContainerStatusRequest
    {
        /// <summary>
        /// Массив из ContainersId
        /// </summary>
        public int[] ContainersId { get; set; }

        /// <summary>
        /// Целевой статус
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Флаг указывающий на необходимость проверки контейнера на блокировку
        /// </summary>
        public bool Force { get; set; }
    }
}
