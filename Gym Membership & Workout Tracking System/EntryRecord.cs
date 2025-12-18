using System.Text.Json;
using Gym_Membership___Workout_Tracking_System.DTO;

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
        if (_member == null) throw new ArgumentNullException("Member should be added first");
        if (member == null)
        {
            throw new ArgumentNullException("Member cannot be null");
        }
        if (!_member.Equals(member)) throw new ArgumentException("Member specified is different from member in this entry record"); 
        if (member._ReadOnlyEntryRecords.ContainsKey(StartTime)) 
        {
            member.RemoveEntryRecord(this);
        }
        _member = null;
        RemoveEntryRecordEXT(this);
        
        
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
   public static void Save(string path = "EntryRecords.json")
    {
        var dtoList = _entries
            .Select(m => new EntryRecordDTO
            {
                StartTime = m.StartTime,
                EndTime = m.EndTime,
                Member = new MemberDTO
                { 
                    MemberID = m.Member.MemberID,
                    JoinDate = m.Member.JoinDate,
                    MembershipType = m.Member.MembershipType,
                    TotalPoints = m.Member.TotalPoints,
                    MembershipStatus = m.Member.MembershipStatus,
                    User = new UserDTO
                    {
                        name = m.Member.User.Name,
                        email = m.Member.User.Email,
                        phoneNumber = m.Member.User.PhoneNumber,
                        address = new AddressDTO
                        {
                            City = m.Member.User.Address.City,
                            Street = m.Member.User.Address.Street,
                            Building = m.Member.User.Address.Building
                        }
                    }
                }

            })
            .ToList();

        string json = JsonSerializer.Serialize(dtoList, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        Console.WriteLine("EntryRecords saved to " + path);
    }

    public static void Load(string path = "EntryRecords.json")
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        _entries.Clear();

        string json = File.ReadAllText(path);

        var dtoList = JsonSerializer.Deserialize<List<EntryRecordDTO>>(json)
                      ?? throw new ArgumentNullException("No data in JSON file");

        foreach (var dto in dtoList)
        {
            if (dto.Member == null) throw new ArgumentNullException("Member data is missing in JSON");
            Member member = new Member();
            member.MemberID = dto.Member.MemberID;
            member.JoinDate = dto.Member.JoinDate;
            member.MembershipType = dto.Member.MembershipType;
            member.TotalPoints = dto.Member.TotalPoints;
            member.MembershipStatus = dto.Member.MembershipStatus;
            if (dto.Member.User != null)
            {
                Address address = null;
                if (dto.Member.User.address != null)
                {
                    address = new Address(
                        dto.Member.User.address.Street,
                        dto.Member.User.address.City,
                        dto.Member.User.address.Building
                    );
                }
                
                member.User = new User(
                    dto.Member.User.name,
                    dto.Member.User.email,
                    dto.Member.User.phoneNumber,
                    address
                );
            }
            
            new EntryRecord(
               dto.StartTime,
                dto.EndTime,
                member
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
    private static void RemoveEntryRecordEXT(EntryRecord entryRecord) 
    {
        if (entryRecord==null) 
        {
            throw new ArgumentNullException("Entry record cannot be null");
        }
        _entries.Remove(entryRecord);
        
        
    }

}