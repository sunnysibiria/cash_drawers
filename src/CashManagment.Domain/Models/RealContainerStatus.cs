namespace CashManagment.Domain.Models
{
    public class RealContainerStatus
    {
        /// <summary>
        /// Уникальный номер статуса
        /// </summary>
        public int IdType { get; set; }

        /// <summary>
        /// Наименование статуса
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// LM код статуса
        /// </summary>
        public string Code { get; set; }
    }
}
