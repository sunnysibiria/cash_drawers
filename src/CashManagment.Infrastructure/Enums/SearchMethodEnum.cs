namespace CashManagment.Infrastructure.Enums
{
    public enum SearchMethodEnum
    {
        /// <summary>
        /// Поиск контейнеров содержащих в номере строку поиска
        /// </summary>
        Like,

        /// <summary>
        /// Поиск контейнера с номером равным строке поиска
        /// </summary>
        Equals,

        /// <summary>
        /// Поиск списанных контейнеров содержащих в номере строку поиска
        /// </summary>
        LikeAndOnlyWroteOff,

        /// <summary>
        /// Поиск списанных контейнеров или имеющих признаки(Проверить в ИТ, Списан) и содержащих в номере строку поиска
        /// </summary>
        LikeAndOnlyDisabled,

        /// <summary>
        /// Поиск контейнера по порядковому номеру
        /// </summary>
        Sequent,

        /// <summary>
        /// Список всех контейнеров
        /// </summary>
        All
    }
}
