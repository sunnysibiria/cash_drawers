using System;
using System.Collections.Generic;
using System.Text;

namespace CashManagment.Application.V10.Models
{
    public class CassetteList
    {
        public int Limit;

        public int Offset;

        public int Totalcount;

        public List<Cassette> Cassettes;
    }
}
