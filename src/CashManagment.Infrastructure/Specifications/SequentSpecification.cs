using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using CashManagment.Domain.Specifications;

namespace CashManagment.Infrastructure.Specifications
{
    public class SequentSpecification : ASpecification
    {
        private readonly string _qrCode;
        public SequentSpecification(string qrCode)
        {
            _qrCode = qrCode;
        }

        public override string ToSql()
        {
            return @"sContainerSequentNumberInHexadecimalFormat = @qrCode
                        and idCasseteType = (select idCasseteType from drType where scode = 'CashContainerType_Cassette')";
        }

        public override IEnumerable<SqlParameter> ToSqlParameters()
        {
            yield return new SqlParameter("qrCode", _qrCode);
        }
    }
}
