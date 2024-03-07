using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using Microsoft.AspNetCore.Http;

namespace CasaAPI.Services
{
    public class ManageDesignService : IManageDesignService
    {
        private IManageDesignRepository _manageDesignRepository;
        private IFileManager _fileManager;

        public ManageDesignService(IManageDesignRepository manageDesignRepository, IFileManager fileManager)
        {
            _manageDesignRepository = manageDesignRepository;
            _fileManager = fileManager;
        }

        public async Task<IEnumerable<DesignResponse>> GetDesignesList(SearchDesignRequest request)
        {
            return await _manageDesignRepository.GetDesignesList(request);
        }

        public async Task<int> SaveDesignDetails(DesignRequest designRequest)
        {
            List<DesignMasterImages> lstDesignImages = new List<DesignMasterImages>();

            if (designRequest.DesignImages != null)
            {
                foreach (IFormFile image in designRequest.DesignImages)
                {
                    lstDesignImages.Add(new DesignMasterImages()
                    {
                        UploadedFilesName = image.FileName,
                        SavedFilesName = _fileManager.UploadDesignFiles(image)
                    });
                }
            }

            return await _manageDesignRepository.SaveDesignDetails(designRequest, lstDesignImages);
        }

        public async Task<IEnumerable<DesignDataValidationErrors>> ImportDesignsDetails(List<ImportedDesignDetails> request)
        {
            return await _manageDesignRepository.ImportDesignsDetails(request);
        }

        public async Task<DesignResponse?> GetDesignDetailsById(long id)
        {
            DesignResponse? result;
            List<DesignMasterImages?> lstDesignImages;

            result = await _manageDesignRepository.GetDesignDetailsById(id);

            if (result != null)
            {
                lstDesignImages = (await _manageDesignRepository.GetDesignImagesList(id)).ToList();

                foreach (var designImage in lstDesignImages)
                {
                    designImage!.DesignFile = _fileManager.GetDesignFiles(designImage.SavedFilesName);
                }

                result.DesignImages = lstDesignImages;
            }

            return result;
        }
    }
}
