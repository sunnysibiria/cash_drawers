using System;
using System.Collections.Generic;
using System.Text;

namespace CashManagment.Application.V10.Models
{
    public class CassetteListByPid
    {
        public int Limit;

        public int Offset;

        public int TotalCount;

        public List<Cassette> Cassettes;
    }
}
