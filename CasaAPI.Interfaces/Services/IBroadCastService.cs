using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Services
{
    public interface IBroadCastService
    {
        #region Catalog API Service Interface

        Task<IEnumerable<CatalogResponse>> GetCatalogDetailsList(SearchCatalogRequest request);
        Task<int> SaveCatalogDetails(CatalogRequest parameters);
        Task<CatalogDetailsResponse?> GetCatalogDetailsById(long id);
        Task<IEnumerable<CollectionResponseModel>> GetBroadCastCollectionNameList();

        #endregion

        #region Catalog Related API Service Interface

        Task<IEnumerable<CatalogRelatedResponse>> GetCatalogRelatedList(SearchCatalogRelatedRequest request);
        Task<int> SaveCatalogRelatedDetails(CatalogRelatedRequest parameters);
        Task<CatalogRelatedDetailsResponse?> GetCatalogRelatedListById(long id);

        #endregion

        #region Project API Service Interface

        Task<IEnumerable<ProjectResponse>> GetProjectList(SearchProjectRequest request);
        Task<int> SaveProject(ProjectRequest parameters);
        Task<ProjectDetailsResponse?> GetProjectDetailsById(long id);

        #endregion
    }
}
