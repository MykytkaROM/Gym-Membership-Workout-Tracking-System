using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_Membership___Workout_Tracking_System
{
    public class MemberDTO
    {
        public int MemberID { get; set; }
        public DateTime JoinDate { get; set; }
        public string MembershipType { get; set; }
        public int TotalPoints { get; set; }
        public MembershipStatus MembershipStatus { get; set; }
        public List<EntryRecordDTO> EntryRecords { get; set; } = new();
        public List<BoughtMembershipDTO> BoughtMemberships { get; set; } = new();
        public User User { get; set; }
    }
}
