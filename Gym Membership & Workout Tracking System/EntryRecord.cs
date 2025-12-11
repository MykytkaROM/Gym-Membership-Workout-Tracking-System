using System.Text.Json;

namespace Gym_Membership___Workout_Tracking_System;

public class EntryRecord
{
    public DateTime StartTime { get;  }
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

    public EntryRecord(DateTime start, DateTime end, Member member)
    {
        StartTime = start;
        EndTime = end;
        AddEntryRecordsEXT(this);
        AddMember(member);
        
    }

    public EntryRecord(DateTime start, DateTime end)
    {
        StartTime = start;
        EndTime = end;
        
        AddEntryRecordsEXT(this);
    }

    private Member? _member;
    public Member? Member { get => _member; }

    public void AddMember(Member member) 
    {
        if (member == null)
        {
            throw new ArgumentNullException("Member cannot be null");
        }
        if(!member._ReadOnlyEntryRecords.ContainsKey(StartTime))
        {
            member.AddEntryRecord(this);
        }
        _member = member;
    }
   
    public void RemoveMember(Member member) 
    {
        if (member == null)
        {
            throw new ArgumentNullException("Member cannot be null");
        }
        if (member._ReadOnlyEntryRecords.ContainsKey(StartTime)) 
        {
            
            member.RemoveEntryRecord(this);
        }
        _member = null;
    }
    
    private static List<EntryRecord> _entries = new List<EntryRecord>();
    public static List<EntryRecord> Entries
    {
        get
        {
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
        _member = entryRecord.Member;
    }
    public static void save(string path = "EntryRecords.json")
    {
        var dtoList = _entries
            .Select(m => new EntryRecordDTO
            {
                StartTime = m.StartTime,
                EndTime = m.EndTime,

            })
            .ToList();

        string json = JsonSerializer.Serialize(dtoList, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        Console.WriteLine("EntryRecords saved to " + path);
    }

    public static void load(string path = "EntryRecords.json")
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        _entries.Clear();

        string json = File.ReadAllText(path);

        var dtoList = JsonSerializer.Deserialize<List<EntryRecordDTO>>(json)
                      ?? throw new ArgumentNullException("No data in JSON file");

        foreach (var dto in dtoList)
        {
            new EntryRecord(
               dto.StartTime,
                dto.EndTime,
                dto.Member
            );
        }
    }
    private static void AddEntryRecordsEXT(EntryRecord entryRecord)
    {

        if (_entries.Contains(entryRecord))
        {
            throw new ArgumentException("Value is already in the list");
        }
        if (entryRecord == null)
        {
            throw new ArgumentNullException("Value must be specified");
        }
        _entries.Add(entryRecord);
    }
    public static void RemoveEntryRecordEXT(EntryRecord entryRecord) 
    {
        if (entryRecord==null) 
        {
            throw new ArgumentNullException("Entry record cannot be null");
        }
        _entries.Remove(entryRecord);
        if (entryRecord.Member == null) 
        {
            throw new ArgumentNullException("This entry record does not have member");
        }
        entryRecord.Member.RemoveEntryRecord(entryRecord);
        entryRecord._member = null;
        
    }

}