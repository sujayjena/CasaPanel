using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;

namespace CasaAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _customerRepository;
        private IFileManager _fileManager;

        public CustomerService(ICustomerRepository customerRepository, IFileManager fileManager)
        {
            _customerRepository = customerRepository;
            _fileManager = fileManager;
        }

        public async Task<IEnumerable<CustomerResponse>> GetCustomersList(SearchCustomerRequest request)
        {
            return await _customerRepository.GetCustomersList(request);
        }

        public async Task<int> SaveCustomerDetails(CustomerRequest customerRequest)
        {
            return await _customerRepository.SaveCustomerDetails(customerRequest);
        }

        public async Task<CustomerDetailsResponse?> GetCustomerDetailsById(long id)
        {
            CustomerDetailsResponse objCustomerDetailsResponse=new CustomerDetailsResponse();
            CustomerResponse? data = await _customerRepository.GetCustomerDetailsById(id);

            if (data != null)
            {
                objCustomerDetailsResponse.customerDetails = data;
                
                objCustomerDetailsResponse.customerDetails.GstFile = _fileManager.GetCustomerDocuments(objCustomerDetailsResponse.customerDetails.GstSavedFileName);
                objCustomerDetailsResponse.customerDetails.PanCard = _fileManager.GetCustomerDocuments(objCustomerDetailsResponse.customerDetails.PanCardSavedFileName);

                objCustomerDetailsResponse.contactDetails = (await _customerRepository.GetCustomerContactDetailsById(data.CustomerId)).ToList();

                foreach (ContactDetail contact in objCustomerDetailsResponse.contactDetails)
                {
                    contact.PanCardFile = _fileManager.GetCustomerDocuments(contact.PanCardSavedFileName);
                    contact.AdharCardFile = _fileManager.GetCustomerDocuments(contact.AdharCardSavedFileName);
                }

                objCustomerDetailsResponse.addressDetails = (await _customerRepository.GetCustomerAddressDetailsById(data.CustomerId)).ToList();
            }

            return objCustomerDetailsResponse;
        }

        public async Task<IEnumerable<CustomerDataValidationErrors>> ImportCustomersDetails(List<ImportedCustomerDetails> request)
        {
            return await _customerRepository.ImportCustomersDetails(request);
        }

        public async Task<int> SaveContactDetails(ContactSaveRequestParameters parameter)
        {
            return await _customerRepository.SaveContactDetails(parameter);
        }
    }
}
