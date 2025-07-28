using DocsAndHospitals.Models;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

public class AppointmentManager
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task<bool> TryBookSlotAsync(Doctor doctor, Slot slot, Patient patient)
    {
        await _semaphore.WaitAsync();
        try
        {
            await Task.Delay(200); 

            if (slot.BookedPatient == null)
            {
                slot.BookedPatient = patient;
                Console.WriteLine($" {patient.Name} get slot {slot.Start.Day} {slot.Start.ToString("MMMM", new CultureInfo("en-US"))} {slot.Start.Year}, time: {slot.Start.TimeOfDay}");
                return true;
            }

            Console.WriteLine($" {patient.Name} mistake. slot unavailable");
            return false;
        }
        finally
        {
            _semaphore.Release(); 
        }
    }

    public bool TryBookSlotAtomic(Slot slot, Patient patient)
    {
        var original = Interlocked.CompareExchange(ref slot.BookedPatient, patient, null);
        if(original == null)
        {
            Console.WriteLine($" {patient.Name} get slot {slot.Start.Day} {slot.Start.ToString("MMMM", new CultureInfo("en-US"))} {slot.Start.Year}, time: {slot.Start.TimeOfDay}");
            return true;
        }
        else
        {
            Console.WriteLine($" {patient.Name} mistake. slot unavailable");
            return false;
        }
    }
}

