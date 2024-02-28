using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class TileTypeModel
    {       
            public class TileTypeSaveParameters
            {
                public int TileTypeId { get; set; }
                [Required(ErrorMessage = ValidationConstants.TileTypeRequied_Msg)]
                [RegularExpression(ValidationConstants.TileTypeRegExp, ErrorMessage = ValidationConstants.TileTypeRegExp_Msg)]
                [MaxLength(ValidationConstants.TileType_MaxLength, ErrorMessage = ValidationConstants.TileType_MaxLength_Msg)]
                public string TileType { get; set; }
                public bool IsActive { get; set; }
            }
            public class TileTypeDetailsResponse : LogParameters
            {
                public int TileTypeId { get; set; }
                public string TileType { get; set; }

            }
            public class TileTypeSearchParameters
            {
                public PaginationParameters pagination { get; set; }
                public string ValueForSearch { get; set; } = null;
                public bool? IsActive { get; set; }
                public bool? IsExport { get; set; }

            }
            public class TileTypeImportSaveParameters
            {
                [Required(ErrorMessage = ValidationConstants.TileTypeRequied_Msg)]
                [RegularExpression(ValidationConstants.TileTypeRegExp, ErrorMessage = ValidationConstants.TileTypeRegExp_Msg)]
                [MaxLength(ValidationConstants.TileType_MaxLength, ErrorMessage = ValidationConstants.TileType_MaxLength_Msg)]
                public string TileType { get; set; }
                [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
                [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
                public string IsActive { get; set; }
            }
            public class TileTypeFailToImportValidationErrors
        {
                public string TileType { get; set; }
                public string IsActive { get; set; }
                public string ValidationMessage { get; set; }
            }        
    }
}

