using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using static CasaAPI.Models.DriverDeatilsModel;
using static CasaAPI.Models.VehicleModel;

namespace CasaAPI.Repositories
{
    public class DriverRepository: BaseRepository, IDriverRepository
    {
        private IConfiguration _configuration;
        public DriverRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Driver Details
        public async Task<int> SaveDriver(DriverSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@DriverId", parameters.DriverId);
            queryParameters.Add("@DriverName", parameters.DriverName.SanitizeValue().ToUpper());
            queryParameters.Add("@MobileNumber", parameters.MobileNumber.SanitizeValue());
            queryParameters.Add("@ProfilePath", parameters.ProfilePath.SanitizeValue());
            queryParameters.Add("@VehicleNumber", parameters.VehicleNumber.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveDriverDetails", queryParameters);
        }
        public async Task<IEnumerable<DriverDetailsResponse>> GetDriversList(DriverSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<DriverDetailsResponse>("GetDriverrsList", queryParameters);
        }
        public async Task<DriverDetailsResponse?> GetDriverDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<DriverDetailsResponse>("GetDriverDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<DriverFailToImportValidationErrors>> ImportDriversDetails(List<DriverImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlDriverData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlDriverData", xmlDriverData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<DriverFailToImportValidationErrors>("SaveImportDriverDetails", queryParameters);
        }
        #endregion

        #region Vehicle Number
        public async Task<int> SaveVehicle(VehicleSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VehicleId", parameters.VehicleId);
            queryParameters.Add("@VehicleNumber", parameters.VehicleNumber.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveVehicleDetails", queryParameters);
        }
        public async Task<IEnumerable<VehicleDetailsResponse>> GetVehiclesList(VehicleSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<VehicleDetailsResponse>("GetVehiclesList", queryParameters);
        }
        public async Task<VehicleDetailsResponse?> GetVehicleDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<VehicleDetailsResponse>("GetVehicleDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<VehicleFailToImportValidationErrors>> ImportVehiclesDetails(List<VehicleImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlVehicleData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<VehicleFailToImportValidationErrors>("SaveImportVehicleDetails", queryParameters);
        }
        #endregion
    }
}
