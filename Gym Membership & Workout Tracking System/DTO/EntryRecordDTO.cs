using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_Membership___Workout_Tracking_System.DTO
{
    public class EntryRecordDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public MemberDTO Member { get; set; }
    }
}
