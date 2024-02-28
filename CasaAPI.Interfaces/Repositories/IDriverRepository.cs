using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasaAPI.Models.DriverDeatilsModel;
using static CasaAPI.Models.VehicleModel;

namespace CasaAPI.Interfaces.Repositories
{
    public interface IDriverRepository
    {
        Task<int> SaveDriver(DriverSaveParameters parameters);
        Task<IEnumerable<DriverDetailsResponse>> GetDriversList(DriverSearchParameters parameters);
        Task<DriverDetailsResponse?> GetDriverDetailsById(long id);
        Task<IEnumerable<DriverFailToImportValidationErrors>> ImportDriversDetails(List<DriverImportSaveParameters> parameters);
        #region Vehicle Number
        Task<int> SaveVehicle(VehicleSaveParameters parameters);
        Task<IEnumerable<VehicleDetailsResponse>> GetVehiclesList(VehicleSearchParameters parameters);
        Task<VehicleDetailsResponse?> GetVehicleDetailsById(long id);
        Task<IEnumerable<VehicleFailToImportValidationErrors>> ImportVehiclesDetails(List<VehicleImportSaveParameters> parameters);
        #endregion
    }
}
