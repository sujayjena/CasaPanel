using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Models;
using CasaAPI.Models.Enums;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Repositories
{
    public class DealerRepository : BaseRepository, IDealerRepository
    {
        private IConfiguration _configuration;
        public DealerRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
        #region Dealer
        public async Task<int> SaveDealer(DealerSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@CompanyName", parameters?.CompanyName.SanitizeValue());
            queryParameters.Add("@CompanyEmailId", parameters?.CompanyEmailId.SanitizeValue());
            queryParameters.Add("@GSTNumber", parameters?.GSTNumber.SanitizeValue());
            queryParameters.Add("@PANNumber", parameters?.PANNumber.SanitizeValue());
            queryParameters.Add("@AadhaarNumber", parameters?.AadhaarNumber);
            queryParameters.Add("@BusinessCardUpload", parameters?.BusinessCardUpload.SanitizeValue());
            queryParameters.Add("@CompanyDealingaddress", parameters?.CompanyDealingaddress.SanitizeValue());
            queryParameters.Add("@State", parameters?.State);
            queryParameters.Add("@Region", parameters?.Region);
            queryParameters.Add("@District", parameters?.District);
            queryParameters.Add("@City", parameters?.City);
            queryParameters.Add("@Area", parameters?.Area);
            queryParameters.Add("@Pincode", parameters?.Pincode);
            queryParameters.Add("@FirstName", parameters?.FirstName.SanitizeValue());
            queryParameters.Add("@LastName", parameters?.LastName.SanitizeValue());
            queryParameters.Add("@ContactType", parameters?.ContactType);
            queryParameters.Add("@MobileNumber", parameters?.MobileNumber);
            queryParameters.Add("@EmailId", parameters?.EmailId.SanitizeValue());
            queryParameters.Add("@EmergencyContactNumber", parameters?.EmergencyContactNumber);
            queryParameters.Add("@DateofBirth", parameters?.DateofBirth);
            queryParameters.Add("@BloodGroup", parameters?.BloodGroup);
            queryParameters.Add("@Gender", parameters?.Gender);
            queryParameters.Add("@Anniversarydate", parameters?.Anniversarydate);
            queryParameters.Add("@CurrentHandlingBrandOrCompany", parameters?.CurrentHandlingBrandOrCompany);
            queryParameters.Add("@TileType", parameters?.TileType);
            queryParameters.Add("@SegmentType", parameters?.SegmentType);
            queryParameters.Add("@AnyOtherBranchDealing", parameters?.AnyOtherBranchDealing);
            queryParameters.Add("@PresentShowroomSqFt", parameters?.PresentShowroomSqFt);
            queryParameters.Add("@PresentGodownSqFt", parameters?.PresentGodownSqFt);
            queryParameters.Add("@DistanceFromShowroomToGodown", parameters?.DistanceFromShowroomToGodown);
            queryParameters.Add("@Showroom", parameters?.Showroom);
            queryParameters.Add("@WeekCloseOrOffDayInMarket", parameters?.WeekCloseOrOffDayInMarket);
            queryParameters.Add("@SpaceProvidedToCase", parameters?.SpaceProvidedToCase);
            queryParameters.Add("@ImageUpload", parameters?.ImageUpload);
            queryParameters.Add("@Dealershowroom", parameters?.Dealershowroom);
            queryParameters.Add("@Rating", parameters?.Rating);
            queryParameters.Add("@StatusId", parameters?.StatusId);
            queryParameters.Add("@IsActive", parameters?.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveDealerDetails", queryParameters);
        }
        public async Task<IEnumerable<DealerDetailsResponse>> GetDealerList(DealerSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            //queryParameters.Add("@IsExport", parameters.IsExport);

            var result = await ListByStoredProcedure<DealerDetailsResponse>("GetDealerList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<DealerDetailsResponse?> GetDealerDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<DealerDetailsResponse>("GetDealerDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<int> UpdateDealerStatus(DealerStatusUpdate parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("UpdateDealerStatus", queryParameters);
        }
        #endregion
        #region DealerAddress
        public async Task<int> SaveDealeAddress(DealerAddressSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@DealerId", parameters.DealerId);
            queryParameters.Add("@CompanyDealingaddress", parameters?.CompanyDealingaddress.SanitizeValue());
            queryParameters.Add("@State", parameters?.State);
            queryParameters.Add("@Region", parameters?.Region);
            queryParameters.Add("@District", parameters?.District);
            queryParameters.Add("@City", parameters?.City);
            queryParameters.Add("@Area", parameters?.Area);
            queryParameters.Add("@Pincode", parameters?.Pincode);
            queryParameters.Add("@IsDefault", parameters?.IsDefault);
            queryParameters.Add("@IsActive", parameters?.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveDealerAddressDetails", queryParameters);
        }
        public async Task<IEnumerable<DealerAddressDetailsResponse>> GetDealerAddressList(DealerAddressSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@DealerId", parameters.DealerId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<DealerAddressDetailsResponse>("GetDealerAddressList", queryParameters);
        }
        public async Task<DealerAddressDetailsResponse?> GetDealerAddressDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<DealerAddressDetailsResponse>("GetDealerAddressDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion
        #region DealerContactDetails
        public async Task<int> SaveDealerContactDetails(DealerContactDetailsSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@DealerId", parameters.DealerId);
            queryParameters.Add("@EmailId", parameters?.EmailId.SanitizeValue());
            queryParameters.Add("@FirstName", parameters?.FirstName.SanitizeValue());
            queryParameters.Add("@LastName", parameters?.LastName.SanitizeValue());
            queryParameters.Add("@ContactType", parameters?.ContactType);
            queryParameters.Add("@MobileNumber", parameters?.MobileNumber);
            queryParameters.Add("@EmergencyContactNumber", parameters?.EmergencyContactNumber);
            queryParameters.Add("@DateofBirth", parameters?.DateofBirth);
            queryParameters.Add("@BloodGroup", parameters?.BloodGroup);
            queryParameters.Add("@Gender", parameters?.Gender);
            queryParameters.Add("@Anniversarydate", parameters?.Anniversarydate);
            queryParameters.Add("@IsDefault", parameters?.IsDefault);
            queryParameters.Add("@IsActive", parameters?.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveDealerContactDetails", queryParameters);
        }
        public async Task<IEnumerable<DealerContactDetailsResponse>> GetDealerContactDetailsList(DealerContactDetailsSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters?.ValueForSearch.SanitizeValue());
            queryParameters.Add("@DealerId", parameters.DealerId);
            queryParameters.Add("@IsActive", parameters?.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<DealerContactDetailsResponse>("GetDealerContactDetailsList", queryParameters);
        }
        public async Task<DealerContactDetailsResponse?> GetDealerContactDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<DealerContactDetailsResponse>("GetDealerContactDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion

    }
}
