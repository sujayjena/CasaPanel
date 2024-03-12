using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class PanelTypeModel
    {
        public class PanelTypeSaveParameters
        {
            public int PanelTypeId { get; set; }
            [Required(ErrorMessage = ValidationConstants.PanelTypeNameRequied_Msg)]
            [RegularExpression(ValidationConstants.PanelTypeNameRegExp, ErrorMessage = ValidationConstants.PanelTypeNameRegExp_Msg)]
            [MaxLength(ValidationConstants.PanelTypeName_MaxLength, ErrorMessage = ValidationConstants.PanelTypeName_MaxLength_Msg)]
            public string PanelTypeName { get; set; }
            public bool IsActive { get; set; }
        }
        public class PanelTypeDetailsResponse : LogParameters
        {
            public int PanelTypeId { get; set; }
            public string PanelTypeName { get; set; }

        }
        public class PanelTypeSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; }
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class PanelTypeImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.TypeNameRequied_Msg)]
            [RegularExpression(ValidationConstants.TypeNameRegExp, ErrorMessage = ValidationConstants.TypeNameRegExp_Msg)]
            [MaxLength(ValidationConstants.TypeName_MaxLength, ErrorMessage = ValidationConstants.TypeName_MaxLength_Msg)]
            public string PanelTypeName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class PanelTypeFailToImportValidationErrors
        {
            public string PanelTypeName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
