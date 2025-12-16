using System.Text.Json;

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
    
    private readonly List<Exercise> _exercises = new List<Exercise>();
    public List<Exercise> Exercises => _exercises.ToList();
    
    private static List<WorkoutProgram> _workoutPrograms = new List<WorkoutProgram>();
    public List<WorkoutProgram> WorkoutPrograms => _workoutPrograms.ToList();
    
    private static void AddWorkoutProgramEXT(WorkoutProgram program)
    {
        if (program == null) throw new ArgumentNullException("Value must be specified");
        if (_workoutPrograms.Contains(program)) throw new ArgumentException("Value is already in the list");

        _workoutPrograms.Add(program);
    }
    
    public WorkoutProgram(WorkoutProgram p)
    {
        Name = p.Name;
        Goal = p.Goal;
        Difficulty = p.Difficulty;
        DurationWeeks = p.DurationWeeks;

        _creator = p.Creator;

        _exercises = p._exercises.Select(e => new Exercise(e)).ToList();
    }
    
    public WorkoutProgram(string name, string goal, string difficulty, int durationWeeks, Trainer creator)
    {
        Name = name;
        Goal = goal;
        Difficulty = difficulty;
        DurationWeeks = durationWeeks;

        AddCreator(creator);
        AddWorkoutProgramEXT(this);
    }
    
    public bool ContainsExercise(Exercise exercise)
    {
        if (exercise == null) throw new ArgumentNullException("Exercise cannot be null");
        return _exercises.Contains(exercise);
    }
    
    public void AddExercise(Exercise exercise)
    {
        if (exercise == null) throw new ArgumentNullException("Exercise cannot be null");
        if (_exercises.Contains(exercise)) throw new ArgumentException("This exercise is already in the program");

        _exercises.Add(exercise);
    }
    
    public void AddExerciseAt(Exercise exercise, int index)
    {
        if (exercise == null) throw new ArgumentNullException("Exercise cannot be null");
        if (_exercises.Contains(exercise)) throw new ArgumentException("This exercise is already in the program");
        if (index < 0 || index > _exercises.Count) throw new ArgumentOutOfRangeException("Index is out of range");

        _exercises.Insert(index, exercise);
    }
    
    public void DeleteExercise(Exercise exercise)
    {
        if (exercise == null) throw new ArgumentNullException("Exercise cannot be null");
        if (!_exercises.Contains(exercise)) throw new ArgumentException("This exercise is not in the program");
        if (_exercises.Count == 1) throw new InvalidOperationException("WorkoutProgram must contain at least 1 exercise");

        _exercises.Remove(exercise);
    }
    
    public void MoveExercise(Exercise exercise, int newIndex)
    {
        if (exercise == null) throw new ArgumentNullException("Exercise cannot be null");
        if (!_exercises.Contains(exercise)) throw new ArgumentException("This exercise is not in the program");
        if (newIndex < 0 || newIndex >= _exercises.Count) throw new ArgumentOutOfRangeException("Index is out of range");

        _exercises.Remove(exercise);
        _exercises.Insert(newIndex, exercise);
    }
    
    public void ValidateExercises()
    {
        if (_exercises.Count < 1) throw new InvalidOperationException("WorkoutProgram must contain at least 1 exercise");
    }
    
    public static void save(string path = "WorkoutPrograms.json")
    {
        var dtoList = _workoutPrograms
            .Select(p => new WorkoutProgramsDTO
            {
                Name = p.Name,
                Goal = p.Goal,
                Difficulty = p.Difficulty,
                DurationWeeks = p.DurationWeeks,
                CreatorID = p.Creator.TrainerID,
                Exercises = p._exercises.Select(e => new ExerciseDTO
                {
                    Name = e.Name,
                    MuscleGroup = e.MuscleGroup,
                    EquipmentRequired = e.EquipmentRequired
                }).ToList()
            })
            .ToList();

        string json = JsonSerializer.Serialize(dtoList, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        Console.WriteLine("WorkoutPrograms saved to " + path);
    }

    public static void load(string path = "WorkoutPrograms.json")
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        _workoutPrograms.Clear();

        string json = File.ReadAllText(path);

        var dtoList = JsonSerializer.Deserialize<List<WorkoutProgramsDTO>>(json)
                      ?? throw new ArgumentNullException("No data in JSON file");

        foreach (var dto in dtoList)
        {
            Trainer creator = Trainer.GetTrainerById(dto.CreatorID);

            WorkoutProgram program = new WorkoutProgram(
                dto.Name,
                dto.Goal,
                dto.Difficulty,
                dto.DurationWeeks,
                creator
            );

            if (dto.Exercises != null)
            {
                foreach (var ex in dto.Exercises)
                {
                    program.AddExercise(new Exercise(ex.Name, ex.MuscleGroup, ex.EquipmentRequired));
                }
            }
        }
    }
}