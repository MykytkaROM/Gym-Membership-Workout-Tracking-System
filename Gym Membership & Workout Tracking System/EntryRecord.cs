namespace Gym_Membership___Workout_Tracking_System;

public class EntryRecord
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public TimeSpan Duration
    {
        get
        {
            if (EndTime < StartTime)
                throw new InvalidOperationException("End time cannot be earlier than start time.");

            return EndTime - StartTime;
        }
    }

    public EntryRecord(DateTime start, DateTime end)
    {
        StartTime = start;
        EndTime = end;
    }
}