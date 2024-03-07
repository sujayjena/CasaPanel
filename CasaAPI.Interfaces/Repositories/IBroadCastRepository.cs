using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Repositories
{
    public interface IBroadCastRepository
    {
        #region Catalog API

        Task<IEnumerable<CatalogResponse>> GetCatalogDetailsList(SearchCatalogRequest request);
        Task<int> SaveCatalogDetails(CatalogRequest parameters);
        Task<CatalogResponse?> GetCatalogDetailsById(long id);
        Task<IEnumerable<CollectionResponseModel>> GetBroadCastCollectionNameList();

        #endregion

        #region Catalog Related API

        Task<IEnumerable<CatalogRelatedResponse>> GetCatalogRelatedList(SearchCatalogRelatedRequest request);
        Task<int> SaveCatalogRelatedDetails(CatalogRelatedRequest parameters);
        Task<CatalogRelatedResponse?> GetCatalogRelatedListById(long id);

        #endregion

        #region Project API

        Task<IEnumerable<ProjectResponse>> GetProjectList(SearchProjectRequest request);
        Task<int> SaveProject(ProjectRequest parameters);
        Task<ProjectResponse?> GetProjectDetailsById(long id);

        #endregion
    }
}
