namespace Gym_Membership___Workout_Tracking_System;

public class EntryRecord
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public TimeSpan Duration
    {
        get
        {
            if (EndTime <= StartTime)
                throw new InvalidOperationException("End time cannot be equal or earlier than start time.");

            return EndTime - StartTime;
        }
    }

    public EntryRecord(DateTime start, DateTime end)
    {
        StartTime = start;
        EndTime = end;
    }

    private Member _member;
    public Member Member { get => _member; set 
        {

        }
    } 

    private static List<EntryRecord> _entries = new List<EntryRecord>();
    public static List<EntryRecord> Entries
    {
        get{
            List<EntryRecord> copy = new List<EntryRecord>(_entries.Count);

            _entries.ForEach((item) =>
            {
                copy.Add(new EntryRecord(item));
            });
            return copy;
        }
    }

    public EntryRecord(EntryRecord entryRecord) 
    {
        StartTime = entryRecord.StartTime;
        EndTime = entryRecord.EndTime;
        Member = entryRecord.Member;
    }
}