using CasaAPI.Models;

namespace CasaAPI.Interfaces.Services
{
    public interface IProfileService
    {
        Task<UsersLoginSessionData?> ValidateUserLoginByUsername(LoginByEmailRequestModel parameters);
        Task SaveUserLoginHistory(UserLoginHistorySaveParameters parameters);
        Task<UsersLoginSessionData?> GetProfileDetailsByToken(string token);
        //Task<UsersLoginSessionData?> GetProfileDetailsByToken(string token);

        //Task<IEnumerable<RoleResponse>> GetRolesList(SearchRoleRequest request);
        //Task<int> SaveRoleDetails(RoleRequest roleRequest);
        //Task<RoleResponse?> GetRoleDetailsById(long id);
        //Task<IEnumerable<RoleDataValidationErrors>> ImportRolesDetails(List<ImportedRoleDetails> request);

        //Task<IEnumerable<ReportingToResponse>> GetReportingTosList(SearchReportingToRequest request);
        //Task<int> SaveReportingToDetails(ReportingToRequest reportingToRequest);
        //Task<ReportingToResponse?> GetReportingToDetailsById(long id);
        //Task<IEnumerable<ReportingToDataValidationErrors>> ImportReportingTosDetails(List<ImportedReportingToDetails> request);
        //Task<IEnumerable<EmployeeResponse>> GetEmployeesList(SearchEmployeeRequest request);
        //Task<int> SaveEmployeeDetails(EmployeeRequest employeeRequest);
        //Task<int> UpdateEmpDetailsThroughApp(UpdateEmployeeDetailsRequest parameters);

        //Task<EmployeeResponse?> GetEmployeeDetailsById(long id);
        //Task<IEnumerable<EmployeeDataValidationErrors>> ImportEmployeesDetails(List<ImportedEmployeeDetails> request);


        //Task<PunchInOutHistoryModel?> SubmitPunchInOut(long userId);
    }
}
