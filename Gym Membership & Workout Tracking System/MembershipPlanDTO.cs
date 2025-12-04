using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_Membership___Workout_Tracking_System
{
    public class MembershipPlanDTO
    {
        public string Name { get; set; }
        public int DurationMonths { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountRate { get; set; }
        public string Benefits { get; set; }
    }
}
