using CasaAPI.Models;
using CasaAPI.Interfaces.Services;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Helpers;

namespace CasaAPI.Services
{
    public class ProfileService : IProfileService
    {
        private IProfileRepository _profileRepository;
        //private IFileManager _fileManager;

        public ProfileService(IProfileRepository profileRepository) //, IFileManager fileManager)
        {
            _profileRepository = profileRepository;
            //_fileManager = fileManager;
        }

        public async Task<UsersLoginSessionData?> ValidateUserLoginByUsername(LoginByEmailRequestModel parameters)
        {
            parameters.Password = EncryptDecryptHelper.EncryptString(parameters.Password);
            return await _profileRepository.ValidateUserLoginByUsername(parameters);
        }

        public async Task SaveUserLoginHistory(UserLoginHistorySaveParameters parameters)
        {
            await _profileRepository.SaveUserLoginHistory(parameters);
        }

        public async Task<UsersLoginSessionData?> GetProfileDetailsByToken(string token)
        {
            return await _profileRepository.GetProfileDetailsByToken(token);
        }

        //public async Task<IEnumerable<RoleResponse>> GetRolesList(SearchRoleRequest request)
        //{
        //    return await _profileRepository.GetRolesList(request);
        //}

        //public async Task<int> SaveRoleDetails(RoleRequest roleRequest)
        //{
        //    return await _profileRepository.SaveRoleDetails(roleRequest);
        //}

        //public async Task<RoleResponse?> GetRoleDetailsById(long id)
        //{
        //    return await _profileRepository.GetRoleDetailsById(id);
        //}
        //public async Task<IEnumerable<RoleDataValidationErrors>> ImportRolesDetails(List<ImportedRoleDetails> request)
        //{
        //    return await _profileRepository.ImportRolesDetails(request);
        //}

        //public async Task<IEnumerable<ReportingToResponse>> GetReportingTosList(SearchReportingToRequest request)
        //{
        //    return await _profileRepository.GetReportingTosList(request);
        //}

        //public async Task<int> SaveReportingToDetails(ReportingToRequest reportingToRequest)
        //{
        //    return await _profileRepository.SaveReportingToDetails(reportingToRequest);
        //}
        //public async Task<ReportingToResponse?> GetReportingToDetailsById(long id)
        //{
        //    return await _profileRepository.GetReportingToDetailsById(id);
        //}
        //public async Task<IEnumerable<ReportingToDataValidationErrors>> ImportReportingTosDetails(List<ImportedReportingToDetails> request)
        //{
        //    return await _profileRepository.ImportReportingTosDetails(request);
        //}

        //public async Task<IEnumerable<EmployeeResponse>> GetEmployeesList(SearchEmployeeRequest request)
        //{
        //    return await _profileRepository.GetEmployeesList(request);
        //}

        //public async Task<int> SaveEmployeeDetails(EmployeeRequest employeeRequest)
        //{
        //    if (employeeRequest.ProfilePicture != null)
        //    {
        //        employeeRequest.FileOriginalName = employeeRequest.ProfilePicture.FileName;
        //        employeeRequest.ImageUpload = _fileManager.UploadProfilePicture(employeeRequest.ProfilePicture);
        //    }

        //    return await _profileRepository.SaveEmployeeDetails(employeeRequest);
        //}

        //public async Task<int> UpdateEmpDetailsThroughApp(UpdateEmployeeDetailsRequest parameters)
        //{
        //    return await _profileRepository.UpdateEmpDetailsThroughApp(parameters);
        //}

        //public async Task<EmployeeResponse?> GetEmployeeDetailsById(long id)
        //{
        //    EmployeeResponse? response = await _profileRepository.GetEmployeeDetailsById(id);

        //    if (response != null && !string.IsNullOrEmpty(response.ImageUpload))
        //    {
        //        response.ProfilePicture = _fileManager.GetProfilePicture(response.ImageUpload);
        //    }

        //    return response;
        //}

        //public async Task<IEnumerable<EmployeeDataValidationErrors>> ImportEmployeesDetails(List<ImportedEmployeeDetails> request)
        //{
        //    return await _profileRepository.ImportEmployeesDetails(request);
        //}

        //public async Task<PunchInOutHistoryModel?> SubmitPunchInOut(long userId)
        //{
        //    return await _profileRepository.SubmitPunchInOut(userId);
        //}
    }
}
