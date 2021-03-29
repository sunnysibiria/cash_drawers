using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace CashManagment.Domain.Specifications
{
    public class AndSpecification : ASpecification
    {
        private readonly ASpecification _left;
        private readonly ASpecification _right;

        public AndSpecification(ASpecification left, ASpecification right)
        {
            _right = right;
            _left = left;
        }

        public override string ToSql()
        {
            return _left.ToSql() + " AND " + _right.ToSql();
        }

        public override IEnumerable<SqlParameter> ToSqlParameters()
        {
            foreach (var param in _left.ToSqlParameters())
            {
                yield return new SqlParameter(param.ParameterName, param.Value);
            }

            foreach (var param in _right.ToSqlParameters())
            {
                yield return new SqlParameter(param.ParameterName, param.Value);
            }
        }
    }
}
