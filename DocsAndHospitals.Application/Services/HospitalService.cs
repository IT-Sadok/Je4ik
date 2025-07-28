using DocsAndHospitals.Models;
using DocsAndHospitals.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace DocsAndHospitals.Services
{
    public class HospitalService : IHospitalService
    {
        private List<Hospital> _hospitals = new List<Hospital>();
        private readonly IHospitalRepository _repository;

        public HospitalService(IHospitalRepository repository)
        {
            _repository = repository;
        }

        public async Task InitializeAsync()
        {
            _hospitals = await _repository.LoadAsync();
        }

        public Hospital[] GetAllHospitals() => _hospitals.ToArray();

        public void AddHospital(Hospital hospital)
        {
            _hospitals.Add(hospital);
        }

        public Hospital? GetHospitalById(int id)
        {
            return _hospitals.FirstOrDefault(h => h.Id == id);
        }

        public bool DeleteHospital(int id)
        {
            var hospital = GetHospitalById(id);
            if (hospital == null)
                return false;

            return _hospitals.Remove(hospital);
        }

        public void UpdateHospital(Hospital hospital, string? name = null, string? address = null, string? phone = null)
        {
            if (!string.IsNullOrWhiteSpace(name)) hospital.Name = name;
            if (!string.IsNullOrWhiteSpace(address)) hospital.Address = address;
            if (!string.IsNullOrWhiteSpace(phone)) hospital.PhoneNumber = phone;
        }

        public void AddDoctor(Hospital hospital, Doctor doctor)
        {
            hospital.Doctors.Add(doctor);
        }

        public Doctor? GetDoctor(Hospital hospital, int doctorId)
        {
            return hospital.Doctors.FirstOrDefault(d => d.Id == doctorId);
        }

        public void UpdateDoctor(Doctor doctor, string? name = null, string? specialization = null)
        {
            if (!string.IsNullOrWhiteSpace(name)) doctor.Name = name;
            if (!string.IsNullOrWhiteSpace(specialization)) doctor.Specialization = specialization;
        }

        public void DeleteDoctor(Hospital hospital, int doctorId)
        {
            hospital.Doctors.RemoveAll(d => d.Id == doctorId);
        }

        public async Task SaveAsync()
        {
            await _repository.SaveAsync(_hospitals);
        }
    }

}
