using CasaAPI.Helpers;
using CasaAPI.Models;
using Dapper;
using CasaAPI.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace CasaAPI.Repositories
{
    public class ManageTerritoryRepository : BaseRepository,IManageTerritoryRepository
    {
        private IConfiguration _configuration;

        public ManageTerritoryRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region State
        public async Task<IEnumerable<StateResponse>> GetStatesList(SearchStateRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@StateName", parameters.StateName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<StateResponse>("GetStates", queryParameters);
        }
        public async Task<int> SaveState(StateRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@StateName", parameters.StateName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveStateDetails", queryParameters);
        }
        public async Task<IEnumerable<StateDataValidationErrors>> ImportStatesDetails(List<ImportedStateDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlStateData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlStateData", xmlStateData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<StateDataValidationErrors>("SaveImportStateDetails", queryParameters);
        }
        public async Task<StateResponse?> GetStateDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<StateResponse>("GetStateDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<SelectListResponse>> GetStateDetailsByRegionId(long regionId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RegionId", regionId);
            return (await ListByStoredProcedure<SelectListResponse>("GetStateDetailsByRegionId", queryParameters));
        }
        #endregion

        #region Region
        public async Task<IEnumerable<RegionResponse>> GetRegionsList(SearchRegionRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@RegionName", parameters.RegionName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<RegionResponse>("GetRegions", queryParameters);
        }
        public async Task<int> SaveRegion(RegionRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RegionId", parameters.RegionId);
            //queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@RegionName", parameters.RegionName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveRegionDetails", queryParameters);
        }
        public async Task<RegionResponse?> GetRegionDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<RegionResponse>("GetRegionDetailsById", queryParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<RegionDataValidationErrors>> ImportRegionsDetails(List<ImportedRegionDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlRegionData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlRegionData", xmlRegionData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<RegionDataValidationErrors>("SaveImportRegionDetails", queryParameters);
        }
        #endregion

        #region Districts
        public async Task<IEnumerable<DistrictResponse>> GetDistrictsList(SearchDistrictRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@DistrictName", parameters.DistrictName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<DistrictResponse>("GetDistricts", queryParameters);
        }

        public async Task<int> SaveDistrict(DistrictRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            //queryParameters.Add("@RegionId", parameters.RegionId);
            queryParameters.Add("@DistrictName", parameters.DistrictName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveDistrictDetails", queryParameters);
        }

        public async Task<DistrictResponse?> GetDistrictDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<DistrictResponse>("GetDistrictDetailsById", queryParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<SelectListResponse>> GetDistrictDetailsByStateId(long stateId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@StateId", stateId);
            return (await ListByStoredProcedure<SelectListResponse>("GetDistrictDetailsByStateId", queryParameters));
        }
        public async Task<IEnumerable<DistrictDataValidationErrors>> ImportDistrictsDetails(List<ImportedDistrictDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlDistrictData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlDistrictData", xmlDistrictData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<DistrictDataValidationErrors>("SaveImportDistrictDetails", queryParameters);
        }
        #endregion

        #region City
        public async Task<IEnumerable<CityResponse>> GetCityList(SearchCityRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@CityName", parameters.CityName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<CityResponse>("GetCities", queryParameters);
        }

        public async Task<int> SaveCity(CityRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CityId", parameters.CityId);
            //queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@CityName", parameters.CityName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveCityDetails", queryParameters);
        }

        public async Task<CityResponse?> GetCityDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<CityResponse>("GetCityDetailsById", queryParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<SelectListResponse>> GetCityDetailsByDistrictId(long districtId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@DistrictId", districtId);
            return (await ListByStoredProcedure<SelectListResponse>("GetCityDetailsDistrictById", queryParameters));
        }

        public async Task<IEnumerable<CityDataValidationErrors>> ImportCityDetails(List<ImportedCityDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlDistrictData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlDistrictData", xmlDistrictData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<CityDataValidationErrors>("SaveImportCityDetails", queryParameters);
        }
        #endregion

        #region Area
        public async Task<IEnumerable<AreaResponse>> GetAreasList(SearchAreaRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@AreaName", parameters.AreaName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<AreaResponse>("GetAreas", queryParameters);
        }
        public async Task<int> SaveArea(AreaRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@AreaId", parameters.AreaId);
          //  queryParameters.Add("@CityId", parameters.CityId);
            queryParameters.Add("@AreaName", parameters.AreaName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveAreaDetails", queryParameters);
        }

        public async Task<AreaResponse?> GetAreaDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<AreaResponse>("GetAreaDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<SelectListResponse>> GetAreaDetailsByCityId(long cityId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CityId", cityId);
            return (await ListByStoredProcedure<SelectListResponse>("GetAreaDetailsByCityId", queryParameters));
        }
        public async Task<IEnumerable<AreaDataValidationErrors>> ImportAreasDetails(List<ImportedAreaDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlAreaData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlAreaData", xmlAreaData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<AreaDataValidationErrors>("SaveImportAreaDetails", queryParameters);
        }
        #endregion

        #region Mapping
        public async Task<IEnumerable<AreaMappingResponse>> GetAreaTerritoryList(SearchAreaMappingRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);
            return await ListByStoredProcedure<AreaMappingResponse>("GetAreasMapping", queryParameters);
        }
        public async Task<int> SaveareaTerritory(SaveAreamapping parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RegionId", parameters.RegionId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@CityId", parameters.CityId);
            queryParameters.Add("@AreaId", parameters.AreaId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveareaTerritoryDetails", queryParameters);
        }

        public async Task<AreaMappingResponse?> GetAreaMappingDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<AreaMappingResponse>("GetAreaMappingDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion
    }
}
