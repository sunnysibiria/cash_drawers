using System.Data.SqlClient;
using Dapper;
using CashManagment.Infrastructure.DataBase.Configuration;
using CashManagment.Domain.Models;
using CashManagment.Domain.InfrastructureEntities;

namespace CashManagment.Infrastructure.DataBase.Repositories
{
    /// <summary>
    /// Работа с массовой печатью
    /// </summary>
    public class CasseteQrCodesRepository : ICasseteQrCodesRepository
    {
        /// <summary>
        /// Получение настроек печати из БД по кредитной организации
        /// </summary>
        /// <param name="creditOrgId">ИД кредитной организации</param>
        /// <param name="userId">ИД пользователя</param>
        /// <returns>настройки для печати</returns>
        public ReportRequestPrintParameters GetUserPrintProperties(int creditOrgId, int userId)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = @"SELECT [CreditOrgId], [UserId], [BottomField], [TopField], [LeftField], [RightField], [ColumnMargin], [RowMargin]
                            FROM BankValues.CassetePrintProperties WHERE CreditOrgId = @creditOrgId and UserId=@userId
                            ";
                var printParams = sqlConnect.QuerySingleOrDefault<ReportRequestPrintParameters>(sql, new
                {
                    creditOrgId,
                    userId,
                });

                return printParams;
            }
        }

        /// <summary>
        /// Сохранение настроек печати в БД по кредитной организации
        /// </summary>
        /// <param name="parameters">настройки печати</param>
        /// <returns>результат выполнения запроса</returns>
        public int SetUserPrintProperties(ReportRequestPrintParameters parameters)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = @"IF EXISTS(SELECT CreditOrgId FROM BankValues.CassetePrintProperties where CreditOrgId = @CreditOrgId and UserId=@userId)
                            UPDATE BankValues.CassetePrintProperties 
                            set 
                                UserId = @UserId, 
                                BottomField = @BottomField, 
                                TopField = @TopField, 
                                LeftField = @LeftField, 
                                RightField = @RightField,
                                ColumnMargin = @ColumnMargin,
                                RowMargin = @RowMargin
                            where CreditOrgId = @CreditOrgId and UserId=@userId
                        ELSE
                        INSERT INTO BankValues.CassetePrintProperties 
                        (
                                CreditOrgId,
                                UserId,
                                BottomField,
                                TopField,
                                LeftField,
                                RightField,
                                ColumnMargin,
                                RowMargin
                        )
                        values
                        (
                                @CreditOrgId,
                                @UserId,
                                @BottomField,
                                @TopField,
                                @LeftField,
                                @RightField,
                                @ColumnMargin,
                                @RowMargin
                        )
                    ";
                var result = sqlConnect.ExecuteScalar<int>(sql, new
                {
                    parameters.UserId,
                    parameters.CreditOrgId,
                    parameters.BottomField,
                    parameters.LeftField,
                    parameters.RightField,
                    parameters.TopField,
                    parameters.ColumnMargin,
                    parameters.RowMargin,
                });
                return result;
            }
        }
    }
}
