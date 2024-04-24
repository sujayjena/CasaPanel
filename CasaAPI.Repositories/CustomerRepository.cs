using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Models;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace CasaAPI.Repositories
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        private IConfiguration _configuration;

        public CustomerRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<CustomerResponse>> GetCustomersList(SearchCustomerRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@CustomerTypeId", parameters.CustomerTypeId.SanitizeValue());
            queryParameters.Add("@EmployeeId", parameters.EmployeeId.SanitizeValue());
            //queryParameters.Add("@CompanyName", parameters.CompanyName.SanitizeValue());
            //queryParameters.Add("@EmployeeName", parameters.EmployeeName.SanitizeValue());
            queryParameters.Add("@SearchValue", parameters.SearchValue.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@FilterType", parameters.FilterType.SanitizeValue());
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<CustomerResponse>("GetCustomers", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<IEnumerable<ContactDetailResponse>> GetCustomerContactDetailsById(long CustomerId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerId", CustomerId);
            return await ListByStoredProcedure<ContactDetailResponse>("GetCustomerContactDetailsById", queryParameters);
        }
        

        public async Task<IEnumerable<AddressDetail>> GetCustomerAddressDetailsById(long CustomerId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerId", CustomerId);
            return await ListByStoredProcedure<AddressDetail>("GetCustomerAddressDetailsById", queryParameters);
        }

        public async Task<int> SaveCustomerDetails(CustomerRequest parameters)
        {
            string xmlContactData, xmlAddressData;
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerId", parameters.CustomerId);
            queryParameters.Add("@CompanyName", parameters.CompanyName.SanitizeValue());
            queryParameters.Add("@LandlineNo", parameters.LandlineNo.SanitizeValue());
            queryParameters.Add("@MobileNumber", parameters.MobileNumber.SanitizeValue());
            queryParameters.Add("@EmailId", parameters.EmailId.SanitizeValue());
            queryParameters.Add("@CustomerTypeId", parameters.CustomerTypeId);
            queryParameters.Add("@SpecialRemarks", parameters.SpecialRemarks.SanitizeValue());
            queryParameters.Add("@EmployeeRoleId", parameters.EmployeeRoleId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@GstFileName", parameters.GstFileName.SanitizeValue());
            queryParameters.Add("@GstSavedFileName", parameters.GstSavedFileName.SanitizeValue());
            queryParameters.Add("@PanCardFileName", parameters.PanCardFileName.SanitizeValue());
            queryParameters.Add("@PanCardSavedFileName",parameters.PanCardSavedFileName.SanitizeValue());

            xmlContactData = ConvertListToXml(parameters.ContactDetails);
            queryParameters.Add("@XmlContactData", xmlContactData);

            xmlAddressData = ConvertListToXml(parameters.AddressDetails);
            queryParameters.Add("@XmlAddressData", xmlAddressData);

            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            queryParameters.Add("@Password", EncryptDecryptHelper.EncryptString(EncryptDecryptHelper.CreateRandomPassword()));
            return await SaveByStoredProcedure<int>("SaveCustomerDetails", queryParameters);
        }

        public async Task<CustomerResponse?> GetCustomerDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<CustomerResponse>("GetCustomerDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<CustomerDataValidationErrors>> ImportCustomersDetails(List<ImportedCustomerDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlCustomerData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlCustomerData", xmlCustomerData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<CustomerDataValidationErrors>("SaveImportCustomerDetails", queryParameters);
        }

        public async Task<int> SaveContactDetails(ContactSaveRequestParameters parameter)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@ContactId", parameter.ContactId);
            queryParameters.Add("@CustomerId", parameter.CustomerId);
            queryParameters.Add("@ContactName", parameter.ContactName);
            queryParameters.Add("@MobileNo", parameter.MobileNo);
            queryParameters.Add("@EmailAddress", parameter.EmailAddress);
            queryParameters.Add("@IsActive",parameter.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveContactDetails", queryParameters);
        }
    }
}
