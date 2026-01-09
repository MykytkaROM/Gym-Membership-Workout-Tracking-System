namespace Gym_Membership___Workout_Tracking_System.DTO;

public class TrainingSessionDTO
{
    public string SessionType { get; set; }

    public string Name { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public decimal? Price { get; set; }
    public int? GroupSize { get; set; }

    public decimal? PricePerHour { get; set; }

    public OnsiteSessionDTO Onsite { get; set; }
    public OnlineSessionDTO Online { get; set; }
}