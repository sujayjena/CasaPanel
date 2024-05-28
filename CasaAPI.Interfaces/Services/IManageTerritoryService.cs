using CasaAPI.Models;

namespace CasaAPI.Interfaces.Services
{
    public interface IManageTerritoryService
    {
        Task<IEnumerable<StateResponse>> GetStatesList(SearchStateRequest request);
        Task<int> SaveState(StateRequest stateRequest);
        Task<IEnumerable<StateDataValidationErrors>> ImportStatesDetails(List<ImportedStateDetails> request);
        Task<StateResponse?> GetStateDetailsById(long id);
        Task<IEnumerable<SelectListResponse>> GetStateDetailsByRegionId(long regionId);
        Task<IEnumerable<RegionResponse>> GetRegionsList(SearchRegionRequest request);
        Task<int> SaveRegion(RegionRequest regionRequest);
        Task<RegionResponse?> GetRegionDetailsById(long id);
        Task<IEnumerable<RegionDataValidationErrors>> ImportRegionsDetails(List<ImportedRegionDetails> request);

        Task<IEnumerable<DistrictResponse>> GetDistrictsList(SearchDistrictRequest request);
        Task<int> SaveDistrict(DistrictRequest districtRequest);
        Task<DistrictResponse?> GetDistrictDetailsById(long id);
        Task<IEnumerable<SelectListResponse>> GetDistrictDetailsByStateId(long stateId);
        Task<IEnumerable<DistrictDataValidationErrors>> ImportDistrictsDetails(List<ImportedDistrictDetails> request);

        Task<IEnumerable<CityResponse>> GetCityList(SearchCityRequest request);
        Task<int> SaveCity(CityRequest cityRequest);
        Task<CityResponse?> GetCityDetailsById(long id);
        Task<IEnumerable<SelectListResponse>> GetCityDetailsByDistrictId(long districtId);
        Task<IEnumerable<CityDataValidationErrors>> ImportCityDetails(List<ImportedCityDetails> request);

        Task<IEnumerable<AreaResponse>> GetAreasList(SearchAreaRequest request);
        Task<int> SaveArea(AreaRequest areaRequest);
        Task<AreaResponse?> GetAreaDetailsById(long id);
        Task<IEnumerable<SelectListResponse>> GetAreaDetailsByCityId(long cityId);
        Task<IEnumerable<AreaDataValidationErrors>> ImportAreasDetails(List<ImportedAreaDetails> request);

        #region Mapping
        Task<IEnumerable<AreaMappingResponse>> GetAreaTerritoryList(SearchAreaMappingRequest request);
        Task<int> SaveareaTerritory(SaveAreamapping stateRequest);
        Task<AreaMappingResponse?> GetAreaMappingDetailsById(long id);
        Task<IEnumerable<Territories_State_Dist_City_Area_Response>> GetTerritories_State_Dist_City_Area_List_ById(Territories_State_Dist_City_Area_Search request);
        #endregion
    }
}
