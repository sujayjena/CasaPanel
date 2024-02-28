using CasaAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace CasaAPI.Models
{
    public class TypeModel
    {
        public class TypeSaveParameters
        {
            public int TypeId { get; set; }
            [Required(ErrorMessage = ValidationConstants.TypeNameRequied_Msg)]
            [RegularExpression(ValidationConstants.TypeNameRegExp, ErrorMessage = ValidationConstants.TypeNameRegExp_Msg)]
            [MaxLength(ValidationConstants.TypeName_MaxLength, ErrorMessage = ValidationConstants.TypeName_MaxLength_Msg)]
            public string TypeName { get; set; }
            public bool IsActive { get; set; }
        }
        public class TypeDetailsResponse : LogParameters
        {
            public int TypeId { get; set; }
            public string TypeName { get; set; }

        }
        public class TypeSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; }
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class TypeImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.TypeNameRequied_Msg)]
            [RegularExpression(ValidationConstants.TypeNameRegExp, ErrorMessage = ValidationConstants.TypeNameRegExp_Msg)]
            [MaxLength(ValidationConstants.TypeName_MaxLength, ErrorMessage = ValidationConstants.TypeName_MaxLength_Msg)]
            public string TypeName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class TypeFailToImportValidationErrors
        {
            public string TypeName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
