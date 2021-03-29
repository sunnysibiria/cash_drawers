﻿using CashManagment.Domain.Models;

namespace CashManagment.Domain.InfrastructureEntities
{
    public interface ICasseteQrCodesRepository
    {
        /// <summary>
        /// Получает настройки пользователя для КЦ
        /// </summary>
        /// <param name="creditOrgId">КЦ</param>
        /// <param name="userId">Индентификтор пользователя</param>
        /// <returns>настройки</returns>
        ReportRequestPrintParameters GetUserPrintProperties(int creditOrgId, int userId);

        /// <summary>
        /// Сохраняет настройки пользователя
        /// </summary>
        /// <param name="parameters">настройки</param>
        /// <returns>колисество сохраненных настроек</returns>
        int SetUserPrintProperties(ReportRequestPrintParameters parameters);
    }
}