using CasaAPI.Models;

namespace CasaAPI.Interfaces.Repositories
{
    public interface IVisitRepository
    {
        Task<IEnumerable<VisitsResponse>> GetVisitsList(SearchVisitRequest request);
        Task<int> SaveVisitDetails(VisitsRequest parameters);
        Task<VisitDetailsResponse?> GetVisitDetailsById(long visitId);
        Task<IEnumerable<VisitRemarks>> GetVisitRemarks(long visitId);
        Task<IEnumerable<VisitDataValidationErrors>> ImportVisitsDetails(List<ImportedVisitDetails> parameters);
        Task<IEnumerable<VisitPhotosResponse>> GetVisitPhotos(long visitId);
    }
}
