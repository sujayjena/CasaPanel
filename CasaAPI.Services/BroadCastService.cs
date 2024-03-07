using CasaAPI.Helpers;
using CasaAPI.Models;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Services
{
    public class BroadCastService : IBroadCastService
    {
        private IBroadCastRepository _broadCastRepository;
        private IFileManager _fileManager;

        public BroadCastService(IBroadCastRepository broadCastRepository, IFileManager fileManager)
        {
            _broadCastRepository = broadCastRepository;
            _fileManager = fileManager;
        }

        #region Catelog API
        public async Task<IEnumerable<CatalogResponse>> GetCatalogDetailsList(SearchCatalogRequest request)
        {
            return await _broadCastRepository.GetCatalogDetailsList(request);
        }
        public async Task<int> SaveCatalogDetails(CatalogRequest catalogRequest)
        {
            return await _broadCastRepository.SaveCatalogDetails(catalogRequest);
        }
        public async Task<CatalogDetailsResponse?> GetCatalogDetailsById(long id)
        {
            CatalogDetailsResponse objCatalogDetailsResponse = new CatalogDetailsResponse();
            CatalogResponse? data = await _broadCastRepository.GetCatalogDetailsById(id);

            if (data != null)
            {
                objCatalogDetailsResponse.catalogDetails = data;

                objCatalogDetailsResponse.catalogDetails.ImageFile = _fileManager.GetCatalogDocuments(objCatalogDetailsResponse.catalogDetails.ImageSavedFileName);
                objCatalogDetailsResponse.catalogDetails.CatalogFile = _fileManager.GetCatalogDocuments(objCatalogDetailsResponse.catalogDetails.CatalogSavedFileName);
            }

            return objCatalogDetailsResponse;
        }

        public async Task<IEnumerable<CollectionResponseModel>> GetBroadCastCollectionNameList()
        {
            return await _broadCastRepository.GetBroadCastCollectionNameList();
        }

        #endregion

        #region Catelog Related API
        public async Task<IEnumerable<CatalogRelatedResponse>> GetCatalogRelatedList(SearchCatalogRelatedRequest request)
        {
            return await _broadCastRepository.GetCatalogRelatedList(request);
        }
        public async Task<int> SaveCatalogRelatedDetails(CatalogRelatedRequest catalogRelatedRequest)
        {
            return await _broadCastRepository.SaveCatalogRelatedDetails(catalogRelatedRequest);
        }
        public async Task<CatalogRelatedDetailsResponse?> GetCatalogRelatedListById(long id)
        {
            CatalogRelatedDetailsResponse objCatalogRelatedDetailsResponse = new CatalogRelatedDetailsResponse();
            CatalogRelatedResponse? data = await _broadCastRepository.GetCatalogRelatedListById(id);

            if (data != null)
            {
                objCatalogRelatedDetailsResponse.catalogRelatedDetails = data;

                //objCatalogRelatedDetailsResponse.catalogRelatedDetails.ImageFile = _fileManager.GetCatalogRelatedDocuments(objCatalogRelatedDetailsResponse.catalogRelatedDetails.ImageSavedFileName);

                objCatalogRelatedDetailsResponse.catalogRelatedDetails.ImageFile = _fileManager.GetDesignFiles(objCatalogRelatedDetailsResponse.catalogRelatedDetails.ImageSavedFileName);
            }

            return objCatalogRelatedDetailsResponse;
        }
        #endregion

        #region Project API

        public async Task<IEnumerable<ProjectResponse>> GetProjectList(SearchProjectRequest request)
        {
            return await _broadCastRepository.GetProjectList(request);
        }
        public async Task<int> SaveProject(ProjectRequest projectRequest)
        {
            return await _broadCastRepository.SaveProject(projectRequest);
        }
        public async Task<ProjectDetailsResponse?> GetProjectDetailsById(long id)
        {
            ProjectDetailsResponse objCatalogDetailsResponse = new ProjectDetailsResponse();
            ProjectResponse? data = await _broadCastRepository.GetProjectDetailsById(id);

            if (data != null)
            {
                objCatalogDetailsResponse.ProjectDetails = data;

                var files= _fileManager.GetProjectDocuments(objCatalogDetailsResponse.ProjectDetails.ProjectSavedFileName);
                objCatalogDetailsResponse.ProjectDetails.ProjectFile = files;
            }

            return objCatalogDetailsResponse;
        }

        #endregion
    }
}
