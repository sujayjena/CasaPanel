using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Models;
using CasaAPI.Models.Enums;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Repositories
{
    public class VendorsRepository : BaseRepository, IVendorsRepository
    {
        private IConfiguration _configuration;
        public VendorsRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
        #region VenderManage
        public async Task<int> SaveVender(VenderSaveParameters parameters)
        {
            var status = 0;
            if (VenderStatusMaster.Rejected.ToString() == parameters.Status)
            {
                status = 2;
            }
            else if (VenderStatusMaster.Approved.ToString() == parameters.Status)
            {
                status = 1;
            }
            else if (VenderStatusMaster.Pending.ToString() == parameters.Status)
            {
                status = 0;
            }
            int loggedUserId = Convert.ToInt32(EncryptDecryptHelper.DecryptString(parameters.UserId));

            //var handler = new JwtSecurityTokenHandler();
            //var jsonToken = handler.ReadToken();
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@Date", parameters?.Date);
            queryParameters.Add("@VendorType", parameters?.VendorType);
            queryParameters.Add("@SubVendorType", parameters?.SubVendorType);
            queryParameters.Add("@VendorName", parameters?.VendorName.SanitizeValue());
            queryParameters.Add("@TelephpneNo", parameters?.TelephpneNo);
            queryParameters.Add("@FaxNo", parameters?.FaxNo);
            queryParameters.Add("@MobileNumber", parameters?.MobileNumber);
            queryParameters.Add("@Email", parameters?.Email);
            queryParameters.Add("@CompanyType", parameters?.CompanyType);
            queryParameters.Add("@Website", parameters?.Website);
            queryParameters.Add("@ReferenceBy", parameters?.ReferenceBy);
            queryParameters.Add("@PanNumber", parameters?.PanNumber);
            queryParameters.Add("@UploadPanCard", parameters?.UploadPanCard);
            queryParameters.Add("@GSTNumber", parameters?.GSTNumber);
            queryParameters.Add("@UploadGSTNumber", parameters?.UploadGSTNumber);
            queryParameters.Add("@ImportExportCertificate", parameters?.ImportExportCertificate);
            queryParameters.Add("@IECCode", parameters?.IECCode);
            queryParameters.Add("@UploadICECertificate", parameters?.UploadICECertificate);
            queryParameters.Add("@UploadNameChangeCertificate", parameters?.UploadNameChangeCertificate);
            queryParameters.Add("@IsTheNameChangeDone", parameters?.IsTheNameChangeDone);
            queryParameters.Add("@FirstName", parameters?.FirstName);
            queryParameters.Add("@LastName", parameters?.LastName);
            queryParameters.Add("@ContactType", parameters?.ContactType);
            queryParameters.Add("@AlternativeNumber", parameters?.AlternativeNumber);
            queryParameters.Add("@ContactTelephoneNo", parameters?.ContactTelephoneNo);
            queryParameters.Add("@ContactFaxNo", parameters?.ContactFaxNo);
            queryParameters.Add("@ContactMobileNumber", parameters?.ContactMobileNumber);
            queryParameters.Add("@ContactEmailId", parameters?.ContactEmailId);
            queryParameters.Add("@BankAcNo", parameters?.BankAcNo);
            queryParameters.Add("@NameOfBank", parameters?.NameOfBank);
            queryParameters.Add("@BankBranchName", parameters?.BankBranchName);
            queryParameters.Add("@IFSCCode", parameters?.IFSCCode);
            queryParameters.Add("@MICRCode", parameters?.MICRCode);
            queryParameters.Add("@CancelledChequeNumber", parameters?.CancelledChequeNumber);
            queryParameters.Add("@UploadCanceledcheque", parameters?.UploadCanceledcheque);
            queryParameters.Add("@vendorPostingGroup", parameters?.vendorPostingGroup);
            queryParameters.Add("@WorkingHours", parameters?.WorkingHours);
            queryParameters.Add("@PaymentTerme", parameters?.PaymentTerme);
            queryParameters.Add("@ModeOfTransportation", parameters?.ModeOfTransportation);
            queryParameters.Add("@DistancefromOurFactoryKN", parameters?.DistancefromOurFactoryKN);
            queryParameters.Add("@Status", status);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", loggedUserId);//SessionManager.LoggedInUserId
            return await SaveByStoredProcedure<int>("SaveManageVenderDetails", queryParameters);
        }
        public async Task<IEnumerable<VenderDetailsResponse>> GetVenderList(VenderSearchParameters parameters)
        {
            
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@Status", parameters.Status);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<VenderDetailsResponse>("GetManageVenderList", queryParameters);
        }
        public async Task<VenderDetailsResponse?> GetVenderDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<VenderDetailsResponse>("GetManageVenderDetailsById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> UpdateVenderStatus(VenderUpdateStatus parameters)
        {
            var status = 0;
            if (VenderStatusMaster.Rejected.ToString() == parameters.Status)
            {
                status = 2;
            }
            else if (VenderStatusMaster.Approved.ToString() == parameters.Status)
            {
                status = 1;
            }
            else if (VenderStatusMaster.Pending.ToString() == parameters.Status)
            {
                status = 0;
            }
            int loggedUserId =Convert.ToInt32(EncryptDecryptHelper.DecryptString(parameters.UserId));
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);          
            queryParameters.Add("@Status", status);
            queryParameters.Add("@LoggedInUserId", loggedUserId);
            return await SaveByStoredProcedure<int>("UpdateManageVenderStatus", queryParameters);
        }
        #endregion
    }
}
