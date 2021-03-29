namespace CashManagment.Domain.Enum
{
    public enum RealContainerStatusEnum
    {
        /// <summary>
        /// Свободный контейнер
        /// </summary>
        Free = 1,

        /// <summary>
        /// Занятый контейнер
        /// </summary>
        Locked = 2,

        /// <summary>
        /// Подготовленный контейнер
        /// </summary>
        Prepared = 3,

        /// <summary>
        /// Отбракованный контейнер
        /// </summary>
        Broken = 4,

        /// <summary>
        /// Подменный контейнер
        /// </summary>
        RepairRkc = 5,

        /// <summary>
        /// Списанный контейнер
        /// </summary>
        WroteOff = 6,
    }
}