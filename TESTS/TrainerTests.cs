using Gym_Membership___Workout_Tracking_System;

namespace TESTS;

public class TrainerTests
{
    private string _testFilePath;
    [SetUp]
    public void Setup()
    {
        _testFilePath = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "trainers_test.json"
            );
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
    }
    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
    }
    private Trainer CreateTrainer(int id)
    {
        return new Trainer(
            id,
            "Strength",
            DateTime.Now,
            1000m,
            5,
            new User()
        );
    }

    [Test]
    public void AddMentor_ShouldAddTraineeToMentor()
    {
        var mentor = CreateTrainer(1);
        var trainee = CreateTrainer(2);

        trainee.AddMentor(mentor);

        Assert.That(trainee.Mentor, Is.EqualTo(mentor));
        Assert.That(
            mentor.Trainees.Any(t => t.TrainerID == trainee.TrainerID),
            Is.True
        );
    }

    [Test]
    public void AddTrainee_ShouldSetMentorOnTrainee()
    {
        var mentor = CreateTrainer(3);
        var trainee = CreateTrainer(4);

        mentor.AddTrainee(trainee);

        Assert.That(trainee.Mentor, Is.EqualTo(mentor));
        Assert.That(
            mentor.Trainees.Any(t => t.TrainerID == trainee.TrainerID),
            Is.True
        );
    }

    [Test]
    public void DeleteMentor_ShouldRemoveTraineeFromMentor()
    {
        var mentor = CreateTrainer(5);
        var trainee = CreateTrainer(6);
        trainee.AddMentor(mentor);

        trainee.DeleteMentor(mentor);

        Assert.That(trainee.Mentor, Is.Null);
        Assert.That(
            mentor.Trainees.Any(t => t.TrainerID == trainee.TrainerID),
            Is.False
        );
    }

    [Test]
    public void DeleteTrainee_ShouldClearMentorFromTrainee()
    {
        var mentor = CreateTrainer(7);
        var trainee = CreateTrainer(8);
        mentor.AddTrainee(trainee);

        mentor.DeleteTrainee(trainee);

        Assert.That(trainee.Mentor, Is.Null);
        Assert.That(
            mentor.Trainees.Any(t => t.TrainerID == trainee.TrainerID),
            Is.False
        );
    }

    [Test]
    public void EditMentor_ShouldMoveTraineeToNewMentor()
    {
        var oldMentor = CreateTrainer(9);
        var newMentor = CreateTrainer(10);
        var trainee = CreateTrainer(11);

        trainee.AddMentor(oldMentor);
        trainee.EditMentor(newMentor);

        Assert.That(trainee.Mentor, Is.EqualTo(newMentor));

        Assert.That(
            oldMentor.Trainees.Any(t => t.TrainerID == trainee.TrainerID),
            Is.False
        );

        Assert.That(
            newMentor.Trainees.Any(t => t.TrainerID == trainee.TrainerID),
            Is.True
        );
    }

    [Test]
    public void EditTrainee_ShouldReplaceOldWithNew_TraineeMentorUpdated()
    {
        var mentor = CreateTrainer(12);
        var oldTrainee = CreateTrainer(13);
        oldTrainee.AddMentor(mentor);
        var newTrainee = CreateTrainer(14);

        Assert.That(oldTrainee.Mentor, Is.EqualTo(mentor));
        Assert.That(newTrainee.Mentor, Is.Null);

        mentor.EditTrainee(oldTrainee, newTrainee);

        Assert.That(mentor.Trainees?.Any(t => t.TrainerID == oldTrainee.TrainerID), Is.False);
        Assert.That(mentor.Trainees?.Any(t => t.TrainerID == newTrainee.TrainerID), Is.True);

        Assert.That(oldTrainee.Mentor, Is.Null);
        Assert.That(newTrainee.Mentor, Is.EqualTo(mentor));
    }

    [Test]
    public void SaveAndLoad_ShouldPreserveMentorTraineeRelationship()
    {
        var mentor = CreateTrainer(100);
        var trainee1 = CreateTrainer(101);
        var trainee2 = CreateTrainer(102);

        mentor.AddTrainee(trainee1);
        mentor.AddTrainee(trainee2);

        Trainer.Save(_testFilePath);
        Trainer.Load(_testFilePath);

        var loadedMentor = Trainer.GetTrainerById(100);
        var loadedTrainee1 = Trainer.GetTrainerById(101);
        var loadedTrainee2 = Trainer.GetTrainerById(102);

        Assert.That(loadedTrainee1.Mentor.TrainerID, Is.EqualTo(loadedMentor.TrainerID));
        Assert.That(loadedTrainee2.Mentor.TrainerID, Is.EqualTo(loadedMentor.TrainerID));

        Assert.That(
            loadedMentor.Trainees.Any(t => t.TrainerID == loadedTrainee1.TrainerID),
            Is.True
        );
        Assert.That(
            loadedMentor.Trainees.Any(t => t.TrainerID == loadedTrainee2.TrainerID),
            Is.True
        );
    }

    [Test]
    public void SaveAndLoad_ShouldPreserveSingleMentorRelationship()
    {
        var mentor = CreateTrainer(200);
        var trainee = CreateTrainer(201);

        trainee.AddMentor(mentor);

        Trainer.Save(_testFilePath);
        Trainer.Load(_testFilePath);

        var loadedMentor = Trainer.GetTrainerById(200);
        var loadedTrainee = Trainer.GetTrainerById(201);

        Assert.That(loadedTrainee.Mentor.TrainerID, Is.EqualTo(loadedMentor.TrainerID));
        Assert.That(
            loadedMentor.Trainees.Any(t => t.TrainerID == loadedTrainee.TrainerID),
            Is.True
        );
    }
    [Test]
    public void SaveAndLoad_ShouldHandleTrainerWithoutRelations()
    {
        var trainer = CreateTrainer(300);

        Trainer.Save(_testFilePath);
        Trainer.Load(_testFilePath);

        var loadedTrainer = Trainer.GetTrainerById(300);

        Assert.That(loadedTrainer.Trainees, Is.Null);
        Assert.That(loadedTrainer.Mentor, Is.Null);
    }

}
