using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class ContactTypeModel
    {
        public class ContactTypeSaveParameters
        {
            public int ContactTypeId { get; set; }
            [Required(ErrorMessage = ValidationConstants.ContactTypeRequied_Msg)]
            [RegularExpression(ValidationConstants.ContactTypeRegExp, ErrorMessage = ValidationConstants.ContactTypeRegExp_Msg)]
            [MaxLength(ValidationConstants.ContactType_MaxLength, ErrorMessage = ValidationConstants.ContactType_MaxLength_Msg)]
            public string ContactType { get; set; }
            public bool IsActive { get; set; }
        }
        public class ContactTypeDetailsResponse : LogParameters
        {
            public int ContactTypeId { get; set; }
            public string ContactType { get; set; }

        }
        public class ContactTypeSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class ContactTypeImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.ContactTypeRequied_Msg)]
            [RegularExpression(ValidationConstants.ContactTypeRegExp, ErrorMessage = ValidationConstants.ContactTypeRegExp_Msg)]
            [MaxLength(ValidationConstants.ContactType_MaxLength, ErrorMessage = ValidationConstants.ContactType_MaxLength_Msg)]
            public string ContactType { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class ContactTypeFailToImportValidationErrors
        {
            public string ContactType { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
