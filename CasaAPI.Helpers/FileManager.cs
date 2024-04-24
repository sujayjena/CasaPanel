using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace CasaAPI.Helpers
{
    public interface IFileManager
    {
        string UploadProfilePicture(IFormFile file);
        byte[]? GetProfilePicture(string imageFileName);
        byte[]? GetFormatFileFromPath(string fileName);
        string UploadProductDesignFile(IFormFile file);
        byte[]? GetProductDesignFiles(string fileName);
        string? GetProfilePictureFile(string fileName);


        string UploadEmpDocuments(IFormFile file);
        byte[]? GetEmpDocuments(string fileName);
        string? GetEmpDocumentsFile(string fileName);


        string UploadCustomerDocuments(IFormFile file);
        byte[]? GetCustomerDocuments(string fileName);
        string? GetCustomerDocumentsFile(string fileName);

        string UploadReferralDocuments(IFormFile file);
        string? GetReferralDocumentsFile(string fileName);

        string UploadVisitDocuments(IFormFile file);
        byte[]? GetVisitDocuments(string fileName);
        string? GetVisitDocumentsFile(string fileName);

        string UploadDesignFiles(IFormFile file);
        byte[]? GetDesignFiles(string fileName);
        string? GetDesignDcoumentFile(string fileName);

        string UploadCatalogDocuments(IFormFile file);
        byte[]? GetCatalogDocuments(string fileName);
        string? GetCatalogDocumentsFile(string fileName);

        string UploadCatalogRelatedDocuments(IFormFile file);
        byte[]? GetCatalogRelatedDocuments(string fileName);

        string UploadProjectDocuments(IFormFile file);
        byte[]? GetProjectDocuments(string fileName);
        string? GetProjectDocumentsFile(string fileName);

        string UploadPanelPlanningImage(IFormFile file);
        string? GetPanelPlanningFile(string fileName);
    }

    public class FileManager : IFileManager
    {
        private readonly IHostingEnvironment _environment;
        public FileManager(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        private string SaveFileToPath(string folderPath, IFormFile postedFile)
        {
            string fileName = $"{Guid.NewGuid()}{new FileInfo(postedFile.FileName).Extension}";
            string fileSaveLocation = $"{folderPath}{fileName}";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (Stream fileStream = new FileStream(fileSaveLocation, FileMode.Create))
            {
                postedFile.CopyTo(fileStream);
            }

            return fileName;
        }

        public byte[]? GetFormatFileFromPath(string fileNameWithExtention)
        {
            byte[]? result = null;
            string imageWithFullPath = $"{_environment.ContentRootPath}\\FormatFiles\\{fileNameWithExtention}";

            if (File.Exists(imageWithFullPath))
            {
                result = File.ReadAllBytes(imageWithFullPath);
            }
            return result;
        }
        public string UploadProfilePicture(IFormFile file)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\ProfilePicture\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public string UploadEmpDocuments(IFormFile file)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\Documents\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public string? GetEmpDocumentsFile(string fileName)
        {
            string fileWithFullPath = "\\Uploads\\Documents\\" + fileName;
            return fileWithFullPath;
        }

        public byte[]? GetEmpDocuments(string fileName)
        {
            byte[]? result = null;
            string fileWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\Documents\\{fileName}";

            if (File.Exists(fileWithFullPath))
            {
                result = File.ReadAllBytes(fileWithFullPath);
            }

            return result;
        }

        public byte[]? GetProfilePicture(string imageFileName)
        {
            byte[]? result = null;
            string imageWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\ProfilePicture\\{imageFileName}";

            if (File.Exists(imageWithFullPath))
            {
                result = File.ReadAllBytes(imageWithFullPath);
            }
            else
            {
                string file= $"{_environment.ContentRootPath}\\Uploads\\Placeholder.png";
                result = File.ReadAllBytes(file);
            }

            return result;
        }

        public string UploadProductDesignFile(IFormFile file)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\ProductDesignFiles\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public byte[]? GetProductDesignFiles(string fileName)
        {
            byte[]? result = null;
            string fileWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\ProductDesignFiles\\{fileName}";

            if (File.Exists(fileWithFullPath))
            {
                result = File.ReadAllBytes(fileWithFullPath);
            }
            else
            {
                string file = $"{_environment.ContentRootPath}\\Uploads\\Placeholder.png";
                result = File.ReadAllBytes(file);
            }

            return result;
        }

        public string? GetProfilePictureFile(string fileName)
        {
            string fileWithFullPath = "\\Uploads\\ProfilePicture\\" + fileName;
            return fileWithFullPath;
        }


        public string UploadCustomerDocuments(IFormFile file)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\Customers\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public byte[]? GetCustomerDocuments(string fileName)
        {
            byte[]? result = null;
            string fileWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\Customers\\{fileName}";

            if (File.Exists(fileWithFullPath))
            {
                result = File.ReadAllBytes(fileWithFullPath);
            }

            return result;
        }

        public string? GetCustomerDocumentsFile(string fileName)
        {
            string fileWithFullPath = "\\Uploads\\Customers\\" + fileName;
            return fileWithFullPath;
        }

        public string UploadReferralDocuments(IFormFile file)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\Referral\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public string? GetReferralDocumentsFile(string fileName)
        {
            string fileWithFullPath = "\\Uploads\\Referral\\" + fileName;
            return fileWithFullPath;
        }

        public string UploadVisitDocuments(IFormFile file)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\VisitPhotos\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public byte[]? GetVisitDocuments(string fileName)
        {
            byte[]? result = null;
            string fileWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\VisitPhotos\\{fileName}";

            if (File.Exists(fileWithFullPath))
            {
                result = File.ReadAllBytes(fileWithFullPath);
            }

            return result;
        }
        public string? GetVisitDocumentsFile(string fileName)
        {
            string fileWithFullPath = "\\Uploads\\VisitPhotos\\" + fileName;
            return fileWithFullPath;
        }

        public string UploadDesignFiles(IFormFile file)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\DesignFiles\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public byte[]? GetDesignFiles(string fileName)
        {
            byte[]? result = null;
            string fileWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\DesignFiles\\{fileName}";

            if (File.Exists(fileWithFullPath))
            {
                result = File.ReadAllBytes(fileWithFullPath);
            }

            return result;
        }

        public string? GetDesignDcoumentFile(string fileName)
        {
            string fileWithFullPath = "\\Uploads\\DesignFiles\\" + fileName;
            return fileWithFullPath;
        }

        public string UploadCatalogDocuments(IFormFile file)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\Catalog\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public byte[]? GetCatalogDocuments(string fileName)
        {
            byte[]? result = null;
            string fileWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\Catalog\\{fileName}";

            if (File.Exists(fileWithFullPath))
            {
                result = File.ReadAllBytes(fileWithFullPath);
            }

            return result;
        }

        public string? GetCatalogDocumentsFile(string fileName)
        {
            string fileWithFullPath = "\\Uploads\\Catalog\\" + fileName;
            return fileWithFullPath;
        }

        public string UploadCatalogRelatedDocuments(IFormFile file)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\CatalogRelated\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public byte[]? GetCatalogRelatedDocuments(string fileName)
        {
            byte[]? result = null;
            string fileWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\CatalogRelated\\{fileName}";

            if (File.Exists(fileWithFullPath))
            {
                result = File.ReadAllBytes(fileWithFullPath);
            }

            return result;
        }

        public string UploadProjectDocuments(IFormFile file)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\Project\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public byte[]? GetProjectDocuments(string fileName)
        {
            byte[]? result = null;
            string fileWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\Project\\{fileName}";

            if (File.Exists(fileWithFullPath))
            {
                result = File.ReadAllBytes(fileWithFullPath);
            }

            return result;
        }
        public string? GetProjectDocumentsFile(string fileName)
        {
            string fileWithFullPath = "\\Uploads\\Project\\" + fileName;
            return fileWithFullPath;
        }

        public string UploadPanelPlanningImage(IFormFile file)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\PanelPlanning\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }
        public string? GetPanelPlanningFile(string fileName)
        {
            string fileWithFullPath = "\\Uploads\\PanelPlanning\\" + fileName;
            return fileWithFullPath;
        }

        /*
        public string UploadCompanyLogo(IFormFile file, HttpContext context)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\CompanyLogo\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public byte[]? GetCompanyLogo(string imageFileName, HttpContext context)
        {
            byte[]? result = null;
            string imageWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\CompanyLogo\\{imageFileName}";

            if (File.Exists(imageWithFullPath))
            {
                result = File.ReadAllBytes(imageWithFullPath);
            }

            return result;
        }

        public string UploadEmpDocuments(IFormFile file, HttpContext context)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\Documents\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public string UploadCustomerProfilePicture(IFormFile file, HttpContext context)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\CustomerPicture\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public byte[]? GetCustomerProfilePicture(string imageFileName, HttpContext context)
        {
            byte[]? result = null;
            string imageWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\CustomerPicture\\{imageFileName}";

            if (File.Exists(imageWithFullPath))
            {
                result = File.ReadAllBytes(imageWithFullPath);
            }

            return result;
        }

        public void DeleteWOEnqIssueSnaps(int woEnquiryId, HttpContext context)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\WOEnquiries\\IssueSnaps\\{woEnquiryId}\\";
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
        }

        public void DeleteWOProductProofSnaps(int woEnquiryId, HttpContext context)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\WOEnquiries\\ProductProofs\\{woEnquiryId}\\";
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
        }

        public string UploadWOEnqIssueSnaps(int woEnquiryId, IFormFile file, HttpContext context)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\WOEnquiries\\IssueSnaps\\{woEnquiryId}\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public byte[]? GetWOEnqIssueSnaps(int WOEnquiryId, string imageFileName, HttpContext context)
        {
            byte[]? result = null;
            string imageWithFullPath = $"{_environment.ContentRootPath}\\Uploads\\WOEnquiries\\IssueSnaps\\{WOEnquiryId}\\{imageFileName}";

            if (File.Exists(imageWithFullPath))
            {
                result = File.ReadAllBytes(imageWithFullPath);
            }

            return result;
        }

        public string UploadWOProductProofSnaps(int woEnquiryId, IFormFile file, HttpContext context)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\WOEnquiries\\ProductProofs\\{woEnquiryId}\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public string ExtendedWarrantyProofSnaps(int extendedWarrantryId, IFormFile file, HttpContext context)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\ExtendedWarranty\\ProductProofs\\{extendedWarrantryId}\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public string UploadSellDetailsProdProofDocs(long productDetailsId, IFormFile file, HttpContext context)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\SellDetails\\ProductProofs\\{productDetailsId}\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public string UploadSellDetailsProdSnaps(long productDetailsId, IFormFile file, HttpContext context)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\SellDetails\\ProductSnaps\\{productDetailsId}\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }

        public string UploadCustomerEnquiryDocs(long contactUsId, IFormFile file, HttpContext context)
        {
            string folderPath = $"{_environment.ContentRootPath}\\Uploads\\ContactUsEnquiriesDocs\\{contactUsId}\\";
            string fileName = SaveFileToPath(folderPath, file);
            return fileName;
        }
        */
    }
}
