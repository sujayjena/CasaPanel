using CasaAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace CasaAPI.Models
{
    public class CategoryModel
    {
        public class CategorySaveParameters
        {
            public int CategoryId { get; set; }
            //[Required(ErrorMessage = ValidationConstants.CategoryNameRequied_Msg)]
            //[RegularExpression(ValidationConstants.CategoryNameRegExp, ErrorMessage = ValidationConstants.CategoryNameRegExp_Msg)]
            //[MaxLength(ValidationConstants.CategoryName_MaxLength, ErrorMessage = ValidationConstants.CategoryName_MaxLength_Msg)]
            public string CategoryName { get; set; }
            public bool IsActive { get; set; }
        }
        public class CategoryDetailsResponse : LogParameters
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }

        }
        public class CategorySearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; }
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }
        }
        public class CategoryImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.CategoryNameRequied_Msg)]
            [RegularExpression(ValidationConstants.CategoryNameRegExp, ErrorMessage = ValidationConstants.CategoryNameRegExp_Msg)]
            [MaxLength(ValidationConstants.CategoryName_MaxLength, ErrorMessage = ValidationConstants.CategoryName_MaxLength_Msg)]
            public string CategoryName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class CategoryFailToImportValidationErrors
        {
            public string CategoryName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
