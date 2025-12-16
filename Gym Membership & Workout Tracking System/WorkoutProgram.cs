namespace Gym_Membership___Workout_Tracking_System;

public class WorkoutProgram
{
    private string _name;
    public string Name { get => _name; set {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Name cannot be empty");
            _name = value;
        }
    }
    public string Goal { get; set; }
    public string Difficulty { get; set; }
    public int DurationWeeks { get; set; }
    private Trainer _creator;
    public Trainer Creator => _creator;
    
    public WorkoutProgram(string name, string goal, string difficulty, int durationWeeks, Trainer creator)
    {
        Name = name;
        Goal = goal;
        Difficulty = difficulty;
        DurationWeeks = durationWeeks;

        AddCreator(creator);
    }
    
    public void AddCreator(Trainer creator)
    {
        if (_creator != null)
            throw new InvalidOperationException("Creator is already added. If you meant to modify it use EditCreator() instead");
        if (creator == null)
            throw new ArgumentNullException("Creator should be not null");

        _creator = creator;
        
        if (!creator.ContainsWorkoutProgram(this))
        {
            creator.AddWorkoutProgram(this);
        }
    }
    
    public void EditCreator(Trainer newCreator)
    {
        if (newCreator == null) throw new ArgumentNullException("Creator should be not null");
        if (_creator == null) throw new InvalidOperationException("No creator set. Use AddCreator() first");
        if (newCreator.Equals(_creator)) return;

        DeleteCreator(_creator);
        AddCreator(newCreator);
    }

    public void DeleteCreator(Trainer creator)
    {
        if (creator == null) throw new ArgumentNullException("Creator should be not null");
        if (_creator == null) throw new InvalidOperationException("No creator set. Use AddCreator() first");
        if (!_creator.Equals(creator)) throw new ArgumentException("Creator specified is different from creator in this program");
        
        _creator = null;
        if (creator.ContainsWorkoutProgram(this))
        {
            creator.DeleteWorkoutProgram(this);
        }
    }
}