
public class Slot
{
    public int Id { get; set; }
    public DateTime Start { get; set; }          
    public TimeSpan Duration { get; set; }
    public Patient? BookedPatient;
    public DateTime End => Start + Duration;
}

