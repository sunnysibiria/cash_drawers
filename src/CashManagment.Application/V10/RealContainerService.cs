using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CashManagment.Domain.Models;
using CashManagment.Domain.Enum;
using CashManagment.Domain.InfrastructureEntities;
using CashManagment.Domain.Specifications;

namespace CashManagment.Application.V10
{
    public class RealContainerService : IRealContainerService
    {
        private readonly IRealContainerRepository _realcontainerRepository;
        private readonly ISpecificationCreator _specificationCreator;
        public RealContainerService(IRealContainerRepository realRepo, ISpecificationCreator spec)
        {
            _realcontainerRepository = realRepo;
            _specificationCreator = spec;
        }

        public async Task<List<RealContainer>> GetRealContainersByIdAsync(int[] realContainersId)
        {
            return await _realcontainerRepository.GetAsync(realContainersId);
        }

        public async Task<List<RealContainer>> FindRealContainersAsync(string qrCode, int creditOrgId, int? typeId, int? excludeTypeId, string method, string sortField, string sortType, int offset, int limit)
        {
            var qrCodeParts = RealContainer.TryParseCompositeQR(qrCode);
            var isCompositeQR = qrCodeParts?.Length > 1;
            var searchCode = isCompositeQR ? qrCodeParts[(int)RealContainerQrEnum.ContainerNum].ToString("X").PadLeft(6, '0') : qrCode;

            var specification = _specificationCreator.CreateSpecification(method, searchCode, creditOrgId, typeId, excludeTypeId);

            // Получаем контейнеры по QR коду в базе
            var container = await _realcontainerRepository.GetListAsync(specification, sortField, sortType, offset, limit);

            // Если QR код составной и по нему найден контейнер необходимо осуществить проверку корректности QR кода
            if (isCompositeQR && container.Count == 1)
            {
                // Если значения в QR коде расходятся с данными из базы, помечаем контейнер к проверке
                if (container[0].CheckQRcode(qrCodeParts).Count != 0)
                {
                    container[0].NeedCheck = true;
                    await _realcontainerRepository.SetPropertiesAsync(new int[] { container[0].RealContainerId }, bNeedCheck: true);
                }
            }

            return container;
        }

        public async Task<int> GetCountRealContainersAsync(string qrCode, int creditOrgId, int? typeId, int? excludeTypeId, string method)
        {
            var specification = _specificationCreator.CreateSpecification(method, qrCode, creditOrgId, typeId, excludeTypeId);
            return await _realcontainerRepository.GetCountAsync(specification);
        }

        public async Task<List<RealContainerStatus>> GetRealContainerStatusesAsync()
        {
            return await _realcontainerRepository.GetStatusesAsync();
        }

        public async Task<int> UpdateRealContainerAsync(RealContainer realContainer, bool force)
        {
            return await _realcontainerRepository.UpdateAsync(realContainer, force);
        }

        public async Task<RealContainer> InsertRealContainerAsync(RealContainer realContainer)
        {
            return await _realcontainerRepository.InsertAsync(realContainer);
        }

        public async Task<int> DeleteRealContainersAsync(int[] realContainersId, bool force)
        {
            return await _realcontainerRepository.DeleteAsync(realContainersId, force);
        }

        public async Task<int> UpdateRealContainersStatusAsync(int[] realContainersId, int statusId, bool force)
        {
            return await _realcontainerRepository.UpdateStatusAsync(realContainersId, statusId, force);
        }

        public async Task<List<RealContainer>> InsertRealContainerCopyAsync(RealContainer realContainer)
        {
            return await _realcontainerRepository.InsertCopyAsync(realContainer);
        }

        public async Task<int> SetRealContainersPropertiesAsync(int[] realContainersId, bool? bWroteOff = null, bool? bNeedCheck = null)
        {
            return await _realcontainerRepository.SetPropertiesAsync(realContainersId, bWroteOff, bNeedCheck);
        }

        public async Task<List<RealContainerDetail>> GetRealContainerDetailsInWorthAsync(int realContainerId, int userId)
        {
            return await _realcontainerRepository.GetDetailsInWorthAsync(realContainerId, userId);
        }

        public async Task<bool> CheckRealContainerInWorthAsync(int realContainerId, int? userId)
        {
            return await _realcontainerRepository.CheckInWorthAsync(realContainerId, userId);
        }
    }
}
