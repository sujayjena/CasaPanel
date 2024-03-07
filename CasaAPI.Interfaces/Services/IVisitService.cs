﻿using CasaAPI.Models;

namespace CasaAPI.Interfaces.Services
{
    public interface IVisitService
    {
        Task<IEnumerable<VisitsResponse>> GetVisitsList(SearchVisitRequest request);
        Task<int> SaveVisitDetails(VisitsRequest visitRequest);
        Task<VisitDetailsResponse?> GetVisitDetailsById(long visitId);
        Task<IEnumerable<VisitRemarks>> GetVisitRemarks(long visitId);
        Task<IEnumerable<VisitDataValidationErrors>> ImportVisitsDetails(List<ImportedVisitDetails> request);
        Task<IEnumerable<VisitPhotosResponse>> GetVisitPhotos(long visitId, string host);
    }
}
