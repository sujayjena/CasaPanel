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

        #region Order
        public async Task<int> SaveOrder(OrderSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@OrderDate", parameters.OrderDate);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@CustomerId", parameters.CustomerId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@RegionId", parameters.RegionId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@AreaId", parameters.AreaId);
            queryParameters.Add("@CityId", parameters.CityId);
            queryParameters.Add("@Pincode", parameters.Pincode);
            queryParameters.Add("@BrandId", parameters.BrandId);
            queryParameters.Add("@PanelQty", parameters.PanelQty);
            queryParameters.Add("@BinderQty", parameters.BinderQty);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@GrandTotalQty", parameters.GrandTotalQty);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveOrder", queryParameters);
        }

        public async Task<IEnumerable<OrderListResponse>> GetOrderList(OrderSearchParameters parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@CustomerId", parameters.CustomerId);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@BrandId", parameters.BrandId);
            queryParameters.Add("@CollectionId", parameters.CollectionId);
            queryParameters.Add("@BaseDesignId", parameters.BaseDesignId);
            queryParameters.Add("@SizeId", parameters.SizeId);
            queryParameters.Add("@FilterType", parameters.FilterType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<OrderListResponse>("GetOrderList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<OrderListResponse?> GetOrderById(int id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<OrderListResponse>("GetOrderById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> SaveOrderDetails(OrderDetailsSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@OrderId", parameters.OrderId);
            queryParameters.Add("@CollectionId", parameters.CollectionId);
            queryParameters.Add("@BaseDesignId", parameters.BaseDesignId);
            queryParameters.Add("@SizeId", parameters.SizeId);
            queryParameters.Add("@SurfaceId", parameters.SurfaceId);
            queryParameters.Add("@ThicknessId", parameters.ThicknessId);
            queryParameters.Add("@Quantity", parameters.Quantity);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveOrderDetails", queryParameters);
        }

        public async Task<IEnumerable<OrderDetailsResponse>> GetOrderDetailsList(OrderDetailsSearchParameters parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@OrderId", parameters.OrderId);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<OrderDetailsResponse>("GetOrderDetailsList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        #endregion

        #region Order Booking
        public async Task<int> SaveOrderBooking(OrderBooking_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@CollectionId", parameters.CollectionId);
            queryParameters.Add("@BaseDesignId", parameters.BaseDesignId);
            queryParameters.Add("@SizeId", parameters.SizeId);
            queryParameters.Add("@SurfaceId", parameters.SurfaceId);
            queryParameters.Add("@ThicknessId", parameters.ThicknessId);
            queryParameters.Add("@ImageFileName", parameters.ImageFileName);
            queryParameters.Add("@ImageOriginalFileName", parameters.ImageOriginalFileName);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveOrderBooking", queryParameters);
        }

        public async Task<IEnumerable<OrderBooking_Response>> GetOrderBookingList(OrderBooking_Search parameters)
        {

            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@CollectionId", parameters.CollectionId);
            queryParameters.Add("@BaseDesignId", parameters.BaseDesignId);
            queryParameters.Add("@SizeId", parameters.SizeId);
            queryParameters.Add("@SurfaceId", parameters.SurfaceId);
            queryParameters.Add("@ThicknessId", parameters.ThicknessId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<OrderBooking_Response>("GetOrderBookingList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<OrderBooking_Response?> GetOrderBookingById(int id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<OrderBooking_Response>("GetOrderBookingById", queryParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<OrderBooking_Collection_BaseDesign_Size_Surface_Response>> GetOrderBooking_Collection_BaseDesign_Size_Surface_List_ById(OrderBooking_Collection_BaseDesign_Size_Surface_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CollectionId", parameters.CollectionId);
            queryParameters.Add("@BaseDesignId", parameters.BaseDesignId);
            queryParameters.Add("@SizeId", parameters.SizeId);
            queryParameters.Add("@SurfaceId", parameters.SurfaceId);

            var result = await ListByStoredProcedure<OrderBooking_Collection_BaseDesign_Size_Surface_Response>("GetOrderBooking_Collection_BaseDesign_Size_Surface_List_ById", queryParameters);

            return result;
        }
        #endregion
    }
}
