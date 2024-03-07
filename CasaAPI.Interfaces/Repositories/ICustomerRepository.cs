using CasaAPI.Models;

namespace CasaAPI.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerResponse>> GetCustomersList(SearchCustomerRequest request);
        Task<int> SaveCustomerDetails(CustomerRequest parameters);
        Task<IEnumerable<ContactDetail>> GetCustomerContactDetailsById(long CustomerId);
        Task<IEnumerable<AddressDetail>> GetCustomerAddressDetailsById(long CustomerId);
        Task<CustomerResponse?> GetCustomerDetailsById(long id);
        Task<IEnumerable<CustomerDataValidationErrors>> ImportCustomersDetails(List<ImportedCustomerDetails> parameters);
        Task<int> SaveContactDetails(ContactSaveRequestParameters parameter);
    }
}
