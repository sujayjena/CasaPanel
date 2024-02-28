using CasaAPI.Models;
using Models;

namespace Interfaces.Repositories
{
    public interface IManageTerritoryRepository
    {
        Task<IEnumerable<StateResponse>> GetStatesList(SearchStateRequest request);
        Task<int> SaveState(StateRequest stateRequest);
        Task<IEnumerable<StateDataValidationErrors>> ImportStatesDetails(List<ImportedStateDetails> parameters);
        Task<StateResponse?> GetStateDetailsById(long id);
        Task<IEnumerable<SelectListResponse>> GetStateDetailsByRegionId(long regionId);

        #region Region
        Task<IEnumerable<RegionResponse>> GetRegionsList(SearchRegionRequest request);
        Task<int> SaveRegion(RegionRequest regionRequest);
        Task<RegionResponse?> GetRegionDetailsById(long id);
        Task<IEnumerable<RegionDataValidationErrors>> ImportRegionsDetails(List<ImportedRegionDetails> parameters);
        #endregion

        #region District
        Task<IEnumerable<DistrictResponse>> GetDistrictsList(SearchDistrictRequest request);
        Task<int> SaveDistrict(DistrictRequest districtRequest);
        Task<DistrictResponse?> GetDistrictDetailsById(long id);
        Task<IEnumerable<SelectListResponse>> GetDistrictDetailsByStateId(long stateId);
        Task<IEnumerable<DistrictDataValidationErrors>> ImportDistrictsDetails(List<ImportedDistrictDetails> parameters);

        #endregion

        #region City
        Task<IEnumerable<CityResponse>> GetCityList(SearchCityRequest request);
        Task<int> SaveCity(CityRequest cityRequest);
        Task<CityResponse?> GetCityDetailsById(long id);
        Task<IEnumerable<SelectListResponse>> GetCityDetailsByDistrictId(long districtId);
        Task<IEnumerable<CityDataValidationErrors>> ImportCityDetails(List<ImportedCityDetails> parameters);

        #endregion

        #region Area
        Task<IEnumerable<AreaResponse>> GetAreasList(SearchAreaRequest request);
        Task<int> SaveArea(AreaRequest areaRequest);
        Task<AreaResponse?> GetAreaDetailsById(long id);
        Task<IEnumerable<SelectListResponse>> GetAreaDetailsByCityId(long cityId);
        Task<IEnumerable<AreaDataValidationErrors>> ImportAreasDetails(List<ImportedAreaDetails> parameters);


        #endregion

        #region Mapping
        Task<IEnumerable<AreaMappingResponse>> GetAreaTerritoryList(SearchAreaMappingRequest parameters);
        Task<int> SaveareaTerritory(SaveAreamapping parameters);
        Task<AreaMappingResponse?> GetAreaMappingDetailsById(long id);
        #endregion
    }
}
