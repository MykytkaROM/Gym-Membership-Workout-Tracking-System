using Gym_Membership___Workout_Tracking_System;

namespace TESTS
{

    public class RecordTest4
    {
        public class EntryRecord_tests
        {
            [SetUp]
            public void Setup()
            {
            }

            [Test]
            public void EntryRecord_Duration_ReturnsCorrectTimeSpan()
            {
                var start = new DateTime(2025, 1, 1, 10, 0, 0);
                var end = start.AddHours(1.5);

                var record = new EntryRecord(start, end);

                Assert.That(record.Duration, Is.EqualTo(TimeSpan.FromMinutes(90)));
            }

            [Test]
            public void EntryRecord_Duration_StartEqualsEnd_ThrowsInvalidOperationException()
            {
                var time = new DateTime(2025, 1, 1, 10, 0, 0);

                var record = new EntryRecord(time, time);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    var _ = record.Duration;
                });
            }

            [Test]
            public void EntryRecord_Duration_EndBeforeStart_ThrowsInvalidOperationException()
            {
                var start = new DateTime(2025, 1, 1, 12, 0, 0);
                var end = new DateTime(2025, 1, 1, 11, 0, 0);

                var record = new EntryRecord(start, end);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    var _ = record.Duration;
                });
            }

            [Test]
            public void EntryRecord_ChangingTimes_AffectsDuration()
            {
                var start = new DateTime(2025, 1, 1, 10, 0, 0);
                var end = start.AddHours(1);

                var record = new EntryRecord(start, end);
                Assert.That(record.Duration, Is.EqualTo(TimeSpan.FromHours(1)));

                record.EndTime = start.AddHours(2);
                Assert.That(record.Duration, Is.EqualTo(TimeSpan.FromHours(2)));
            }
        }
    }
}