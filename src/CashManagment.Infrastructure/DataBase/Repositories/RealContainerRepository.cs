using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using CashManagment.Domain.Models;
using CashManagment.Domain.Enum;
using CashManagment.Domain.InfrastructureEntities;
using CashManagment.Domain.Specifications;
using CashManagment.Infrastructure.DataBase.Configuration;

namespace CashManagment.Infrastructure.DataBase.Repositories
{
    public class RealContainerRepository : IRealContainerRepository
    {
        private string _bodyModel = @"[idCassete] as RealContainerId
                                      ,[idCasseteType] as RealContainerTypeId
                                      ,cass.[idModel] as ModelId
                                      ,[idNominal] as NominalId
                                      ,cass.[idCurrency] as CurrencyId
                                      ,cass.[idVendor] as VendorId
                                      ,[idPosition] as PositionId
                                      ,[Volume] as Volume 
                                      ,cass.[idCreditOrg] as CreditOrgId
                                      ,[idStatus] as StatusId
                                      ,[QrCode]
                                      ,[QrCodeNum]
                                      ,[Model]
                                      ,[sSeal] as Seal
                                      ,[sSealDouble] as SealDouble
                                      ,[bSealIntegrity] as SealIntegrity
                                      ,[bSealIntegrityDouble] as SealIntegrityDouble
                                      ,[idCreateUser]
                                      ,[sSerialNumber] as SerialNumber
                                      ,[bNeedCheck] as NeedCheck
                                      ,[bWroteOff] as WroteOff
                                      ,[bDepo]
                                      ,[idCasseteModel] as CasseteModelId
                                      ,[lContainerSequentNumber] as ContainerSequentNumber
                                      ,[sContainerSequentNumberInHexadecimalFormat] as ContainerSequentNumberInHexadecimalFormat
									,ac.sNote AS SmbPositionNum
									,c.SMB_idCurrency AS SmbCurrencyId
									,b.SMB_idBanknotes AS SmbBanknotesId
                                    ,d.SMB_IdModel AS SmbCasseteModelId
                                    ,co.[sEncashmentPin] as EncashmentPin
                                    ,c.sCurrencyCode as Currency
                                    ,b.mValue as CassetteNominal
                                    ,s.StatusName as Status
                                    ,co.SName as Regions
                                    ,ac.sNote as Position
                                    ,d.sName as CassetteModel
                                    ,cass.idCity as CityId
                                    ,cc.Guid as CityGuid
                                    ,cv.sName as Vendor";

        private string _joinModel = @"
                           LEFT JOIN [dbo].[vdrATMCassettePosition] ac on ac.idATMCassettePosition=cass.idPosition
						   LEFT JOIN [dbo].[vdrCasseteVendor] cv on cv.idVendor=cass.idVendor
						   LEFT JOIN [dbo].[drCurrency] c ON c.idCurrency=cass.idCurrency
						   LEFT JOIN [dbo].[drBanknotes] b ON b.idBanknotes=cass.idNominal
                           LEFT JOIN [dbo].[vdrCasseteModel] d ON d.idModel=cass.idCasseteModel
                           LEFT JOIN [dbo].[vdrCreditOrg] co ON cass.idCreditOrg = co.idCreditOrg
                           LEFT JOIN [BankValues].[CasseteCity] cc ON cass.idCity = cc.idCity
                           LEFT JOIN [BankValues].[CasseteStatus] s ON s.id = cass.idStatus";

        /// <summary>
        /// Получить контейнер по ID
        /// </summary>
        /// <param name="realContainersId">realContainersId</param>
        /// <returns>Контейнер</returns>
        public async Task<List<RealContainer>> GetAsync(int[] realContainersId)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = $@"SELECT
                                {_bodyModel}
                          FROM [BankValues].[Cassetes] cass
                                {_joinModel}
                          WHERE
                                idCassete in @realContainersId";
                var result = await sqlConnect.QueryAsync<RealContainer>(sql, new { realContainersId });
                return result.ToList();
            }
        }

        /// <summary>
        /// Найти контейнеры по фильтру
        /// </summary>
        /// <param name="specification">specification</param>
        /// <param name="sortField">sortField</param>
        /// <param name="sortType">sortType</param>
        /// <param name="offset">offset</param>
        /// <param name="limit">limit</param>
        /// <returns>Коллекция контейнеров</returns>
        public async Task<List<RealContainer>> GetListAsync(ASpecification specification, string sortField, string sortType, int offset, int limit)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = $@"SELECT 
                               {_bodyModel}                             
                           FROM [BankValues].[Cassetes] cass
                               {_joinModel}
                           WHERE 1=1
                                {specification.ToSql()}
                           ORDER BY {sortField} {sortType}
                           OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY;";
                var args = new DynamicParameters(new { });
                foreach (var spec in specification.ToSqlParameters())
                {
                    args.Add(spec.ParameterName, spec.Value);
                }

                var result = await sqlConnect.QueryAsync<RealContainer>(sql, args);
                var list = result.ToList();

                return list;
            }
        }

        /// <summary>
        /// Получить кол-во контейнеров по КЦ, типу, QR коду
        /// </summary>
        /// <param name="specification">specification</param>
        /// <returns>Кол-во контейнеров</returns>
        public async Task<int> GetCountAsync(ASpecification specification)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = $@"SELECT 
                               COUNT(cass.[idCassete]) AS RealContainersCount                          
                           FROM [BankValues].[Cassetes] cass						  
                           WHERE 1=1
                                {specification.ToSql()}";
                var args = new DynamicParameters(new { });
                foreach (var spec in specification.ToSqlParameters())
                {
                    args.Add(spec.ParameterName, spec.Value);
                }

                var result = await sqlConnect.ExecuteScalarAsync<int>(sql, args);
                return result;
            }
        }

        /// <summary>
        /// Обновление статусов контейнеров
        /// </summary>
        /// <param name="realContainersId">realContainersId</param>
        /// <param name="statusId">Целевой статус</param>
        /// <param name="force">Необходимость блокировки проверки</param>
        /// <param name="tran">tran</param>
        /// <returns>Кол-во успешных обновлений данных</returns>
        public async Task<int> UpdateStatusAsync(int[] realContainersId, int statusId, bool force, IDbTransaction tran = null)
        {
            if (await CheckRealContainerIsBlockedAsync(realContainersId, Connections.GetLM(), force))
            {
                // изменение статуса заблокировно
                return 0;
            }

            async Task<int> Work(IDbConnection conn, IDbTransaction tranLocal)
            {
                var sql = @"UPDATE [BankValues].[Cassetes]
                                SET [idStatus] = @statusId
                                WHERE idCassete in @realContainersId";
                var result = await conn.ExecuteAsync(sql, new { realContainersId, statusId }, tranLocal);
                return result;
            }

            if (tran != null)
            {
                return await Work(tran.Connection, tran);
            }

            using (var sqlConnect = Connections.GetLM())
            {
                return await Work(sqlConnect, null);
            }
        }

        /// <summary>
        /// Получение существующих в базе статусов контейнеров
        /// </summary>
        /// <returns>Коллекция статусов контейнеров</returns>
        public async Task<List<RealContainerStatus>> GetStatusesAsync()
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = @"SELECT 
                             [Id] as IdType
                            ,[StatusName] as Name
                            ,[StatusCode] as Code
                        FROM [BankValues].[CasseteStatus]";
                var result = await sqlConnect.QueryAsync<RealContainerStatus>(sql);
                return result.ToList();
            }
        }

        /// <summary>
        /// обновляет существующий контейнер
        /// </summary>
        /// <param name="realContainer">realContainer</param>
        /// <param name="force">необходимость проверки на блокировку</param>
        /// <returns>1 удаление прошло успешно, 0 ничего не удалено</returns>
        public async Task<int> UpdateAsync(RealContainer realContainer, bool force)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                if(await CheckRealContainerIsBlockedAsync(realContainer.RealContainerId, sqlConnect, force))
                {
                    // изменение статуса заблокировно
                    return 0;
                }

                realContainer.UpdateQrCodeNum();

                var sql = @"UPDATE [BankValues].[Cassetes]
                           SET [idCasseteType] = @RealContainerTypeId
                              ,[idModel] = @ModelId
                              ,[idNominal] = @NominalId
                              ,[idCurrency] = @CurrencyId
                              ,[idVendor] = @VendorId
                              ,[idPosition] = @PositionId
                              ,[Volume] = @Volume
                              ,[idCreditOrg] = @CreditOrgId
                              ,[idStatus] = @StatusId
                              ,[QrCode] = @QrCode
                              ,[QrCodeNum] = @QrCodeNum
                              ,[Model] = @Model
                              ,[sSeal] = @Seal
                              ,[sSealDouble] = @SealDouble
                              ,[bSealIntegrity] = @SealIntegrity
                              ,[bSealIntegrityDouble] = @SealIntegrityDouble
                              ,[sSerialNumber] = @SerialNumber
                              ,[bDepo] = @BDepo
                              ,[idCasseteModel] = @CasseteModelId
                              ,[idCity] = @CityId
                         WHERE idCassete = @RealContainerId";
                var result = await sqlConnect.ExecuteAsync(sql, new
                {
                    realContainer.RealContainerTypeId,
                    realContainer.ModelId,
                    realContainer.NominalId,
                    realContainer.CurrencyId,
                    realContainer.VendorId,
                    realContainer.PositionId,
                    realContainer.Volume,
                    realContainer.CreditOrgId,
                    realContainer.StatusId,
                    realContainer.QrCode,
                    realContainer.QrCodeNum,
                    realContainer.Model,
                    realContainer.Seal,
                    realContainer.SealDouble,
                    realContainer.SealIntegrity,
                    realContainer.SealIntegrityDouble,
                    realContainer.SerialNumber,
                    realContainer.BDepo,
                    realContainer.RealContainerId,
                    realContainer.CasseteModelId,
                    realContainer.CityId,
                });
                return result;
            }
        }

        /// <summary>
        /// Создает контейнер в базе данных формирует номер QR кода
        /// </summary>
        /// <param name="realContainer">realContainer</param>
        /// <returns>созданный контейнер</returns>
        public async Task<RealContainer> InsertAsync(RealContainer realContainer)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                string[] bCodeForCheck = { "Bag", "SecurePack", "RKCBag", "TechBag", "MoneyPack", "CoinBag", "BanknotePack" };

                var qrCode = realContainer.QrCode;
                if (realContainer.BCode != null && bCodeForCheck.Contains(realContainer.BCode.Trim()))
                {
                    realContainer.VendorId = null;
                    realContainer.NominalId = null;
                    realContainer.PositionId = null;
                    realContainer.CasseteModelId = null;

                    realContainer.QrCode = qrCode;
                    realContainer.UpdateQrCodeNum();

                    if (!await CheckRealContainerNotExistAsync(realContainer.QrCode, realContainer.CreditOrgId) || !await CheckCreditOrgAsync(realContainer.CreditOrgId))
                    {
                        throw new Exception($"Контейнер с номером \"{realContainer.QrCode}\" уже существует в базе.");
                    }
                }
                else
                {
                    qrCode = await GetUniqueNumaratorAsync(realContainer.CreditOrgId);
                    realContainer.QrCode = qrCode;
                    realContainer.UpdateQrCodeNum();

                    if (!await CheckRealContainerNotExistAsync(realContainer) || !await CheckCreditOrgAsync(realContainer.CreditOrgId))
                    {
                        throw new Exception($"Контейнер с заданными параметрами и номером \"{realContainer.QrCode}\" уже существует в базе.");
                    }
                }

                var sql = @"INSERT INTO [BankValues].[Cassetes]
                            ([idCasseteType]
                            ,[idModel]
                            ,[idNominal]
                            ,[idCurrency]
                            ,[idVendor]
                            ,[idPosition]
                            ,[Volume]
                            ,[idCreditOrg]
                            ,[idStatus]
                            ,[QrCode]
                            ,[QrCodeNum]
                            ,[Model]
                            ,[sSeal]
                            ,[sSealDouble]
                            ,[bSealIntegrity]
                            ,[bSealIntegrityDouble]
                            ,[sSerialNUmber]
                            ,[bDepo]
                            ,[idCasseteModel]
                            ,[idCreateUser]
                            ,[idCity])
                        OUTPUT INSERTED.[idCassete]
                        VALUES
                            (@RealContainerTypeId
                            ,@ModelId
                            ,@NominalId
                            ,@CurrencyId
                            ,@VendorId
                            ,@PositionId
                            ,@Volume
                            ,@CreditOrgId
                            ,@StatusId
                            ,@QrCode
                            ,@QrCodeNum
                            ,@Model
                            ,@Seal
                            ,@SealDouble
                            ,@SealIntegrity
                            ,@SealIntegrityDouble
                            ,@SerialNumber
                            ,@BDepo
                            ,@CasseteModelId
                            ,@IdCreateUser
                            ,@CityId)";
                var newRealContainerId = await sqlConnect.QuerySingleAsync<int>(sql, new
                {
                    realContainer.RealContainerTypeId,
                    realContainer.ModelId,
                    realContainer.NominalId,
                    realContainer.CurrencyId,
                    realContainer.VendorId,
                    realContainer.PositionId,
                    realContainer.Volume,
                    realContainer.CreditOrgId,
                    realContainer.StatusId,
                    realContainer.QrCode,
                    realContainer.QrCodeNum,
                    realContainer.Model,
                    realContainer.Seal,
                    realContainer.SealDouble,
                    realContainer.SealIntegrity,
                    realContainer.SealIntegrityDouble,
                    realContainer.SerialNumber,
                    realContainer.BDepo,
                    realContainer.CasseteModelId,
                    realContainer.IdCreateUser,
                    realContainer.CityId
                });

                if (newRealContainerId == 0)
                {
                    throw new Exception($"Не удалось добавить контейнеры.");
                }

                var realContainers = await GetAsync(new[] { newRealContainerId });
                return realContainers.SingleOrDefault();
            }
        }

        /// <summary>
        /// Создает указанное кол-во копий контейнеров в базе с разными QR
        /// </summary>
        /// <param name="realContainer">realContainer</param>
        /// <returns>коллекция созданных контейнеров</returns>
        public async Task<List<RealContainer>> InsertCopyAsync(RealContainer realContainer)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                List<string> copyCassetes = new List<string>();

                realContainer.QrCode = await GetUniqueNumaratorAsync(realContainer.CreditOrgId, realContainer.CountCopyCassetes);
                long qrCount = long.Parse(realContainer.QrCode);

                for (int i = 0; i < realContainer.CountCopyCassetes; i++)
                {
                    realContainer.QrCode = (qrCount++).ToString();
                    realContainer.UpdateQrCodeNum();
                    copyCassetes.Add(@"(@RealContainerTypeId
                            , @ModelId
                            , @NominalId
                            , @CurrencyId
                            , @VendorId
                            , @PositionId
                            , @Volume
                            , @CreditOrgId
                            , @StatusId
                            , '@QrCode'
                            , '@QrCodeNum'
                            , @Model
                            , @Seal
                            , @SealDouble
                            , @SealIntegrity
                            , @SealIntegrityDouble
                            , @SerialNumber
                            , @BDepo
                            , @CasseteModelId
                            , @IdCreateUser
                            , @CityId)".Replace("@QrCodeNum", realContainer.QrCodeNum.ToString()).Replace("@QrCode", realContainer.QrCode));
                }

                if (await CheckRealContainerNotExistAsync(realContainer) && await CheckCreditOrgAsync(realContainer.CreditOrgId))
                {
                    var sql = @"INSERT INTO [BankValues].[Cassetes]
                            ([idCasseteType]
                            ,[idModel]
                            ,[idNominal]
                            ,[idCurrency]
                            ,[idVendor]
                            ,[idPosition]
                            ,[Volume]
                            ,[idCreditOrg]
                            ,[idStatus]
                            ,[QrCode]
                            ,[QrCodeNum]
                            ,[Model]
                            ,[sSeal]
                            ,[sSealDouble]
                            ,[bSealIntegrity]
                            ,[bSealIntegrityDouble]
                            ,[sSerialNUmber]
                            ,[bDepo]
                            ,[idCasseteModel]
                            ,[idCreateUser]
                            ,[idCity])
                        OUTPUT INSERTED.[idCassete]
                        VALUES ";
                    sql = sql + string.Join(", ", copyCassetes.ToArray());
                    var newRealContainerId = await sqlConnect.QueryMultipleAsync(sql, new
                    {
                        realContainer.RealContainerTypeId,
                        realContainer.ModelId,
                        realContainer.NominalId,
                        realContainer.CurrencyId,
                        realContainer.VendorId,
                        realContainer.PositionId,
                        realContainer.Volume,
                        realContainer.CreditOrgId,
                        realContainer.StatusId,
                        realContainer.Model,
                        realContainer.Seal,
                        realContainer.SealDouble,
                        realContainer.SealIntegrity,
                        realContainer.SealIntegrityDouble,
                        realContainer.SerialNumber,
                        realContainer.BDepo,
                        realContainer.CasseteModelId,
                        realContainer.IdCreateUser,
                        realContainer.CityId
                    });

                    var results = newRealContainerId.Read<int>().ToArray();
                    if (results.Length == 0)
                    {
                        return null;
                    }

                    var realContainers = await GetAsync(results);
                    return realContainers;
                }
            }

            return new List<RealContainer>();
        }

        /// <summary>
        /// Удаляет контейнеры из базы данных
        /// </summary>
        /// <param name="realContainersId">realContainersId</param>
        /// <param name="force">необходимость проверки</param>
        /// <param name="conn">conn</param>
        /// <param name="tran">tran</param>
        /// <returns>кол-во удаленных контейнеров</returns>
        public async Task<int> DeleteAsync(int[] realContainersId, bool force, SqlConnection conn = null, IDbTransaction tran = null)
        {
            async Task<int> Work(SqlConnection сonnLocal, IDbTransaction tranLocal)
            {
                var cssOnUserTable = await CheckRealContainerInWorthAsync(realContainersId, сonnLocal, tranLocal);
                var containerIsBlocked = await CheckRealContainerIsBlockedAsync(realContainersId, сonnLocal, force, tranLocal);
                var cssActual = await CheckActualRealContainerAsync(realContainersId, сonnLocal, tranLocal);
                if (!containerIsBlocked && !cssOnUserTable && (force || cssActual))
                {
                    var sql = string.Format(
                        @"update [Cash].[ftTransfer] set idRealContainer = null where idRealContainer in ({0})",
                        string.Join(',', realContainersId));
                    сonnLocal.Execute(sql, transaction: tranLocal);

                    sql = string.Format(
                        @"DELETE FROM [BankValues].[Cassetes] WHERE idCassete in ({0})",
                        string.Join(',', realContainersId));
                    var result = сonnLocal.Execute(sql, transaction: tranLocal);
                    return result;
                }

                // изменение статуса заблокировно
                return 0;
            }

            if (tran != null)
            {
                return await Work(conn, tran);
            }

            using (var sqlConnect = Connections.GetLM())
            {
                sqlConnect.Open();
                using (var tranIn = sqlConnect.BeginTransaction())
                {
                    var result = await Work(sqlConnect, tranIn);
                    tranIn.Commit();
                    return result;
                }
            }
        }

        /// <summary>
        /// Проверяет находятся ли контейнеры на столе у сотрудника
        /// </summary>
        /// <param name="realContainersId">realContainersId</param>
        /// <param name="connect">connect</param>
        /// <param name="tran">tran</param>
        /// <returns>bool</returns>
        private async Task<bool> CheckRealContainerInWorthAsync(int[] realContainersId, SqlConnection connect, IDbTransaction tran = null)
        {
            var sql = string.Format(
                @"select dl.idRealContainer from Cash.ftUserWorthDetails dl 
					    where dl.idRealContainer in ({0})",
                string.Join(',', realContainersId));
            var result = await connect.QueryAsync(sql, transaction: tran);
            return result != null && result.Count() == realContainersId.Length;
        }

        /// <summary>
        /// Проверка контейнера на блокировку изза: 1. необходимости проверки в ИТ 2. кассета отмечена на списание
        /// </summary>
        /// <param name="realContainerId">realContainersId</param>
        /// <param name="connect">connect</param>
        /// <param name="force">нужно ли пропустить данную проверку</param>
        /// <param name="tran">tran</param>
        /// <returns>True в случае если есть блокировка</returns>
        private async Task<bool> CheckRealContainerIsBlockedAsync(int realContainerId, SqlConnection connect, bool force, IDbTransaction tran = null)
        {
            // проверяем необходимость осуществления проверки заблокированных контейнеров. Например если запрос пришел в ходе работы с заявкой.
            if (!force)
            {
                return false;
            }

            var sql = @"select c.idCassete 
						   from [BankValues].[Cassetes] c
                           join dbo.vdrCashContainerType ct on c.[idCasseteType] = ct.idCashContainerType
						   where ([bNeedCheck] = 1 OR [bWroteOff] = 1) and ct.sCode = 'CashContainerType_Cassette' and c.idCassete = @realContainerId";
            var result = await connect.QueryAsync(sql, new { realContainerId }, tran);
            return result != null && result.Count() > 0;
        }

        /// <summary>
        /// Проверка списка реальных контейнеров на блокировку изза 1. необходимости проверки в ИТ 2. кассета отмечена на списание
        /// </summary>
        /// <param name="realContainersId"> массив из realContainerId </param>
        /// <param name="connect">connect</param>
        /// <param name="force">нужно ли пропустить данную проверку</param>
        /// <param name="tran">tran</param>
        /// <returns>True в случае если есть блокировка</returns>
        private async Task<bool> CheckRealContainerIsBlockedAsync(int[] realContainersId, SqlConnection connect, bool force, IDbTransaction tran = null)
        {
            // проверяем необходимость осуществления проверки заблокированных контейнеров. Например если запрос пришел в ходе работы с заявкой.
            if (!force)
            {
                return false;
            }

            var sql = string.Format(
                @"select c.idCassete
						   from [BankValues].[Cassetes] c
                           join dbo.vdrCashContainerType ct on c.[idCasseteType] = ct.idCashContainerType
						   where ([bNeedCheck] = 1 OR [bWroteOff] = 1) and ct.sCode = 'CashContainerType_Cassette' and c.idCassete in ({0})",
                string.Join(',', realContainersId));
            var result = await connect.QueryAsync(sql, transaction: tran);
            return result != null && result.Count() == realContainersId.Length;
        }

        /// <summary>
        /// Находятся ли контейнеры в свободном статусе
        /// </summary>
        /// <param name="realContainersId">realContainersId</param>
        /// <param name="connect">connect</param>
        /// <param name="tran">tran</param>
        /// <returns>true - 1 или несколько контейнеров несвободны false - все контейнеры свободны</returns>
        private async Task<bool> CheckActualRealContainerAsync(int[] realContainersId, SqlConnection connect, IDbTransaction tran = null)
        {
            var sql = string.Format(
                @"select idCassete 
                        from BankValues.Cassetes css 
                        where css.idCassete in ({0}) and css.idStatus = " + (int)RealContainerStatusEnum.Free,
                string.Join(',', realContainersId));
            var result = await connect.QueryAsync(sql, transaction: tran);
            return result != null && result.Count() == realContainersId.Length;
        }

        /// <summary>
        /// Проверка существования в базе такого контейнера
        /// </summary>
        /// <param name="realContainer">realContainer</param>
        /// <returns>true если контейнеров нет, false если такой контейнер найден</returns>
        private async Task<bool> CheckRealContainerNotExistAsync(RealContainer realContainer)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = $@"SELECT 
                            {_bodyModel}
                          FROM [BankValues].[Cassetes] cass
                            {_joinModel}
                          WHERE cass.idCreditOrg = @CreditOrgId
                          and idCasseteType = @RealContainerTypeId
                          and cass.idModel = @ModelId
                          and idNominal = @NominalId
                          and cass.idCurrency= @CurrencyId
                          and cass.idVendor = @VendorId
                          and cass.idCasseteModel = @CasseteModelId
                          and idPosition = @PositionId
                          and Volume = @Volume
                          and idStatus = @StatusId
                          and QrCode = @QrCode
                          and Model = @Model
                          and sSeal = @Seal
                          and sSealDouble = @SealDouble
                          and bSealIntegrity = @SealIntegrity
                          and bSealIntegrityDouble = @SealIntegrityDouble
                          and sSerialNumber = @SerialNumber
                          and cass.idCity = @CityId";
                var result = await sqlConnect.QueryAsync<RealContainer>(sql, new
                {
                    realContainer.RealContainerTypeId,
                    realContainer.ModelId,
                    realContainer.NominalId,
                    realContainer.CurrencyId,
                    realContainer.VendorId,
                    realContainer.PositionId,
                    realContainer.Volume,
                    realContainer.StatusId,
                    realContainer.Model,
                    realContainer.CreditOrgId,
                    realContainer.QrCode,
                    realContainer.Seal,
                    realContainer.SealDouble,
                    realContainer.SealIntegrity,
                    realContainer.SealIntegrityDouble,
                    realContainer.SerialNumber,
                    realContainer.CasseteModelId,
                    realContainer.CityId
                });
                return !result.Any();
            }
        }

        /// <summary>
        /// Проверка существования в базе такого контейнера (проверка для сумок, кассет ркц и т.д., кроде кассет)
        /// </summary>
        /// <param name="qrCode">код контейнера (может быть ручным вводом)</param>
        /// <param name="creditOrgId">ид КЦ</param>
        /// <returns>true если контейнеров нет, false если такой контейнер найден</returns>
        private async Task<bool> CheckRealContainerNotExistAsync(string qrCode, int creditOrgId)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = $@"SELECT 
                            {_bodyModel}
                          FROM [BankValues].[Cassetes] cass
                            {_joinModel}
                          WHERE cass.idCreditOrg = @creditOrgId
                          and QrCode = @qrCode";
                var result = await sqlConnect.QueryAsync<RealContainer>(sql, new
                {
                    qrCode,
                    creditOrgId
                });
                return !result.Any();
            }
        }

        /// <summary>
        /// Присвоить уникальный QR код контейнеру
        /// </summary>
        /// <param name="creditOrgId">creditOrgId</param>
        /// <param name="countQr">countQr</param>
        /// <returns>QR код</returns>
        private async Task<string> GetUniqueNumaratorAsync(int creditOrgId, int countQr = 1)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = @"
                        BEGIN TRAN;
                        SELECT
                           [idCreditOrgGroup] as CreditGroupId
                          ,[Numerator]
                          FROM [BankValues].[CasseteNumerator] with (updlock)
                          WHERE idCreditOrg = @creditOrgId

                        BEGIN TRY
                        UPDATE [BankValues].[CasseteNumerator]
                                       SET [Numerator] = [Numerator] + @countQr
                                     WHERE idCreditOrg = @creditOrgId
                        COMMIT;
                        END TRY
                        BEGIN CATCH
                            ROLLBACK;
                        END CATCH;";
                var result = await sqlConnect.QueryAsync<RealContainerNumerator>(sql, new { creditOrgId, countQr });
                if (result.Any())
                {
                    var numerator = result.ToList().SingleOrDefault();
                    var id = numerator.CreditGroupId + $"{numerator.Numerator:d8}";
                    return id;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Проверить существование КЦ по id
        /// </summary>
        /// <param name="creditOrgId">creditOrgId</param>
        /// <returns>true-кц существует, false-кц не существует</returns>
        private async Task<bool> CheckCreditOrgAsync(int creditOrgId)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = @"SELECT
                           [idCreditOrgGroup] as CreditGroupId
                          ,[Numerator]
                          FROM [BankValues].[CasseteNumerator]
                          WHERE idCreditOrg = @creditOrgId";
                var result = await sqlConnect.QueryAsync<RealContainerNumerator>(sql, new { creditOrgId });
                if (result.Any())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Устанавливает параметр блокировки контейнера
        /// </summary>
        /// <param name="realContainersId">realContainerId</param>
        /// <param name="bWroteOff">На списание</param>
        /// <param name="bNeedCheck">Нужна проверка в ИТ</param>
        /// <param name="tran">tran</param>
        /// <returns>кол-во обновленных контейнеров</returns>
        public async Task<int> SetPropertiesAsync(int[] realContainersId, bool? bWroteOff = null, bool? bNeedCheck = null, IDbTransaction tran = null)
        {
            async Task<int> Work(IDbConnection conn, IDbTransaction tranLocal)
            {
                var sql = @"UPDATE [BankValues].[Cassetes]
                                SET [bWroteOff] = CASE WHEN @bWroteOff is NULL THEN [bWroteOff] ELSE @bWroteOff END,
                                [bNeedCheck] = CASE WHEN @bNeedCheck is NULL THEN [bNeedCheck] ELSE @bNeedCheck END
                                WHERE idCassete in @realContainersId";
                var result = await conn.ExecuteAsync(sql, new { realContainersId, bWroteOff, bNeedCheck }, tranLocal);
                return result;
            }

            if (tran != null)
            {
                return await Work(tran.Connection, tran);
            }

            using (var sqlConnect = Connections.GetLM())
            {
                return await Work(sqlConnect, null);
            }
        }

        public async Task<List<RealContainerDetail>> GetDetailsInWorthAsync(int realContainerId, int userId)
        {
            var details = new List<RealContainerDetail>();

            using (var sqlConnect = Connections.GetLM())
            {
                sqlConnect.Open();
                using (var tran = sqlConnect.BeginTransaction())
                {
                    // Выборка со стола сотрудника ценностей сонтейнера, который еще не добавляли в заявку
                    var sql = @"select 
                             ft.idCashRequestDetailsCategory as CashRequestDetailsCategoryId
                            ,ft.idRealContainer as RealContainerId
                            ,ft.idBanknotes as BanknotesId
                            ,ft.iCount as Count
                            ,ft.mSum as Sum
                            ,ft.idCashRequest as CashRequestId
                            ,ft.idCurrency as CurrencyId                                
                            ,ft.idWorthType as WorthTypeId 
                            ,ft.idUserWorthDetails as IdUserWorthDetails
                            from Cash.ftUserWorthDetails ft
                        where 
                            idCashContainer is null 
                            and idCashRequest is null 
                            and idCashRequestDetails is null 
                            and ft.idRealContainer = @realContainerId 
                            and ft.idUser=@userId";
                    details = (await sqlConnect.QueryAsync<RealContainerDetail>(sql, new { realContainerId, userId }, tran)).ToList();

                    foreach (var detail in details)
                    {
                        var sqlReal = @"select 
                                         cass.idCasseteType as RealContainerTypeId
                                        ,cass.QrCode
                                    from BankValues.Cassetes cass 
                                    where cass.idCassete = @RealContainerId";
                        var resultReal = await sqlConnect.QueryAsync<RealContainer>(sqlReal, new { detail.RealContainerId }, tran);
                        var real = resultReal.Any() ? resultReal.ToList().First() : new RealContainer();
                        if (real.RealContainerTypeId == null)
                        {
                            throw new Exception("Не удалось определить тип реального контейнера");
                        }

                        detail.RealContainerTypeId = real.RealContainerTypeId.Value;
                        detail.Number = real.QrCode;
                    }

                    tran.Commit();
                }
            }

            return details;
        }

        public async Task<bool> CheckInWorthAsync(int realContainerId, int? userId)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = $@"select 
                            count(*)
                        from Cash.ftUserWorthDetails ft
                        where 
                            idCashContainer is null 
                            and idCashRequest is null 
                            and idCashRequestDetails is null 
                            and ft.idRealContainer = @realContainerId 
                            {(userId == null ? string.Empty : "and ft.idUser = @userId")}";
                var count = await sqlConnect.ExecuteScalarAsync<int>(sql, new { realContainerId, userId });

                return count > 0;
            }
        }

        public async Task<List<RealContainerCity>> GetCitiesAsync()
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = @"SELECT
                             [idCity] as IdType
                            ,[Name] as Name
                            ,[Address] as Address
                            ,[Guid] as Guid
                        FROM [BankValues].[CasseteCity]";
                var result = await sqlConnect.QueryAsync<RealContainerCity>(sql);
                return result.ToList();
            }
        }

        public async Task<int> InsertCityAsync(RealContainerCity city)
        {
            using (var sqlConnect = Connections.GetLM())
            {
                var sql = @"INSERT INTO [BankValues].[CasseteCity]
                            ([Name]
                            ,[Address]
                            ,[Guid])
                        OUTPUT INSERTED.[idCity]
                        VALUES (@Name, @Address, @Guid)";
                var newId = await sqlConnect.QuerySingleAsync<int>(sql, new { city.Name, city.Address, city.Guid });

                if (newId == 0)
                {
                    throw new Exception($"Не удалось добавить город.");
                }

                return newId;
            }
        }
    }
}
