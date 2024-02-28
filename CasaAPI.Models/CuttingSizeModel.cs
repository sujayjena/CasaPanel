using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class CuttingSizeModel
    {
        public class CuttingSizeSaveParameters
        {
            public int CuttingSizeId { get; set; }
            //[Required(ErrorMessage = ValidationConstants.CuttingSizeRequied_Msg)]
            //[RegularExpression(ValidationConstants.CuttingSizeRegExp, ErrorMessage = ValidationConstants.CuttingSizeRegExp_Msg)]
            //[MaxLength(ValidationConstants.CuttingSize_MaxLength, ErrorMessage = ValidationConstants.CuttingSize_MaxLength_Msg)]
            public string CuttingSize { get; set; }
            public bool IsActive { get; set; }
        }
        public class CuttingSizeDetailsResponse : LogParameters
        {
            public int CuttingSizeId { get; set; }
            public string CuttingSize { get; set; }

        }
        public class CuttingSizeSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class CuttingSizeImportSaveParameters
        {
            //[Required(ErrorMessage = ValidationConstants.CuttingSizeRequied_Msg)]
            //[RegularExpression(ValidationConstants.CuttingSizeRegExp, ErrorMessage = ValidationConstants.CuttingSizeRegExp_Msg)]
            //[MaxLength(ValidationConstants.CuttingSize_MaxLength, ErrorMessage = ValidationConstants.CuttingSize_MaxLength_Msg)]
            public string CuttingSize { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class CuttingSizeFailToImportValidationErrors
        {
            public string CuttingSize { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
