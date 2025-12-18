using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_Membership___Workout_Tracking_System
{
    public class TrainersDTO
    {
        public int TrainerID {  get; set; }
        public string Specialization { get; set; }
        public DateTime HireDate { get; set; }
        public decimal BaseSalary { get; set; }
        public int YearOfExpirience { get; set; }
        public int? MentorID { get; set; }
        public List<int> TraineeIDs { get; set; } = new List<int>();
        public User User { get; set; }
    }
}
