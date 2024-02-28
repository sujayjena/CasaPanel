using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        private IConfiguration _configuration;
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
        #region DispatchOrder
        public async Task<int> SaveDispatchOrder(DispatchOrderSaveParameters parameters)
        {

            string xmlDispatchOrder;
            DynamicParameters queryParameters = new DynamicParameters();

            xmlDispatchOrder = ConvertListToXml(parameters?.PanelDisplayLists);
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@DONumber", parameters?.DONumber);
queryParameters.Add("@CSPCode", parameters?.CSPCode);
queryParameters.Add("@DealerId", parameters?.DealerId);
queryParameters.Add("@StateId", parameters?.StateId);
//queryParameters.Add("@DistrictId", parameters.DistrictId);
queryParameters.Add("@CityId", parameters?.CityId);
//queryParameters.Add("@AreaId", parameters.AreaId);
queryParameters.Add("@ContactPerson", parameters?.ContactPerson);
queryParameters.Add("@RefBy", parameters?.RefBy);
queryParameters.Add("@CaseDisplayPanel", parameters?.CaseDisplayPanel);
queryParameters.Add("@CaseFullTileBoxes", parameters?.CaseFullTileBoxes);
            queryParameters.Add("@CaseCttingSampleBoxes", parameters?.CaseCttingSampleBoxes);
            queryParameters.Add("@DispatchedTruckNumber", parameters?.DispatchedTruckNumber);
queryParameters.Add("@DriverId", parameters?.DriverId);
queryParameters.Add("@DriverContactNumber", parameters?.DriverContactNumber);
queryParameters.Add("@LoadingTime", parameters?.LoadingTime);
queryParameters.Add("@MasterName", parameters?.MasterName);
queryParameters.Add("@MasterNumber", parameters?.MasterNumber);
queryParameters.Add("@PanelCode", parameters?.PanelCode);
queryParameters.Add("@Qty", parameters?.Qty);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@XmlDispatchOrder", xmlDispatchOrder);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveDispatchOrder", queryParameters);
        }

        public async Task<IEnumerable<DispatchOrderDetailsResponse>> GetDispatchOrderList(DispatchOrderSearchParameters parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<DispatchOrderDetailsResponse>("GetDispatchOrderList", queryParameters);
        }
        public async Task<DispatchOrderDetailsResponse?> GetDispatchOrderDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<DispatchOrderDetailsResponse>("GetDispatchOrderDetailsById", queryParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<DispatchPanelDisplayDetailsResponse>> GetDispatchPanelDisplayOrderList(long dispatchOrderId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@DispatchOrderId", dispatchOrderId);
            return await ListByStoredProcedure<DispatchPanelDisplayDetailsResponse>("GetDispatchPanelDisplayOrderList", queryParameters);
        }
       

        #endregion
    }
}
