using System;
using System.Collections.Generic;
using System.Text;
using CashManagment.Domain.Specifications;
using CashManagment.Infrastructure.Enums;

namespace CashManagment.Infrastructure.Specifications
{
    public class SpecificationCreator : ISpecificationCreator
    {
        public ASpecification CreateSpecification(string method, string qrCode = "", int? creditOrgId = null, int? typeId = null, int? excludeTypeId = null)
        {
            SearchMethodEnum methodEnum;
            Enum.TryParse(method, true, out methodEnum);

            var orgSpec = new EqualToCreditOrgIdSpecification(creditOrgId);
            ASpecification typeSpec;
            if (typeId.HasValue)
            {
                typeSpec = excludeTypeId.HasValue ? new EqualToTypeIdSpecification(typeId.Value).And(new EqualToTypeIdSpecification(excludeTypeId.Value, "excludeTypeId").Not()) : new EqualToTypeIdSpecification(typeId.Value);
            }
            else
            {
                typeSpec = excludeTypeId.HasValue ? new EqualToTypeIdSpecification(excludeTypeId.Value).Not() : null;
            }

            var commonSpec = typeSpec == null ? orgSpec : orgSpec.And(typeSpec);
            switch (methodEnum)
            {
                case SearchMethodEnum.Like: return commonSpec.And(new LikeToQrCodeSpecification(qrCode));
                case SearchMethodEnum.Equals: return commonSpec.And(new EqualToQrCodeSpecification(qrCode));
                case SearchMethodEnum.Sequent: return commonSpec.And(new SequentSpecification(qrCode));
                case SearchMethodEnum.LikeAndOnlyDisabled: return commonSpec.And(new LikeToQrCodeSpecification(qrCode).And(new DisabledOnlySpecification()));
                case SearchMethodEnum.LikeAndOnlyWroteOff: return commonSpec.And(new LikeToQrCodeSpecification(qrCode).And(new WroteOffOnlySpecification()));
                case SearchMethodEnum.All: return commonSpec;
                default:
                    throw new Exception("Не определен метод поиска реального контейнера");
            }
        }
    }
}
