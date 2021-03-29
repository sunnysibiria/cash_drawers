using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CashManagment.Domain.Enum;

namespace CashManagment.Domain.Models
{
    public class RealContainer
    {
        /// <summary>
        /// Разбивает строку на составные части композитного qr кода
        /// </summary>
        /// <param name="qrCode">строка-qrCode</param>
        /// <returns>составные части qr кода </returns>
        public static long[] TryParseCompositeQR(string qrCode)
        {
            long[] qrRealContainer = { };
            if (qrCode?.Length == 16)
            {
                qrRealContainer = new long[]
                {
                    // 0-1 позиции - ПИН КЦ
                    Convert.ToInt64(qrCode.Substring(0, 2), 16),

                    // 2-7 позиции - сквозной номер кассеты
                    Convert.ToInt64(qrCode.Substring(2, 6), 16),

                    // 8 позиция - позиция кассеты
                    Convert.ToInt64(qrCode.Substring(8, 1), 16),

                    // 9 позиция - валюта кассеты
                    Convert.ToInt64(qrCode.Substring(9, 1), 16),

                    // 10 позиция - номинал кассеты
                    Convert.ToInt64(qrCode.Substring(10, 1), 16),

                    // 11-12 позиции - модель кассеты
                    Convert.ToInt64(qrCode.Substring(11, 2), 16),

                    // 13-15 позиции - резерв
                    Convert.ToInt64(qrCode.Substring(13, 3), 16)
                };
            }

            return qrRealContainer;
        }

        /// <summary>
        /// Уникальный идентификатор контейнера
        /// </summary>
        public int RealContainerId { get; set; }

        /// <summary>
        /// Уникальный индетификтор типа контейнера
        /// </summary>
        public int? RealContainerTypeId { get; set; }

        /// <summary>
        /// Уникальный идентификтор модели
        /// </summary>
        public int? ModelId { get; set; }

        /// <summary>
        /// Уникальный идентификтор валюты контейнера
        /// </summary>
        public int? CurrencyId { get; set; }

        /// <summary>
        /// Уникальный идентификтор произоводителя
        /// </summary>
        public int? VendorId { get; set; }

        /// <summary>
        /// Уникальный идентификтор номинала
        /// </summary>
        public int? NominalId { get; set; }

        /// <summary>
        /// Уникальный идентификтор позиции
        /// </summary>
        public int? PositionId { get; set; }

        /// <summary>
        /// Номер контейнера
        /// </summary>
        public int? Volume { get; set; }

        /// <summary>
        /// Уникальный идентификатор кассового центра
        /// </summary>
        public int CreditOrgId { get; set; }

        /// <summary>
        /// Уникальный идентификтор статуса контейнера
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Qr код контейнера
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public long? QrCodeNum { get; set; }

        /// <summary>
        /// Модель
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Код LM
        /// </summary>
        public string BCode { get; set; }

        /// <summary>
        /// Печать
        /// </summary>
        public string Seal { get; set; }

        /// <summary>
        /// Дополнительная печать
        /// </summary>
        public string SealDouble { get; set; }

        /// <summary>
        /// Признак целостности печати на контейнере
        /// </summary>
        public bool SealIntegrity { get; set; }

        /// <summary>
        /// Признак целостности дополнительной печати
        /// </summary>
        public bool SealIntegrityDouble { get; set; }

        /// <summary>
        /// Серийный номер контейнера
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Признак депо
        /// </summary>
        public bool BDepo { get; set; }

        /// <summary>
        /// Признак списания контейнера
        /// </summary>
        public bool WroteOff { get; set; }

        /// <summary>
        /// Признак необходимость ТО контейнера
        /// </summary>
        public bool NeedCheck { get; set; }

        /// <summary>
        /// Уникальный иденитификтор пользоваля
        /// </summary>
        public int IdCreateUser { get; set; }

        /// <summary>
        /// Количество копий контейнера
        /// </summary>
        public int CountCopyCassetes { get; set; }

        /// <summary>
        /// Позиция в qr коде
        /// </summary>
        public int SmbPositionNum { get; set; }

        /// <summary>
        /// Название модели
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Валюта в qr коде
        /// </summary>
        public int SmbCurrencyId { get; set; }

        /// <summary>
        /// Номинал банкноты в qr коде
        /// </summary>
        public int SmbBanknotesId { get; set; }

        /// <summary>
        /// Модель кассеты в qr коде
        /// </summary>
        public int SmbCasseteModelId { get; set; }

        /// <summary>
        /// Уникальный идентификтор модели кассеты
        /// </summary>
        public int? CasseteModelId { get; set; }

        /// <summary>
        /// Пин код КЦ
        /// </summary>
        public string EncashmentPin { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public long? ContainerSequentNumber { get; set; }

        /// <summary>
        /// Статус кассеты
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Валюта контейнера
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Производитель кассеты
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Город КЦ
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Идентификатор города
        /// </summary>
        public Guid CityGuid { get; set; }

        /// <summary>
        /// Позиция кассые в устройстве
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Идентификатор города кассеты
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// Модель кассеты
        /// </summary>
        public string CassetteModel { get; set; }

        /// <summary>
        /// Номинал кассеты
        /// </summary>
        public decimal CassetteNominal { get; set; }

        /// <summary>
        /// Порядковый номер в шестнадцатеричном формате
        /// </summary>
        public string ContainerSequentNumberInHexadecimalFormat { get; set; }

        /// <summary>
        /// Перечень отличающих реквизитов на этикетке и информацией из базы
        /// </summary>
        public List<string> NeedCheckInfo { get; set; } = new List<string>();

        /// <summary>
        /// Перезаполняет цисловой qr
        /// </summary>
        public void UpdateQrCodeNum()
        {
            var qrCode = Regex.Replace(QrCode, @"\D", string.Empty);
            if (long.TryParse(qrCode, out var qrCodeNum))
            {
                QrCodeNum = qrCodeNum;
            }
        }

        /// <summary>
        /// Сверка ревизитов из этикетки с базой
        /// </summary>
        /// <param name="qr">qr</param>
        /// <returns>перечень расходждений</returns>
        public List<string> CheckQRcode(long[] qr)
        {
            if (Convert.ToInt64(EncashmentPin) != qr[(int)RealContainerQrEnum.EncashmentPin])
            {
                NeedCheckInfo.Add("Уникальный код КЦ");
            }

            if (SmbPositionNum != qr[(int)RealContainerQrEnum.Position])
            {
                NeedCheckInfo.Add("Позиция кассеты");
            }

            if (SmbCurrencyId != qr[(int)RealContainerQrEnum.Currency])
            {
                NeedCheckInfo.Add("Валюта кассеты");
            }

            if (SmbBanknotesId != qr[(int)RealContainerQrEnum.Nominal])
            {
                NeedCheckInfo.Add("Номинал кассеты");
            }

            if (SmbCasseteModelId != qr[(int)RealContainerQrEnum.Model])
            {
                NeedCheckInfo.Add("Модель кассеты");
            }

            return NeedCheckInfo;
        }
    }
}
