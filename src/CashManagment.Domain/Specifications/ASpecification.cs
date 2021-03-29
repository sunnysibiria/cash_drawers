using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Dapper;

namespace CashManagment.Domain.Specifications
{
    public abstract class ASpecification
    {
        public abstract string ToSql();
        public abstract IEnumerable<SqlParameter> ToSqlParameters();
        public ASpecification And(ASpecification expression)
        {
            return new AndSpecification(this, expression);
        }

        public ASpecification Not()
        {
            return new NotSpecification(this);
        }
    }
}
