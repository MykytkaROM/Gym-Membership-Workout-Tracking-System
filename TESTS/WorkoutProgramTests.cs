using System.Reflection;
using Gym_Membership___Workout_Tracking_System;

namespace TESTS;

public class WorkoutProgramTests
{
    private static User testUser;
    [SetUp]
    public void SetUp()
    {
        testUser = new User("John", "Pork@mail.com", "9871230", new Address("Porkvile", "Porkstreet", 67));
        ResetTrainerStaticList();
        ResetWorkoutProgramStaticList();
    }

    private static void ResetTrainerStaticList()
    {
        var f = typeof(Trainer).GetField("_trainers", BindingFlags.NonPublic | BindingFlags.Static);
        if (f == null) throw new Exception("Trainer._trainers field not found (name changed?)");
        f.SetValue(null, new List<Trainer>());
    }
    
    private static void ResetWorkoutProgramStaticList()
    {
        var f = typeof(WorkoutProgram).GetField("_workoutPrograms", BindingFlags.NonPublic | BindingFlags.Static);
        if (f == null) throw new Exception("WorkoutProgram._workoutPrograms field not found (name changed?)");
        f.SetValue(null, new List<WorkoutProgram>());
    }

    private static Trainer NewTrainer(int id, string spec = "Strength")
        => new Trainer(id, spec, new DateTime(2024, 1, 1), 1000m, 2, testUser);
    
    [Test]
    public void Constructor_ShouldSetProperties_AndLinkCreatorBothWays()
    {
        var t1 = NewTrainer(1);
        var p = new WorkoutProgram("Plan A", "Gain muscle", "Easy", 4, t1);

        Assert.AreEqual("Plan A", p.Name);
        Assert.AreEqual("Gain muscle", p.Goal);
        Assert.AreEqual("Easy", p.Difficulty);
        Assert.AreEqual(4, p.DurationWeeks);

        Assert.AreSame(t1, p.Creator);
        Assert.IsTrue(t1.WorkoutPrograms.Contains(p));
    }
    
    [Test]
    public void Name_ShouldThrow_WhenEmpty()
    {
        var t1 = NewTrainer(1);
        Assert.Throws<ArgumentNullException>(() =>
        {
            var p = new WorkoutProgram("OK", "G", "D", 1, t1);
            p.Name = "   ";
        });
    }
    
    [Test]
    public void EditCreator_ShouldThrow_WhenNull()
    {
        var t1 = NewTrainer(1);
        var p = new WorkoutProgram("Plan A", "G", "D", 4, t1);

        Assert.Throws<ArgumentNullException>(() => p.EditCreator(null));
    }
    
    [Test]
    public void FromOldTrainer_ToNewTrainer()
    {
        var t1 = NewTrainer(1);
        var t2 = NewTrainer(2);

        var p = new WorkoutProgram("Plan A", "G", "D", 4, t1);
        Assert.IsTrue(t1.WorkoutPrograms.Contains(p));
        Assert.IsFalse(t2.WorkoutPrograms.Contains(p));

        p.EditCreator(t2);

        Assert.AreSame(t2, p.Creator);
        Assert.IsFalse(t1.WorkoutPrograms.Contains(p));
        Assert.IsTrue(t2.WorkoutPrograms.Contains(p));
    }
    
    [Test]
    public void ShouldUnlink_FromTrainerList()
    {
        var t1 = NewTrainer(1);
        var p = new WorkoutProgram("Plan A", "G", "D", 4, t1);

        Assert.IsTrue(t1.WorkoutPrograms.Contains(p));

        p.DeleteCreator(t1);

        Assert.IsNull(p.Creator);
        Assert.IsFalse(t1.WorkoutPrograms.Contains(p));
    }
    
    [Test]
    public void SetCreator_SameTrainer_ShouldNotDuplicateInTrainerList()
    {
        var t1 = NewTrainer(1);
        var p = new WorkoutProgram("Plan A", "G", "D", 4, t1);
        
        p.EditCreator(t1);
        
        var count = 0;
        foreach (var wp in t1.WorkoutPrograms)
            if (ReferenceEquals(wp, p)) count++;

        Assert.AreEqual(1, count);
    }
    
    [Test]
    public void SaveLoad_CreatorAndBidirectionalLink()
    {
        var t1 = NewTrainer(1);
        var t2 = NewTrainer(2);

        var p1 = new WorkoutProgram("Plan A", "Gain muscle", "Easy", 4, t1);
        var p2 = new WorkoutProgram("Plan B", "Feet loss", "Hard", 6, t2);

        var trainersPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Trainers_test.json");
        var programsPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "WorkoutPrograms_test.json");

        Trainer.Save(trainersPath);
        WorkoutProgram.Save(programsPath);

        ResetTrainerStaticList();
        ResetWorkoutProgramStaticList();

        Trainer.Load(trainersPath);
        WorkoutProgram.Load(programsPath);

        Assert.That(WorkoutProgram.WorkoutPrograms.Count, Is.EqualTo(2));

        var loadedP1 = WorkoutProgram.WorkoutPrograms.First(p => p.Name == "Plan A");
        Assert.AreEqual(1, loadedP1.Creator.TrainerID);

        var realT1 = Trainer.GetTrainerById(1);
        Assert.IsTrue(realT1.WorkoutPrograms.Any(p => p.Name == "Plan A"));
    }

    [Test]
    public void Load_ShouldThrow_WhenTrainersNotLoaded()
    {
        var t1 = NewTrainer(1);
        var p1 = new WorkoutProgram("Plan A", "Gain muscle", "Easy", 4, t1);

        var programsPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "WorkoutPrograms_missing_trainers.json");
        WorkoutProgram.Save(programsPath);

        ResetTrainerStaticList();
        ResetWorkoutProgramStaticList();

        var ex = Assert.Throws<ArgumentException>(() => WorkoutProgram.Load(programsPath));
        Assert.That(ex.Message, Does.Contain("Trainer with this ID does not exist"));
    }

    [Test]
    public void Save_ShouldCreateFile()
    {
        var t1 = NewTrainer(1);
        var p1 = new WorkoutProgram("Plan A", "G", "D", 4, t1);

        var programsPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "WorkoutPrograms_file_test.json");
        WorkoutProgram.Save(programsPath);

        Assert.IsTrue(File.Exists(programsPath));
        Assert.Greater(new FileInfo(programsPath).Length, 5);
    }

}