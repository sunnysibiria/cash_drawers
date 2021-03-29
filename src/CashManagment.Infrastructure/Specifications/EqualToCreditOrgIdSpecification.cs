using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using CashManagment.Domain.Specifications;

namespace CashManagment.Infrastructure.Specifications
{
    public class EqualToCreditOrgIdSpecification : ASpecification
    {
        private readonly int? _creditOrgId;

        public EqualToCreditOrgIdSpecification(int? creditOrgId)
        {
            _creditOrgId = creditOrgId;
        }

        public override string ToSql()
        {
            return _creditOrgId.HasValue ? "and cass.idCreditOrg = @creditOrgId" : string.Empty;
        }

        public override IEnumerable<SqlParameter> ToSqlParameters()
        {
            yield return new SqlParameter("creditOrgId", _creditOrgId);
        }
    }
}
