namespace CashManagment.Domain.Enum
{
    public enum RealContainerQrEnum
    {
        /// <summary>
        /// Порядковый номер уникального кода КЦ
        /// </summary>
        EncashmentPin = 0,

        /// <summary>
        /// Порядковый номер контейнера
        /// </summary>
        ContainerNum = 1,

        /// <summary>
        /// Порядковый номер позиции кассеты
        /// </summary>
        Position = 2,

        /// <summary>
        /// Порядковый номер валюты кассеты
        /// </summary>
        Currency = 3,

        /// <summary>
        /// Порядковый номер номинала кассеты
        /// </summary>
        Nominal = 4,

        /// <summary>
        /// Порядковый номер модели кассеты
        /// </summary>
        Model = 5,
    }
}
