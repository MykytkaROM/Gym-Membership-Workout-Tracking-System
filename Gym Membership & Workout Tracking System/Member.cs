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
            if (_EntryRecords == null) 
            {
                _EntryRecords = new Dictionary<DateTime, EntryRecord>();
            }
            if (record == null) 
            {
                throw new ArgumentNullException("Entry record cannot be null");
            }
            if (record.Member != null) 
            {
                throw new ArgumentException("This Entry record have already specified Member");
            }
            if (record.Member == null) 
            {
                _EntryRecords.Add(record.StartTime, record);
                record.AddMember(this);
            }
        }
        
        public void RemoveEntryRecord(EntryRecord record) 
        {
            if (_EntryRecords == null)
            {
                throw new ArgumentNullException("Dictionary is empty");
            }
            if (record == null)
            {
                throw new ArgumentNullException("Entry record cannot be null");
            }
            if (record.Member != this) 
            {
                throw new ArgumentException("Entry record have different member specified");
            }
            if (record.Member.Equals(this))
            {
                _EntryRecords.Remove(record.StartTime);
                record.RemoveMember(this);
            }
        }

        
        public Member(Member other) 
        {
            if (other._EntryRecords != null)
            {
                _EntryRecords = new Dictionary<DateTime, EntryRecord>(other._EntryRecords);
            }
            else
            {
                _EntryRecords = new Dictionary<DateTime, EntryRecord>();
            }
            MemberID = other.MemberID;
            _joinDate = other.JoinDate;
            MembershipType = other.MembershipType;
            TotalPoints = other.TotalPoints;
            MembershipStatus = other.MembershipStatus;
            
            _boughtMemberships = new List<BoughtMembership>(other._boughtMemberships);
        }
        public Member(int memberID, DateTime joinDate, string membershipType, int totalPoints, MembershipStatus status) 
        {
            MemberID=memberID;
            _joinDate = joinDate;
            MembershipType = membershipType;
            TotalPoints = totalPoints;
            MembershipStatus = status;
            _EntryRecords = new Dictionary<DateTime, EntryRecord>();
            AddMemberEXT(this);
        }
        public Member(int memberID, DateTime joinDate, string membershipType, int totalPoints, MembershipStatus status, Dictionary<DateTime,EntryRecord> entryRecords)
        {
            MemberID = memberID;
            _joinDate = joinDate;
            MembershipType = membershipType;
            TotalPoints = totalPoints;
            MembershipStatus = status;
            foreach (var (key,value) in entryRecords) 
            {
                value.AddMember(this);
            }
            _EntryRecords = entryRecords;
            
            AddMemberEXT(this);
        }
        private static void AddMemberEXT(Member member)
        {
           
            if (member == null)
            {
                throw new ArgumentNullException("Value must be specified");
            }
            if (_members.Contains(member))
            {
                throw new ArgumentException("Value is already in the list");
            }
            if (_members.Any(m => m.MemberID == member.MemberID))
                throw new ArgumentException("Duplicate member ID.");

            _members.Add(member);
        }
        
        public static void RemoveMemberEXT(Member member) 
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
                EntryRecord.RemoveEntryRecordEXT(value);
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
                    EntryRecords = m._ReadOnlyEntryRecords.Values.Select(e => new EntryRecordDTO
                    {
                        StartTime = e.StartTime,
                        EndTime = e.EndTime
                    }).ToList(),
                    BoughtMemberships = m._boughtMemberships.Select(b => new BoughtMembershipDTO
                    {
                        Discount = b.Discount,
                        DateOfPurchase = b.DateOfPurchase,
                        Expires = b.Expires,
                        PlanName = b.Plan.Name
                    }).ToList()

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
                var member = new Member(
                   dto.MemberID,
                   dto.JoinDate,
                   dto.MembershipType,
                   dto.TotalPoints,
                   dto.MembershipStatus
                );
                member._EntryRecords = new Dictionary<DateTime, EntryRecord>();
                
                foreach (var entryDTO in dto.EntryRecords)
                {
                    var entryRecord = new EntryRecord(entryDTO.StartTime, entryDTO.EndTime);
                    member.AddEntryRecord(entryRecord);
                }
                
                foreach (var boughtMembershipDTO in dto.BoughtMemberships)
                {
                    var plan = MembershipPlan.GetByName(boughtMembershipDTO.PlanName);
                    _ = new BoughtMembership(member, plan, boughtMembershipDTO.Discount, boughtMembershipDTO.DateOfPurchase, boughtMembershipDTO.Expires);
                }
            }
        }
        
        private List<BoughtMembership> _boughtMemberships = new List<BoughtMembership>();
        public List<BoughtMembership> BoughtMemberships => new List<BoughtMembership>(_boughtMemberships);
        
        public void AddBoughtMembership(BoughtMembership boughtMembership)
        {
            if (boughtMembership == null) throw new ArgumentNullException(nameof(boughtMembership));
            if (!ReferenceEquals(boughtMembership.Member, this))
                throw new InvalidOperationException("BoughtMembership must refer to this member.");
            if (_boughtMemberships.Contains(boughtMembership)) return;

            _boughtMemberships.Add(boughtMembership);
        }
        
        public void RemoveBoughtMembership(BoughtMembership boughtMembership)
        {
            if (boughtMembership == null) throw new ArgumentNullException(nameof(boughtMembership));
            if (!_boughtMemberships.Contains(boughtMembership))
                throw new InvalidOperationException("This BoughtMembership is not associated with this member.");

            _boughtMemberships.Remove(boughtMembership);
        }
    }
}
