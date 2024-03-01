﻿using CasaAPI.Models;
using Models;

namespace CasaAPI.Interfaces.Repositories
{
    public interface IProfileRepository
    {
        Task<IEnumerable<RoleResponse>> GetRolesList(SearchRoleRequest request);
        Task<int> SaveRoleDetails(RoleRequest parameters);
        Task<RoleResponse?> GetRoleDetailsById(long id);
        Task<IEnumerable<RoleDataValidationErrors>> ImportRolesDetails(List<ImportedRoleDetails> parameters);

        Task<IEnumerable<RoleResponse>> GetReportingHierarchyRolesList(SearchRoleRequest request);
        Task<IEnumerable<ReportingToResponse>> GetReportingTosList(SearchReportingToRequest request);
        Task<int> SaveReportingToDetails(ReportingToRequest parameters);
        Task<ReportingToResponse?> GetReportingToDetailsById(long id);
        Task<IEnumerable<ReportingToDataValidationErrors>> ImportReportingTosDetails(List<ImportedReportingToDetails> parameters);

        Task<IEnumerable<EmployeeResponse>> GetEmployeesList(SearchEmployeeRequest request);
        Task<IEnumerable<EmployeeReportingToResponse>> GetEmployeesListByReportingTo(long employeeId);
        Task<int> SaveEmployeeDetails(EmployeeRequest parameters);
        Task<int> UpdateEmpDetailsThroughApp(UpdateEmployeeDetailsRequest parameters);

        Task<EmployeeResponse?> GetEmployeeDetailsById(long id);

        Task<IEnumerable<EmployeeDataValidationErrors>> ImportEmployeesDetails(List<ImportedEmployeeDetails> parameters);

        Task<UsersLoginSessionData?> ValidateUserLoginByEmail(LoginByMobileNoRequestModel parameters);
        Task SaveUserLoginHistory(UserLoginHistorySaveParameters parameters);

        Task<UsersLoginSessionData?> GetProfileDetailsByToken(string token);
        Task<PunchInOutHistoryModel?> SubmitPunchInOut(PunchInOutRequestModel parameters);
        Task<IEnumerable<PunchInOutHistoryModel>> GetPunchHistoryList(PunchHistoryRequestModel parameters);


        Task<IEnumerable<ModuleMaster_Response>> GetModuleMasterList(SearchModuleMasterRequest parameters);

        Task<IEnumerable<RoleMaster_Permission_Response>> GetRoleMaster_PermissionList(SearchRoleMaster_PermissionRequest request);
        Task<int> SaveRoleMaster_PermissionDetails(RoleMaster_Permission_Request parameters);

        Task<IEnumerable<RoleMaster_Employee_Permission_Response>> GetRoleMaster_Employee_PermissionList(SearchRoleMaster_Employee_PermissionRequest request);
        Task<IEnumerable<RoleMasterEmployeePermissionList>> GetRoleMaster_Employee_PermissionById(long employeeId);
        Task<int> SaveRoleMaster_Employee_PermissionDetails(RoleMaster_Employee_Permission_Request parameters);
    }
}
