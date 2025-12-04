using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gym_Membership___Workout_Tracking_System
{
    public class Member
    {
        private int _memberID;//memberID : int
        public int MemberID
        {
            get => _memberID; set
            {
                if (value < 0) 
                {
                    throw new ArgumentException("ID cannot be negative");
                }
                foreach (var member in _members) 
                {
                    if (member.MemberID == value) 
                    {
                        throw new ArgumentException("ID should be unique");
                    }
                }
                _memberID = value;
            }
        }
        private DateTime _joinDate;//joinDate : DateTime
        public DateTime JoinDate => _joinDate;
        
        private string _membershipType;//membershipType : string
        public string MembershipType
        {
            get => _membershipType; set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Type of membership can't be empty or null");
                }
                _membershipType = value;
            }
        }
        private int _totalPoints;//totalPoints : int
        public int TotalPoints
        {
            get => _totalPoints; set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Loyalty points cannot be negative");
                }
                _totalPoints = value;
            }
        }
        private MembershipStatus _membershipStatus = MembershipStatus.not_purchased; //status : Enum
        public MembershipStatus MembershipStatus { get; set; }

        private static List<Member> _members = new List<Member>();
        public static List<Member> Members 
        {
            get
                {
                List<Member> copy = new List<Member>(_members.Count);

                _members.ForEach((item) =>
                {
                    copy.Add(new Member(item));
                });
                return copy;
            }
        }

        private Dictionary<DateTime, EntryRecord> _EntryRecords;

        public IReadOnlyDictionary<DateTime, EntryRecord> _ReadOnlyEntryRecords => _EntryRecords;


        public void AddEntryRecord(EntryRecord record) 
        {
            if (record == null) 
            {
                throw new ArgumentNullException("Entry record cannot be null");
            }
            if (record.Member != null) 
            {
                throw new ArgumentException("This Entry record have already specified Member");
            }
            _EntryRecords.Add(record.StartTime, record);
            record.AddMember(this);
        }
        public void RemoveEntryRecord(EntryRecord record) 
        {
            if (record == null)
            {
                throw new ArgumentNullException("Entry record cannot be null");
            }
            if (!record.Member.Equals(this)) 
            {
                throw new ArgumentException("Entry record have different member specified");
            }
            _EntryRecords.Remove(record.StartTime);
            record.RemoveMember(this);
        }
        public Member(Member other) 
        {
            MemberID = other.MemberID;
            _joinDate = other.JoinDate;
            MembershipType = other.MembershipType;
            TotalPoints = other.TotalPoints;
            MembershipStatus = other.MembershipStatus;
        }
        public Member(int memberID, DateTime joinDate, string membershipType, int totalPoints, MembershipStatus status) 
        {
            MemberID=memberID;
            _joinDate = joinDate;
            MembershipType = membershipType;
            TotalPoints = totalPoints;
            MembershipStatus = status;
            AddMembers(this);
        }
        public Member(int memberID, DateTime joinDate, string membershipType, int totalPoints, MembershipStatus status, Dictionary<DateTime,EntryRecord> entryRecords)
        {
            MemberID = memberID;
            _joinDate = joinDate;
            MembershipType = membershipType;
            TotalPoints = totalPoints;
            MembershipStatus = status;
            
            AddMembers(this);
        }
        private static void AddMembers(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("Value must be specified");
            }
            if (_members.Contains(member))
            {
                throw new ArgumentException("Value is already in the list");
            }
            
            _members.Add(member);
        }
        
        public static void RemoveMembers(Member member) 
        {
            
            if (member == null)
            {
                throw new ArgumentNullException("Value must be specified");
            }
            if (!_members.Contains(member))
            {
                throw new ArgumentException("Value is not in the list");
            }
            _members.Remove(member);
            foreach (var(key,value) in member._EntryRecords) 
            {
                value.RemoveMember(member);
                EntryRecord.RemoveEntryRecords(value);

            }

        }
        public static void save(string path = "Member.json")
        {
            var dtoList = _members
                .Select(m => new MemberDTO
                {
                    MemberID = m.MemberID,
                    JoinDate = m.JoinDate,
                    MembershipType = m.MembershipType,
                    TotalPoints = m.TotalPoints,
                    MembershipStatus = m.MembershipStatus,


                })
                .ToList();

            string json = JsonSerializer.Serialize(dtoList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            Console.WriteLine("Members saved to " + path);
        }

        public static void load(string path = "Member.json")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            _members.Clear();

            string json = File.ReadAllText(path);

            var dtoList = JsonSerializer.Deserialize<List<MemberDTO>>(json)
                          ?? throw new ArgumentNullException("No data in JSON file");

            foreach (var dto in dtoList)
            {
                new Member(
                   dto.MemberID,
                   dto.JoinDate,
                   dto.MembershipType,
                   dto.TotalPoints,
                   dto.MembershipStatus
                );
            }
        }
    }
}
