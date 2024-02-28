using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasaAPI.Models.DriverDeatilsModel;
using static CasaAPI.Models.VehicleModel;

namespace CasaAPI.Services
{
    public class DriverService: IDriverService
    {
        private IDriverRepository _driverRepository;
        private IFileManager _fileManager;
        public DriverService(IDriverRepository driverRepository, IFileManager fileManager)
        {
            _driverRepository = driverRepository;
            _fileManager = fileManager;
        }
        public async Task<int> SaveDriver(DriverSaveParameters request)
        {
            return await _driverRepository.SaveDriver(request);
        }
        public async Task<IEnumerable<DriverDetailsResponse>> GetDriversList(DriverSearchParameters request)
        {
            return await _driverRepository.GetDriversList(request);
        }
        public async Task<DriverDetailsResponse?> GetDriverDetailsById(long id)
        {
            return await _driverRepository.GetDriverDetailsById(id);
        }

        public async Task<IEnumerable<DriverFailToImportValidationErrors>> ImportDriversDetails(List<DriverImportSaveParameters> request)
        {
            return await _driverRepository.ImportDriversDetails(request);
        }

        #region MyRegion
        public async Task<int> SaveVehicle(VehicleSaveParameters request)
        {
            return await _driverRepository.SaveVehicle(request);
        }
        public async Task<IEnumerable<VehicleDetailsResponse>> GetVehiclesList(VehicleSearchParameters request)
        {
            return await _driverRepository.GetVehiclesList(request);
        }
        public async Task<VehicleDetailsResponse?> GetVehicleDetailsById(long id)
        {
            return await _driverRepository.GetVehicleDetailsById(id);
        }

        public async Task<IEnumerable<VehicleFailToImportValidationErrors>> ImportVehiclesDetails(List<VehicleImportSaveParameters> request)
        {
            return await _driverRepository.ImportVehiclesDetails(request);
        }
        #endregion
    }
}
