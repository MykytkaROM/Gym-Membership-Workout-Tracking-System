namespace Gym_Membership___Workout_Tracking_System;
public class Exercise
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Exercise name cannot be empty");
            _name = value;
        }
    }

    public string MuscleGroup { get; set; }
    public bool EquipmentRequired { get; set; }

    public Exercise(string name, string muscleGroup, bool equipmentRequired)
    {
        Name = name;
        MuscleGroup = muscleGroup;
        EquipmentRequired = equipmentRequired;
    }
    
    public Exercise(Exercise e)
    {
        Name = e.Name;
        MuscleGroup = e.MuscleGroup;
        EquipmentRequired = e.EquipmentRequired;
    }
}