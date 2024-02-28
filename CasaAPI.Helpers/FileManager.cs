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
