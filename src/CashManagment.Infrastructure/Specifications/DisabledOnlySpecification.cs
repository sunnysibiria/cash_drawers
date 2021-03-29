using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CashManagment.Domain.Models;
using CashManagment.Domain.Enum;
using CashManagment.Domain.Specifications;

namespace CashManagment.Infrastructure.Specifications
{
    public class DisabledOnlySpecification : ASpecification
    {
        public DisabledOnlySpecification()
        {
        }

        public override string ToSql()
        {
            return "(bWroteOff = 1 OR bNeedCheck = 1 OR idStatus in(" + (int)RealContainerStatusEnum.RepairRkc +"))" +
                "AND idCasseteType = (select idCasseteType from drType where scode = 'CashContainerType_Cassette')";
        }

        public override IEnumerable<SqlParameter> ToSqlParameters()
        {
            yield return new SqlParameter();
        }
    }
}
