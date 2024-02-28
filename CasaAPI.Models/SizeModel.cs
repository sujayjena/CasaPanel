using CasaAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CasaAPI.Models
{
    public class SizeSaveParameters
    {
        public int SizeId { get; set; }
        [Required(ErrorMessage = ValidationConstants.SizeNameRequied_Msg)]
        [RegularExpression(ValidationConstants.SizeNameRegExp, ErrorMessage = ValidationConstants.SizeNameRegExp)]
        [MaxLength(ValidationConstants.SizeName_MaxLength, ErrorMessage = ValidationConstants.SizeName_MaxLength_Msg)]
        public string SizeName { get; set; }
        public bool IsActive { get; set; }
    }
    public class SizeDetailsResponse : LogParameters
    {
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        
    }
    public class SizeSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }
        
        [JsonIgnore]
        public bool? IsExport { get; set; }
    }

    public class SizeImportSaveParameters
    {
        [Required(ErrorMessage = ValidationConstants.SizeNameRequied_Msg)]
        [RegularExpression(ValidationConstants.SizeNameRegExp, ErrorMessage = ValidationConstants.SizeNameRegExp_Msg)]
        [MaxLength(ValidationConstants.SizeName_MaxLength, ErrorMessage = ValidationConstants.SizeName_MaxLength_Msg)]
        public string SizeName { get; set; }
        [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
        [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
        public string IsActive { get; set; }
    }
    public class SizeFailToImportValidationErrors
    {
        public string SizeName { get; set; }
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
}
