using CasaAPI.Helpers;
using CasaAPI.Models;
using CasaAPI.Repositories;
using Dapper;
using Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace Repositories
{
    public class BroadCastRepository : BaseRepository, IBroadCastRepository
    {
        private IConfiguration _configuration;

        public BroadCastRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Catalog API

        public async Task<IEnumerable<CatalogResponse>> GetCatalogDetailsList(SearchCatalogRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            //queryParameters.Add("@CollectionName", parameters.CollectionName.SanitizeValue());
            queryParameters.Add("@SearchValue", parameters.SearchValue.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@AppType", parameters.AppType.SanitizeValue());
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<CatalogResponse>("GetCatalogDetailsList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<int> SaveCatalogDetails(CatalogRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CatalogId", parameters.CatalogId);
            queryParameters.Add("@LaunchDate", parameters.LaunchDate);
            queryParameters.Add("@CollectionId", parameters.CollectionId);
            queryParameters.Add("@Remark", parameters.Remark);
            queryParameters.Add("@Status", parameters.Status);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@ImageFileName", parameters.ImageFileName.SanitizeValue());
            queryParameters.Add("@ImageSavedFileName", parameters.ImageSavedFileName.SanitizeValue());
            queryParameters.Add("@CatalogFileName", parameters.CatalogFileName.SanitizeValue());
            queryParameters.Add("@CatalogSavedFileName", parameters.CatalogSavedFileName.SanitizeValue());
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveCatalogDetails", queryParameters);
        }
        public async Task<CatalogResponse?> GetCatalogDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<CatalogResponse>("GetCatalogDetailsById", queryParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<CollectionResponseModel>> GetBroadCastCollectionNameList()
        {
            var result = await ListByStoredProcedure<CollectionResponseModel>("GetBroadcastCollectionNameList");
            return result;
        }

        #endregion

        #region Catalog Related API

        public async Task<IEnumerable<CatalogRelatedResponse>> GetCatalogRelatedList(SearchCatalogRelatedRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@SearchValue", parameters.SearchValue.SanitizeValue());
            queryParameters.Add("@CatalogId", parameters.CatalogId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<CatalogRelatedResponse>("GetCatalogRelatedList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<int> SaveCatalogRelatedDetails(CatalogRelatedRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CatalogRelatedId", parameters.CatalogRelatedId);
            queryParameters.Add("@CatalogId", parameters.CatalogId);
            queryParameters.Add("@CollectionId", parameters.CollectionId);
            queryParameters.Add("@CategoryId", parameters.CategoryId);
            queryParameters.Add("@BaseDesignId", parameters.BaseDesignId);
            queryParameters.Add("@SizeId", parameters.SizeId);
            queryParameters.Add("@SeriesId", parameters.SeriesId);
            queryParameters.Add("@DesignCode", parameters.DesignCode);
            queryParameters.Add("@DesignSubCode", parameters.DesignSubCode);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@ImageFileName", parameters.ImageFileName.SanitizeValue());
            queryParameters.Add("@ImageSavedFileName", parameters.ImageSavedFileName.SanitizeValue());
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveCatalogRelatedDetails", queryParameters);
        }
        public async Task<CatalogRelatedResponse?> GetCatalogRelatedListById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<CatalogRelatedResponse>("GetCatalogRelatedListById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Project API

        public async Task<IEnumerable<ProjectResponse>> GetProjectList(SearchProjectRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<ProjectResponse>("GetProjectList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<int> SaveProject(ProjectRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@ProjectId", parameters.ProjectId);
            queryParameters.Add("@ProjectName", parameters.ProjectName);
            queryParameters.Add("@Description", parameters.Description);
            queryParameters.Add("@CompletionDate", parameters.CompletionDate);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@ProjectFileName", parameters.ProjectFileName.SanitizeValue());
            queryParameters.Add("@ProjectSavedFileName", parameters.ProjectSavedFileName.SanitizeValue()); 
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SP_SaveProject", queryParameters);
        }
        public async Task<ProjectResponse?> GetProjectDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<ProjectResponse>("GetProjectDetailsById", queryParameters)).FirstOrDefault();
        }

        #endregion
    }
}
