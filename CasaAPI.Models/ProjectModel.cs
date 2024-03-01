using CasaAPI.Models;
using CasaAPI.Models.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class ProjectRequest
    {
        public long ProjectId { get; set; }

        [Required(ErrorMessage = ValidationConstants.ProjectNameRequied_Msg)]
        public string ProjectName { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = ValidationConstants.LaunchDateRequied_Msg)]
        public DateTime CompletionDate { get; set; }

        public string ProjectFileName { get; set; }
        public string ProjectSavedFileName { get; set; }
        public IFormFile ProjectFile { get; set; }

        public bool IsActive { get; set; }
    }
    public class SearchProjectRequest
    {
        public PaginationParameters pagination { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
    public class ProjectResponse : CreationDetails
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime CompletionDate { get; set; }
        public string ProjectFileName { get; set; }
        public string ProjectSavedFileName { get; set; }
        public byte[] ProjectFile { get; set; }
        public string ProjectFileUrl { get; set; }
        public bool IsActive { get; set; }
    }
    public class ProjectDetailsResponse
    {
        public ProjectResponse ProjectDetails { get; set; }
    }
}
