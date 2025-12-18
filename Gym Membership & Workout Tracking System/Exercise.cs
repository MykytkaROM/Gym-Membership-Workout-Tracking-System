using System.Text.Json;
using Gym_Membership___Workout_Tracking_System.DTO;

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
        
        AddExerciseEXT(this);
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
    
    private static readonly List<Exercise> _exercises = new List<Exercise>();
    
    public static List<Exercise> Exercises
    {
        get { return new List<Exercise>(_exercises); }
    }
    
    private static string BuildKey(string name, string muscleGroup, bool equipmentRequired)
    {
        string n = (name ?? "").Trim().ToLowerInvariant();
        string m = (muscleGroup ?? "").Trim().ToLowerInvariant();
        return n + "||" + m + "||" + equipmentRequired;
    }
    
    private string Key => BuildKey(Name, MuscleGroup, EquipmentRequired);
    
    public static bool Exists(string name, string muscleGroup, bool equipmentRequired)
    {
        string key = BuildKey(name, muscleGroup, equipmentRequired);
        return _exercises.Any(e => e.Key == key);
    }
    
    public static Exercise GetExercise(string name, string muscleGroup, bool equipmentRequired)
    {
        string key = BuildKey(name, muscleGroup, equipmentRequired);
        var ex = _exercises.FirstOrDefault(e => e.Key == key);
        if (ex == null) throw new ArgumentException("Exercise not found in registry");
        return ex;
    }
    
    private static void AddExerciseEXT(Exercise ex)
    {
        if (ex == null) throw new ArgumentNullException("Value must be specified");
        if (_exercises.Any(e => e.Key == ex.Key))
            throw new ArgumentException("Exercise with the same (Name, MuscleGroup, EquipmentRequired) already exists");

        _exercises.Add(ex);
    }
    
    public static void save(string path = "Exercises.json")
    {
        var dtoList = _exercises
            .Select(e => new ExerciseDTO
            {
                Name = e.Name,
                MuscleGroup = e.MuscleGroup,
                EquipmentRequired = e.EquipmentRequired
            })
            .ToList();

        string json = JsonSerializer.Serialize(dtoList, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        Console.WriteLine("Exercises saved to " + path);
    }

    public static void load(string path = "Exercises.json")
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        _exercises.Clear();

        string json = File.ReadAllText(path);
        var dtoList = JsonSerializer.Deserialize<List<ExerciseDTO>>(json)
                      ?? throw new ArgumentNullException("No data in JSON file");

        foreach (var dto in dtoList)
        {
            new Exercise(dto.Name, dto.MuscleGroup, dto.EquipmentRequired);
        }
    }
}