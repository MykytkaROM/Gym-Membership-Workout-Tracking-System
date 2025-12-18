using System.Collections;
using System.Reflection;
using Gym_Membership___Workout_Tracking_System;

namespace TESTS;

public class ExerciseTest
{
    [SetUp]
    public void SetUp()
    {
        ResetStaticList(typeof(Trainer), "_trainers");
        ResetStaticList(typeof(WorkoutProgram), "_workoutPrograms");
        ResetStaticList(typeof(Exercise), "_exercises");
    }

    private static void ResetStaticList(Type type, string fieldName)
    {
        var f = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
        if (f == null) return;

        var value = f.GetValue(null);
        if (value is IList list) list.Clear();
    }
    
    private static Trainer NewTrainer(int id, string spec = "Strength")
        => new Trainer(id, spec, new DateTime(2024, 1, 1), 1000m, 2);

    private static WorkoutProgram NewProgram(Trainer t, string name = "Plan A")
        => new WorkoutProgram(name, "Goal", "Easy", 4, t);
    
    [Test]
    public void Name_WhenEmpty()
    {
        var ex = new Exercise("Bench", "Chest", true);
        Assert.Throws<ArgumentNullException>(() => ex.Name = "   ");
    }
    
    [Test]
    public void Constructor_ShouldSetProperties()
    {
        var ex = new Exercise("Squat", "Legs", true);
        Assert.AreEqual("Squat", ex.Name);
        Assert.AreEqual("Legs", ex.MuscleGroup);
        Assert.AreEqual(true, ex.EquipmentRequired);
    }
    
    [Test]
    public void CopyConstructor_ShouldCopyProperties()
    {
        var ex1 = new Exercise("Pull-up", "Back", false);
        var ex2 = new Exercise(ex1);

        Assert.AreEqual(ex1.Name, ex2.Name);
        Assert.AreEqual(ex1.MuscleGroup, ex2.MuscleGroup);
        Assert.AreEqual(ex1.EquipmentRequired, ex2.EquipmentRequired);
    }
    
    [Test]
    public void AddExercise_ShouldLinkBothWays()
    {
        var t = NewTrainer(1);
        var p = NewProgram(t);
        var e = new Exercise("Deadlift", "Back", true);

        var addExercise = typeof(WorkoutProgram).GetMethod("AddExercise", new[] { typeof(Exercise) });
        var containsExercise = typeof(WorkoutProgram).GetMethod("ContainsExercise", new[] { typeof(Exercise) });
        var containsProgram = typeof(Exercise).GetMethod("ContainsWorkoutProgram", new[] { typeof(WorkoutProgram) });

        addExercise.Invoke(p, new object[] { e });

        Assert.IsTrue((bool)containsExercise.Invoke(p, new object[] { e }));
        Assert.IsTrue((bool)containsProgram.Invoke(e, new object[] { p }));
    }
    
    [Test]
    public void AddWorkoutProgram_ShouldLinkBothWays()
    {
        var t = NewTrainer(1);
        var p = NewProgram(t);
        var e = new Exercise("Plank", "Core", false);

        var addProgram = typeof(Exercise).GetMethod("AddWorkoutProgram", new[] { typeof(WorkoutProgram) });
        var containsExercise = typeof(WorkoutProgram).GetMethod("ContainsExercise", new[] { typeof(Exercise) });
        var containsProgram = typeof(Exercise).GetMethod("ContainsWorkoutProgram", new[] { typeof(WorkoutProgram) });

        addProgram.Invoke(e, new object[] { p });

        Assert.IsTrue((bool)containsProgram.Invoke(e, new object[] { p }));
        Assert.IsTrue((bool)containsExercise.Invoke(p, new object[] { e }));
    }
    
    [Test]
    public void DeleteExercise_ShouldUnlinkBothWays()
    {
        var t = NewTrainer(1);
        var p = NewProgram(t);

        var e1 = new Exercise("Curl", "Biceps", true);
        var e2 = new Exercise("Push-up", "Chest", false);

        var addExercise = typeof(WorkoutProgram).GetMethod("AddExercise", new[] { typeof(Exercise) });
        var deleteExercise = typeof(WorkoutProgram).GetMethod("DeleteExercise", new[] { typeof(Exercise) });
        var containsExercise = typeof(WorkoutProgram).GetMethod("ContainsExercise", new[] { typeof(Exercise) });
        var containsProgram = typeof(Exercise).GetMethod("ContainsWorkoutProgram", new[] { typeof(WorkoutProgram) });
        
        addExercise.Invoke(p, new object[] { e1 });
        addExercise.Invoke(p, new object[] { e2 });

        deleteExercise.Invoke(p, new object[] { e1 });

        Assert.IsFalse((bool)containsExercise.Invoke(p, new object[] { e1 }));
        Assert.IsFalse((bool)containsProgram.Invoke(e1, new object[] { p }));
    }
    
    [Test]
    public void Save_ShouldCreateJsonFile()
    {
        var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Exercises_test_save.json");

        new Exercise("Bench Press", "Chest", true);
        new Exercise("Plank", "Core", false);

        Exercise.save(path);

        Assert.IsTrue(File.Exists(path));
        Assert.Greater(new FileInfo(path).Length, 5);
    }
    
    [Test]
    public void SaveLoad_ShouldRestoreExercises()
    {
        var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Exercises_test_roundtrip.json");

        new Exercise("Squat", "Legs", true);
        new Exercise("Pull-up", "Back", false);

        Exercise.save(path);
        
        ResetStaticList(typeof(Exercise), "_exercises");

        Exercise.load(path);

        Assert.That(Exercise.Exercises.Count, Is.EqualTo(2));

        Assert.IsTrue(Exercise.Exists("Squat", "Legs", true));
        Assert.IsTrue(Exercise.Exists("Pull-up", "Back", false));

        var ex = Exercise.GetExercise("Pull-up", "Back", false);
        Assert.That(ex.Name, Is.EqualTo("Pull-up"));
        Assert.That(ex.MuscleGroup, Is.EqualTo("Back"));
        Assert.That(ex.EquipmentRequired, Is.EqualTo(false));
    }
    
    [Test]
    public void Load_ShouldThrow_WhenFileDoesNotExist()
    {
        var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Exercises_file_does_not_exist.json");
        if (File.Exists(path)) File.Delete(path);

        Assert.Throws<FileNotFoundException>(() => Exercise.load(path));
    }
}