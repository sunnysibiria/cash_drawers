using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashManagment.Application.V10.Models
{
    public class CassetteProperties
    {
        /// <summary>
        /// Номер кассеты в РК
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// Новый статус кассеты в РК
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Новый признак кассеты «На списание»
        /// </summary>
        public bool AtRemove { get; set; }

        /// <summary>
        /// Новый признак кассеты «Проверить кассету в ИТ»
        /// </summary>
        public bool AtCheck { get; set; }
    }
}
