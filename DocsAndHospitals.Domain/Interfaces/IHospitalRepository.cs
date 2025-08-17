using DocsAndHospitals.Models;
using System.Collections.Generic;

namespace DocsAndHospitals.Persistence;

public interface IHospitalRepository
    {
        Task<List<Hospital>> LoadAsync();
        Task SaveAsync(IEnumerable<Hospital> hospitals);
    }
