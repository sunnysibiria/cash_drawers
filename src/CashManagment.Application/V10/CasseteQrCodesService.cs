using CashManagment.Domain.Models;
using CashManagment.Domain.InfrastructureEntities;

namespace CashManagment.Application.V10
{
    /// <summary>
    /// Работа с массовой печатью
    /// </summary>
    public class CasseteQrCodesService : ICasseteQrCodesService
    {
        private readonly ICasseteQrCodesRepository _casseteQrCodesRepo;
        public CasseteQrCodesService(ICasseteQrCodesRepository casseteQrCodesRepo)
        {
            _casseteQrCodesRepo = casseteQrCodesRepo;
        }

        public ReportRequestPrintParameters GetUserPrintProperties(int creditOrgId, int userId)
        {
            return _casseteQrCodesRepo.GetUserPrintProperties(creditOrgId, userId);
        }

        public int SetUserPrintProperties(ReportRequestPrintParameters parameters)
        {
            return _casseteQrCodesRepo.SetUserPrintProperties(parameters);
        }
    }
}
