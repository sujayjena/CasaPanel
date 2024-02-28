using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Repositories
{
    public class PortfolioRepository : BaseRepository, IPortfolioRepository
    {
        private IConfiguration _configuration;
        public PortfolioRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
        #region Portfolio
        public async Task<int> SavePortfolio(PortfolioSaveParameters parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@CompanyId", parameters?.CompanyId);
            queryParameters.Add("@DesignId", parameters?.DesignId);
queryParameters.Add("@CollectionId", parameters?.CollectionId);
queryParameters.Add("@CatregoryId", parameters?.CatregoryId);
queryParameters.Add("@TypeId", parameters?.TypeId);
queryParameters.Add("@PunchId", parameters?.PunchId);
queryParameters.Add("@SurfaceId", parameters?.SurfaceId);
queryParameters.Add("@BrandId", parameters?.BrandId);
queryParameters.Add("@TileSizeId", parameters?.TileSizeId);
queryParameters.Add("@NoOfTilesPerBox", parameters?.NoOfTilesPerBox.SanitizeValue());
queryParameters.Add("@WeightPerBox", parameters?.WeightPerBox.SanitizeValue());
queryParameters.Add("@ThicknessId", parameters?.ThicknessId);
queryParameters.Add("@CoverageAreaperBoxFoot", parameters?.CoverageAreaperBoxFoot.SanitizeValue());
queryParameters.Add("@CoverageAreaPerBoxMerte", parameters?.CoverageAreaPerBoxMerte.SanitizeValue());
queryParameters.Add("@ImageUpload", parameters?.ImageUpload.SanitizeValue());
queryParameters.Add("@Remarks", parameters?.Remarks.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SavePortfolioDetails", queryParameters);
        }

        public async Task<IEnumerable<PortfolioDetailsResponse>> GetPortfolioList(PortfolioSearchParameters parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<PortfolioDetailsResponse>("GetPortfolioList", queryParameters);
        }
        public async Task<PortfolioDetailsResponse?> GetPortfolioDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<PortfolioDetailsResponse>("GetPortfolioDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion
    }
}
