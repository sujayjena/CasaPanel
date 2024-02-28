using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models.Enums;
using CasaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CasaAPI.Models.Constants;
using CasaAPI.Services;

namespace CasaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private ResponseModel _response;
        private IPortfolioService _portfolioService;
        private IFileManager _fileManager;
        public PortfolioController(IPortfolioService portfolioService, IFileManager fileManager)
        {
            _portfolioService = portfolioService;
            _fileManager = fileManager;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }
        #region Portfolio
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePortfolio([FromForm] PortfolioSaveParameters Request)
        {
            if (Request?.ImageUploadfile?.Length > 0)
            {
                Request.ImageUpload = _fileManager.UploadProfilePicture(Request.ImageUploadfile);
            }
            int result = await _portfolioService.SavePortfolio(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Portfolio is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Portfolio details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPortfolioList(PortfolioSearchParameters request)
        {
            IEnumerable<PortfolioDetailsResponse> lstDealer = await _portfolioService.GetPortfolioList(request);
            _response.Data = lstDealer.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetPortfolioDetails(long id)
        {
            PortfolioDetailsResponse? panelDisplay;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                panelDisplay = await _portfolioService.GetPortfolioDetailsById(id);
                _response.Data = panelDisplay;
            }

            return _response;
        }
        #endregion
    }
}
