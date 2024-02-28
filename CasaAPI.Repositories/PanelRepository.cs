using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models.Enums;
using CasaAPI.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Repositories
{
    public class PanelRepository : BaseRepository, IPanelRepository
    {
        private IConfiguration _configuration;
        public PanelRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
        #region PanelDisplay
        public async Task<int> SavePanelDisplay(PanelDisplaySaveParameters parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@PanelCode", parameters?.PanelCode.SanitizeValue());
            queryParameters.Add("@DesignInfromation", parameters?.DesignInfromation.SanitizeValue());
            queryParameters.Add("@Collection", parameters?.Collection);
            queryParameters.Add("@Punch", parameters?.Punch);
            queryParameters.Add("@Thickness", parameters?.Thickness);
            queryParameters.Add("@Size", parameters?.Size);
            queryParameters.Add("@FullPieceQty", parameters?.FullPieceQty);
            queryParameters.Add("@CutPieceQty", parameters?.CutPieceQty);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SavePanelDisplayDetails", queryParameters);
        }

        public async Task<IEnumerable<PanelDisplayDetailsResponse>> GetPanelDisplayList(PanelDisplaySearchParameters parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<PanelDisplayDetailsResponse>("GetPanelDisplayList", queryParameters);
        }
        public async Task<PanelDisplayDetailsResponse?> GetPanelDisplayDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<PanelDisplayDetailsResponse>("GetPanelDisplayDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region PanelInventoryIn
        public async Task<int> SavePanelInventoryIn(PanelInventoryInSaveParameters parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@VendorName", parameters.VendorName);
            queryParameters.Add("@Collection", parameters.Collection);
            queryParameters.Add("@CuttingSize", parameters.CuttingSize);
            queryParameters.Add("@Thickness", parameters.Thickness);
            queryParameters.Add("@EmailId", parameters.EmailId);
            queryParameters.Add("@MobileNumber", parameters.MobileNumber);
            queryParameters.Add("@Finish", parameters.Finish);
            queryParameters.Add("@Type", parameters.Type);
            queryParameters.Add("@InwardingDate", parameters.InwardingDate);
            queryParameters.Add("@InwardingQty", parameters.InwardingQty);
            queryParameters.Add("@OrderId", parameters.OrderId);
            queryParameters.Add("@TelephoneNumber", parameters.TelephoneNumber);
            queryParameters.Add("@TotalStock", parameters.TotalStock);
            queryParameters.Add("@TotalPieces", parameters.TotalPieces);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SavePanelInventoryInDetails", queryParameters);
        }

        public async Task<IEnumerable<PanelInventoryInDetailsResponse>> GetPanelInventoryInList(PanelInventoryInSearchParameters parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<PanelInventoryInDetailsResponse>("GetPanelInventoryInList", queryParameters);
        }
        public async Task<PanelInventoryInDetailsResponse?> GetPanelInventoryInDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<PanelInventoryInDetailsResponse>("GetPanelInventoryInDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region PanelInventoryOut
        public async Task<int> SavePanelInventoryOut(PanelInventoryOutSaveParameters parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@Collection", parameters.Collection);
            queryParameters.Add("@CuttingSize", parameters.CuttingSize);
            queryParameters.Add("@Thickness", parameters.Thickness);
            queryParameters.Add("@Desing", parameters.Desing);
            queryParameters.Add("@Finish", parameters.Finish);
            queryParameters.Add("@Type", parameters.Type);
            queryParameters.Add("@OutwardingDate", parameters.OutwardingDate);
            queryParameters.Add("@OutwardingQty", parameters.OutwardingQty);
            queryParameters.Add("@OrderId", parameters.OrderId);
            queryParameters.Add("@TotalStock", parameters.TotalStock);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SavePanelInventoryOutDetails", queryParameters);
        }

        public async Task<IEnumerable<PanelInventoryOutDetailsResponse>> GetPanelInventoryOutList(PanelInventoryOutSearchParameters parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<PanelInventoryOutDetailsResponse>("GetPanelInventoryOutList", queryParameters);
        }
        public async Task<PanelInventoryOutDetailsResponse?> GetPanelInventoryOutDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<PanelInventoryOutDetailsResponse>("GetPanelInventoryOutDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion
    }
}
