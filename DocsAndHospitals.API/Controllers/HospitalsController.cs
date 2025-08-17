using Microsoft.AspNetCore.Mvc;
using DocsAndHospitals.Application;
using DocsAndHospitals.Application.DTOs;
using System.Threading.Tasks;
using DocsAndHospitals.Models;
using DocsAndHospitals.Services;

namespace DocsAndHospitals.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HospitalsController : ControllerBase
    {
        private readonly IHospitalService _hospitalService;

        public HospitalsController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var hospitals = _hospitalService.GetAllHospitals();
            return Ok(hospitals);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var hospital = _hospitalService.GetHospitalById(id);
            if (hospital == null) return NotFound();
            return Ok(hospital);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Hospital hospital)
        {
            _hospitalService.AddHospital(hospital);
            await _hospitalService.SaveAsync();
            return CreatedAtAction(nameof(GetById), new { id = hospital.Id }, hospital);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Hospital updatedHospital)
        {
            var hospital = _hospitalService.GetHospitalById(id);
            if (hospital == null) return NotFound();

            _hospitalService.UpdateHospital(hospital, updatedHospital.Name, updatedHospital.Address, updatedHospital.PhoneNumber);
            await _hospitalService.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = _hospitalService.DeleteHospital(id);
            if (!deleted) return NotFound();

            await _hospitalService.SaveAsync();
            return NoContent();
        }
    }
}
