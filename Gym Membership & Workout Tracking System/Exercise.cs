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
    
    private readonly List<WorkoutProgram> _workoutPrograms = new List<WorkoutProgram>();
    public List<WorkoutProgram> WorkoutPrograms
    {
        get
        {
            return new List<WorkoutProgram>(_workoutPrograms);
        }
    }

    public bool ContainsWorkoutProgram(WorkoutProgram program)
    {
        if (program == null) throw new ArgumentNullException("WorkoutProgram cannot be null");
        return _workoutPrograms.Contains(program);
    }
    
    public void AddWorkoutProgram(WorkoutProgram program)
    {
        if (program == null) throw new ArgumentNullException("WorkoutProgram cannot be null");
        if (_workoutPrograms.Contains(program)) throw new ArgumentException("This WorkoutProgram is already in the list");

        _workoutPrograms.Add(program);
        
        if (!program.ContainsExercise(this))
        {
            program.AddExercise(this);
        }
    }

    public void RemoveWorkoutProgram(WorkoutProgram program)
    {
        if (program == null) throw new ArgumentNullException("WorkoutProgram cannot be null");
        if (!_workoutPrograms.Contains(program)) throw new ArgumentException("WorkoutProgram is not in the list");

        _workoutPrograms.Remove(program);

        if (program.ContainsExercise(this))
        {
            program.DeleteExercise(this);
        }
    }
}