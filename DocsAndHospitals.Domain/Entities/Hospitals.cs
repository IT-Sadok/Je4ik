using System;
namespace DocsAndHospitals.Models;

public class Hospital
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public List<Doctor> Doctors { get; set; } = new();
}
