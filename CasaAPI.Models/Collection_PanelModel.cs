using CasaAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace CasaAPI.Models
{
    public class Collection_PanelModel
    {
        public class Collection_PanelSaveParameters
        {
            public int CollectionId { get; set; }
            //[Required(ErrorMessage = ValidationConstants.CollectionName_Required_Msg)]
            //[RegularExpression(ValidationConstants.CollectionName_RegExp, ErrorMessage = ValidationConstants.CollectionName_RegExp_Msg)]
            //[MaxLength(ValidationConstants.CollectionName_MaxLength, ErrorMessage = ValidationConstants.CollectionName_MaxLength_Msg)]
            public string CollectionName { get; set; }
            public bool IsActive { get; set; }
        }
        public class CollectionDetails_PanelResponse : LogParameters
        {
            public int CollectionId { get; set; }
            public string CollectionName { get; set; }

        }
        public class Collection_PanelSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }
             
        }
        public class Collection_PanelImportSaveParameters
        {
            //[Required(ErrorMessage = ValidationConstants.CollectionName_Required_Msg)]
            //[RegularExpression(ValidationConstants.CollectionName_RegExp, ErrorMessage = ValidationConstants.CollectionName_RegExp_Msg)]
            //[MaxLength(ValidationConstants.CollectionName_MaxLength, ErrorMessage = ValidationConstants.CollectionName_MaxLength_Msg)]
            public string CollectionName { get; set; }
            //[Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            //[RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class Collection_PanelFailToImportValidationErrors
        {
            public string CollectionName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
