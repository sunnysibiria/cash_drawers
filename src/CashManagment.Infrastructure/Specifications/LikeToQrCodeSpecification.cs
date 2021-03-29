using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CashManagment.Domain.Specifications;

namespace CashManagment.Infrastructure.Specifications
{
    public class LikeToQrCodeSpecification : ASpecification
    {
        private readonly string _qrCode;
        public LikeToQrCodeSpecification(string qrCode)
        {
            _qrCode = qrCode;
        }

        public override string ToSql()
        {
            return "(QrCode LIKE CONCAT('%',@qrCode,'%') " +
                        "or (sContainerSequentNumberInHexadecimalFormat like CONCAT('%',@qrCode,'%') " +
                        "and idCasseteType = (select idCasseteType from drType where scode = 'CashContainerType_Cassette')))";
        }

        public override IEnumerable<SqlParameter> ToSqlParameters()
        {
            yield return new SqlParameter("qrCode", _qrCode);
        }
    }
}
