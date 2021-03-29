using System;
using System.Collections.Generic;
using System.Text;

namespace CashManagment.Domain.Specifications
{
    public interface ISpecificationCreator
    {
        ASpecification CreateSpecification(string method, string qrCode = "", int? creditOrgId = null, int? typeId = null, int? exludeTypeId = null);
    }
}
