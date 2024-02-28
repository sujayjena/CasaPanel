﻿using CasaAPI.Models;
using Interfaces.Repositories;
using Interfaces.Services;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ManageTerritoryService : IManageTerritoryService
    {
        private IManageTerritoryRepository _manageTerritorRepository;

        public ManageTerritoryService(IManageTerritoryRepository manageTerritorRepository)
        {
            _manageTerritorRepository = manageTerritorRepository;
        }
        #region State
        public async Task<IEnumerable<StateResponse>> GetStatesList(SearchStateRequest request)
        {
            return await _manageTerritorRepository.GetStatesList(request);
        }
        public async Task<int> SaveState(StateRequest stateRequest)
        {
            return await _manageTerritorRepository.SaveState(stateRequest);
        }
        public async Task<IEnumerable<StateDataValidationErrors>> ImportStatesDetails(List<ImportedStateDetails> request)
        {
            return await _manageTerritorRepository.ImportStatesDetails(request);
        }
        public async Task<StateResponse?> GetStateDetailsById(long id)
        {
            return await _manageTerritorRepository.GetStateDetailsById(id);
        }
        public async Task<IEnumerable<SelectListResponse>> GetStateDetailsByRegionId(long regionId)
        {
            return await _manageTerritorRepository.GetStateDetailsByRegionId(regionId);
        }
        #endregion

        #region Region
        public async Task<IEnumerable<RegionResponse>> GetRegionsList(SearchRegionRequest request)
        {
            return await _manageTerritorRepository.GetRegionsList(request);
        }
        public async Task<int> SaveRegion(RegionRequest regionRequest)
        {
            return await _manageTerritorRepository.SaveRegion(regionRequest);
        }
        public async Task<RegionResponse?> GetRegionDetailsById(long id)
        {
            return await _manageTerritorRepository.GetRegionDetailsById(id);
        }
        public async Task<IEnumerable<RegionDataValidationErrors>> ImportRegionsDetails(List<ImportedRegionDetails> request)
        {
            return await _manageTerritorRepository.ImportRegionsDetails(request);
        }
        #endregion

        #region District
        public async Task<IEnumerable<DistrictResponse>> GetDistrictsList(SearchDistrictRequest request)
        {
            return await _manageTerritorRepository.GetDistrictsList(request);
        }
        public async Task<int> SaveDistrict(DistrictRequest districtRequest)
        {
            return await _manageTerritorRepository.SaveDistrict(districtRequest);
        }
        public async Task<DistrictResponse?> GetDistrictDetailsById(long id)
        {
            return await _manageTerritorRepository.GetDistrictDetailsById(id);
        }
        
        public async Task<IEnumerable<SelectListResponse>> GetDistrictDetailsByStateId(long stateId)
        {
            return await _manageTerritorRepository.GetDistrictDetailsByStateId(stateId);
        }
        public async Task<IEnumerable<DistrictDataValidationErrors>> ImportDistrictsDetails(List<ImportedDistrictDetails> request)
        {
            return await _manageTerritorRepository.ImportDistrictsDetails(request);
        }
        #endregion

        #region City
        public async Task<IEnumerable<CityResponse>> GetCityList(SearchCityRequest request)
        {
            return await _manageTerritorRepository.GetCityList(request);
        }
        public async Task<int> SaveCity(CityRequest cityRequest)
        {
            return await _manageTerritorRepository.SaveCity(cityRequest);
        }
        public async Task<CityResponse?> GetCityDetailsById(long id)
        {
            return await _manageTerritorRepository.GetCityDetailsById(id);
        }
        public async Task<IEnumerable<SelectListResponse>> GetCityDetailsByDistrictId(long districtId)
        {
            return await _manageTerritorRepository.GetCityDetailsByDistrictId(districtId);
        }
        public async Task<IEnumerable<CityDataValidationErrors>> ImportCityDetails(List<ImportedCityDetails> request)
        {
            return await _manageTerritorRepository.ImportCityDetails(request);
        }
        #endregion

        #region Area
        public async Task<IEnumerable<AreaResponse>> GetAreasList(SearchAreaRequest request)
        {
            return await _manageTerritorRepository.GetAreasList(request);
        }
        public async Task<int> SaveArea(AreaRequest areaRequest)
        {
            return await _manageTerritorRepository.SaveArea(areaRequest);
        }
        public async Task<AreaResponse?> GetAreaDetailsById(long id)
        {
            return await _manageTerritorRepository.GetAreaDetailsById(id);
        }

        public async Task<IEnumerable<SelectListResponse>> GetAreaDetailsByCityId(long cityId)
        {
            return await _manageTerritorRepository.GetAreaDetailsByCityId(cityId);
        }
        public async Task<IEnumerable<AreaDataValidationErrors>> ImportAreasDetails(List<ImportedAreaDetails> request)
        {
            return await _manageTerritorRepository.ImportAreasDetails(request);
        }
        #endregion

        #region Mappimg
        public async Task<IEnumerable<AreaMappingResponse>> GetAreaTerritoryList(SearchAreaMappingRequest request)
        {
            return await _manageTerritorRepository.GetAreaTerritoryList(request);
        }
        public async Task<int> SaveareaTerritory(SaveAreamapping stateRequest)
        {
            return await _manageTerritorRepository.SaveareaTerritory(stateRequest);
        }

        public async Task<AreaMappingResponse?> GetAreaMappingDetailsById(long id)
        {
            return await _manageTerritorRepository.GetAreaMappingDetailsById(id);
        }
        #endregion
    }
}
