namespace CashManagment.Domain.Models
{
    public class ReportRequestPrintParameters
    {
        /// <summary>
        /// Нижнее поле
        /// </summary>
        public double BottomField { get; set; }

        /// <summary>
        /// Верхнее поле
        /// </summary>
        public double TopField { get; set; }

        /// <summary>
        ///  Левое поле
        /// </summary>
        public double LeftField { get; set; }

        /// <summary>
        /// Правое поле
        /// </summary>
        public double RightField { get; set; }

        /// <summary>
        /// Размер отступа горизонтальный
        /// </summary>
        public double ColumnMargin { get; set; }

        /// <summary>
        /// Размер отступа вертикальный
        /// </summary>а
        public double RowMargin { get; set; }

        /// <summary>
        /// Номер строки
        /// </summary>
        public int StartRow { get; set; }

        /// <summary>
        /// Номер колонки
        /// </summary>
        public int StartCol { get; set; }

        /// <summary>
        /// Список реальных контейнеров
        /// </summary>
        public int[] ContainersId { get; set; }

        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Уникальный номер КЦ
        /// </summary>
        public int CreditOrgId { get; set; }
    }
}
