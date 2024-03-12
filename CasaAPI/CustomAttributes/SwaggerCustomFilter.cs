using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CasaAPI.CustomAttributes
{
    /// <summary>
    /// To configure request parameters for Swagger UI
    /// </summary>
    public class SwaggerCustomFilter : IOperationFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string relativePath = context.ApiDescription.RelativePath ?? "";
            //bool isTokenRequired = true;

            //if (string.Equals(relativePath, "api/Login/LoginByEmail", StringComparison.OrdinalIgnoreCase))
            //{
            //    isTokenRequired = false;
            //}

            //operation.Parameters.Add(new OpenApiParameter
            //{
            //    Name = "Authorization",
            //    In = ParameterLocation.Header,
            //    Description = "Session Token",
            //    Required = isTokenRequired
            //});

            if (string.Equals(relativePath, "api/Profile/SaveEmployeeDetails", StringComparison.OrdinalIgnoreCase))
            {
                operation.Parameters.Clear();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "parameter",
                    In = ParameterLocation.Query,
                    Description = "{\r\n  \"EmployeeId\": 0,\r\n  \"EmployeeName\": \"\",\r\n  \"EmployeeCode\": \"\",\r\n  \"EmailId\": \"\"," +
                    "\r\n  \"MobileNumber\": \"\",\r\n  \"RoleId\": 1,\r\n  \"ReportingTo\": 1,\r\n  \"DateOfBirth\": \"2000-01-10\"," +
                    "\r\n  \"DateOfJoining\": \"2023-08-10\",\r\n  \"EmergencyContactNumber\": \"\",\r\n  \"BloodGroupId\": null," +
                    "\r\n  \"IsWebUser\": true,\r\n  \"IsMobileUser\": true,\r\n  \"IsActive\": true,\r\n  \"Department\": \"\"," +
                    "\r\n  \"MaterialStatus\": 1,\r\n  \"Gender\": 1,\r\n  \"CompanyNumber\": \"\",\r\n  \"PermanentAddress\": \"\"," +
                    "\r\n  \"PermanentStateId\": 1,\r\n  \"PermanentRegionId\": 1,\r\n  \"PermanentDistrictId\": 1,\r\n  \"PermanentCityId\": 1," +
                    "\r\n  \"PermanentAreaId\": 1,\r\n  \"PermanentPinCode\": 1,\r\n  \"IsTemporaryAddressIsSame\": true," +
                    "\r\n  \"TemporaryAddress\": \"\",\r\n  \"TemporaryStateId\": 1,\r\n  \"TemporaryRegionId\": 1,\r\n  \"TemporaryDistrictId\": 1," +
                    "\r\n  \"TemporaryCityId\": 1,\r\n  \"TemporaryAreaId\": 1,\r\n  \"TemporaryPinCode\": 1,\r\n  \"EmergencyName\": \"\"," +
                    "\r\n  \"EmergencyNumber\": 1,\r\n  \"EmergencyRelation\": \"\",\r\n  \"EmployeePostCompanyName\": \"\",\r\n  \"TotalNumberOfExp\": \"\"," +
                    "\r\n  \"AddharNumber\": \"\",\r\n  \"PANNumber\": \"\",\r\n  \"OtherProof\": \"\",\r\n  \"Remark\": \"\"\r\n}",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "json"
                    }
                });

                //operation.Parameters.Add(new OpenApiParameter
                //{
                //    Name = "profilePicture",
                //    In = ParameterLocation.Query,
                //    Description = "Upload File",
                //    Required = false,
                //    Schema = new OpenApiSchema
                //    {
                //        Type = "file",
                //        Format = "binary"
                //    }
                //});
            }
        }
    }
}
