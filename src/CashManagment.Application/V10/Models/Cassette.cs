using System;
using System.Collections.Generic;
using System.Text;

namespace CashManagment.Application.V10.Models
{
    public class Cassette
    {
        /// <summary>
        /// Номер банкомата в АМОН
        /// </summary>
        public int Pid { get; set; }

        /// <summary>
        /// Наименование вендора\производителя кассет
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Идентификатор города
        /// </summary>
        public string IdCity { get; set; }

        /// <summary>
        /// Номер кассеты в РК
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// Позиция кассеты в банкомате
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Наименование модели кассеты
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Валюта кассеты
        /// </summary>
        public string Curr { get; set; }

        /// <summary>
        /// Номинал кассеты
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Статус кассеты в РК
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Признак кассеты «На списание»
        /// </summary>
        public bool AtRemove { get; set; }

        /// <summary>
        /// Признак кассеты «Проверить кассету в ИТ»
        /// </summary>
        public bool AtCheck { get; set; }
    }
}
