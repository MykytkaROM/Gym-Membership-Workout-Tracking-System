using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_Membership___Workout_Tracking_System
{
    public class Member
    {
        private int _memberID;//memberID : int
        public int MemberID
        {
            get => _memberID; set
            {
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

            }
        }
        private int _totalPoints;//totalPoints : int
        public int TotalPoints
        {
            get => _totalPoints; set
            {

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
        }
        public Member() 
        {

        }
    }
}
