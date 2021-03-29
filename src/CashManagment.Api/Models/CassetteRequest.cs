using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashManagment.Api.Models
{
    public class CassetteRequest
    {
        /// <summary>
        /// Номер кассеты в РК
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// Номер pid автоматического устройства
        /// </summary>
        public int? Pid { get; set; }

        /// <summary>
        /// Смещение выборки результата
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Количество записей в выборки
        /// </summary>
        public int? Limit { get; set; }
    }
}