namespace CashManagment.Domain.Models
{
    public class TransferStorage
    {
        /// <summary>
        /// Уникальный идентификатор трансфера
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор пользователя-отправителя
        /// </summary>
        public int IdUserFrom { get; set; }

        /// <summary>
        /// Уникальный идентификатор контейнера
        /// </summary>
        public int IdRealContainer { get; set; }

        /// <summary>
        /// Уникальный идентификатор содержимого контейнера
        /// </summary>
        public int IdUserWorthDetails { get; set; }

        /// <summary>
        /// Уникальный идентификтор содержимого хранилища
        /// </summary>
        public int? IdStorageDetails { get; set; }

        /// <summary>
        /// Количество банкнот
        /// </summary>
        public int ICountEdit { get; set; }

        /// <summary>
        /// Общая сумма
        /// </summary>
        public decimal MSumEdit { get; set; }

        /// <summary>
        /// Уникальный идентификатор валюты
        /// </summary>
        public int IdCurrency { get; set; }

        /// <summary>
        /// Уникальный иденитфиктор банкноты
        /// </summary>
        public int? IdBanknotes { get; set; }

        /// <summary>
        /// Уникальный идентификатор категории содержимого контейнера
        /// </summary>
        public int? IdCashRequestDetailsCategory { get; set; }
    }
}
