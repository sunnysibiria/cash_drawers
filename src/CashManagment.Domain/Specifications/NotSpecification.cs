using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace CashManagment.Domain.Specifications
{
    public class NotSpecification : ASpecification
    {
        private readonly ASpecification _spec;

        public NotSpecification(ASpecification spec)
        {
            _spec = spec;
        }

        public override string ToSql()
        {
            return " NOT( " + _spec.ToSql() + ")";
        }

        public override IEnumerable<SqlParameter> ToSqlParameters()
        {
            foreach (var param in _spec.ToSqlParameters())
            {
                yield return new SqlParameter(param.ParameterName, param.Value);
            }
        }
    }
}
