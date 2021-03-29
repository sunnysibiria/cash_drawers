namespace CashManagment.Domain.Models
{
    public class RealContainerDetail
    {
        /// <summary>
        /// Уникальный идентификатор контейнера
        /// </summary>
        public int RealContainerId { get; set; }

        /// <summary>
        /// Уникальный идентификтор типа контейнера
        /// </summary>
        public int RealContainerTypeId { get; set; }

        /// <summary>
        /// Qr код контейнера
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Уникальный идентификатор категории содержимого контейнера
        /// </summary>
        public int CashRequestDetailsCategoryId { get; set; }

        /// <summary>
        /// Уникальный идентификатор банкноты
        /// </summary>
        public int BanknotesId { get; set; }

        /// <summary>
        /// Количество банкнот
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Общая сумма
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Уникальный идентификатор заявки
        /// </summary>
        public int CashRequestId { get; set; }

        /// <summary>
        /// Уникальный идентификатор валюты
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Уникальный идентификатор типа содержимого контейнера
        /// </summary>
        public int WorthTypeId { get; set; }

        /// <summary>
        /// Уникальный номер содержимого контейнера
        /// </summary>
        public int IdUserWorthDetails { get; set; }
    }
}
