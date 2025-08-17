using DocsAndHospitals.Models;
using System.Threading.Tasks;

namespace DocsAndHospitals.Services
{
    public interface IHospitalService
    {
        Task InitializeAsync();
        Hospital[] GetAllHospitals();
        void AddHospital(Hospital hospital);
        Hospital? GetHospitalById(int id);
        bool DeleteHospital(int id);
        void UpdateHospital(Hospital hospital, string? name = null, string? address = null, string? phone = null);
        Task SaveAsync();
    }
}
