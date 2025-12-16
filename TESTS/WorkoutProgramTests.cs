using System.Reflection;
using Gym_Membership___Workout_Tracking_System;

namespace TESTS;

public class WorkoutProgramTests
{
    [SetUp]
    public void SetUp()
    {
        ResetTrainerStaticList();
    }

    private static void ResetTrainerStaticList()
    {
        // Trainer has: private static List<Trainer> _trainers
        var f = typeof(Trainer).GetField("_trainers", BindingFlags.NonPublic | BindingFlags.Static);
        if (f == null) throw new Exception("Trainer._trainers field not found (name changed?)");
        f.SetValue(null, new List<Trainer>());
    }

    private static Trainer NewTrainer(int id, string spec = "Strength")
        => new Trainer(id, spec, new DateTime(2024, 1, 1), 1000m, 2);
    
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
}