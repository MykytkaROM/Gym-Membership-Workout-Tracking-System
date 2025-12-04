using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public DateTime JoinDate
        {
            get => _joinDate; set
            {

            }
        }
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
        public MembershipStatus MembershipStatus { get => _membershipStatus; set { } }

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
        public Member(Member other) 
        {
            MemberID = other.MemberID;
            JoinDate = other.JoinDate;
            MembershipType = other.MembershipType;
            TotalPoints = other.TotalPoints;
            MembershipStatus = other.MembershipStatus;
            
            _boughtMemberships = new List<BoughtMembership>(other._boughtMemberships);
        }
        public Member() 
        {

        }
        
        private List<BoughtMembership> _boughtMemberships = new List<BoughtMembership>();
        public List<BoughtMembership> BoughtMemberships
        {
            get
            {
                return new List<BoughtMembership>(_boughtMemberships);
            }
        }
        
        public void AddBoughtMembership(BoughtMembership boughtMembership)
        {
            if (boughtMembership == null)
            {
                throw new ArgumentNullException(nameof(boughtMembership));
            }

            if (boughtMembership.Member != this)
            {
                throw new InvalidOperationException("BoughtMembership must refer to this member.");
            }

            _boughtMemberships.Add(boughtMembership);
        }
    }
}
