using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using CashManagment.Domain.Specifications;

namespace CashManagment.Infrastructure.Specifications
{
    public class EqualToTypeIdSpecification : ASpecification
    {
        private readonly int _typeId;
        private readonly string _fieldName;

        public EqualToTypeIdSpecification(int typeId, string fieldName = "typeId")
        {
            _typeId = typeId;
            _fieldName = fieldName;
        }

        public override string ToSql()
        {
            return $"idCasseteType = @{_fieldName}";
        }

        public override IEnumerable<SqlParameter> ToSqlParameters()
        {
            yield return new SqlParameter(_fieldName, _typeId);
        }
    }
}
