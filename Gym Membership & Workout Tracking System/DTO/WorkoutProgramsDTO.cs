using System.Collections.Generic;

namespace Gym_Membership___Workout_Tracking_System.DTO
{
    public class WorkoutProgramsDTO
    {
        public string Name { get; set; }
        public string Goal { get; set; }
        public string Difficulty { get; set; }
        public int DurationWeeks { get; set; }

        public int CreatorID { get; set; }

        public List<ExerciseDTO> Exercises { get; set; } = new List<ExerciseDTO>();
    }
}