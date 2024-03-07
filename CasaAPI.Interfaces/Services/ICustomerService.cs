using CasaAPI.Models;

namespace CasaAPI.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerResponse>> GetCustomersList(SearchCustomerRequest request);
        Task<int> SaveCustomerDetails(CustomerRequest customerRequest);
        Task<CustomerDetailsResponse?> GetCustomerDetailsById(long id);
        Task<IEnumerable<CustomerDataValidationErrors>> ImportCustomersDetails(List<ImportedCustomerDetails> request);
        Task<int> SaveContactDetails(ContactSaveRequestParameters parameter);
    }
}
