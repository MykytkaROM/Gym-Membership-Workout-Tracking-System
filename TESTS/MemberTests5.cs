
using Gym_Membership___Workout_Tracking_System;

namespace TESTS
{
    public class MemberTests
    {
        private Member testMember;
        private EntryRecord testEntry;
        private string tempFile;
        private BoughtMembership testBoughtMembership;
        private MembershipPlan testMembershipPlan;

        [SetUp]
        public void Setup()
        {
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "[]");
            User.load(tempFile);
            Member.load(tempFile);
            testMember = new Member(1, new DateTime(2025, 1, 1), "Basic", 0, MembershipStatus.active);
            testMembershipPlan = new MembershipPlan("Gold", 3, 150, 0.1m, "Gym access");
            testBoughtMembership = new BoughtMembership(testMember, testMembershipPlan, 0.1m, new DateTime(2025, 1, 1), 3);
            testEntry = new EntryRecord(new DateTime(2025, 1, 1, 9, 0, 0), new DateTime(2025, 1, 1, 10, 0, 0));
        }
        
        [Test]
        public void TestMemberCreation()
        {
            Assert.That(testMember.MemberID, Is.EqualTo(1));
            Assert.That(testMember.MembershipType, Is.EqualTo("Basic"));
            Assert.That(testMember.TotalPoints, Is.EqualTo(0));
            Assert.That(testMember.MembershipStatus, Is.EqualTo(MembershipStatus.active));
            Assert.That(testMember.JoinDate, Is.EqualTo(new DateTime(2025, 1, 1)));
        }
        
        [Test]
        public void TestAddEntryRecordToMember()
        {
            testMember.AddEntryRecord(testEntry);

            Assert.That(testMember._ReadOnlyEntryRecords.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestAddBoughtMembership()
        {
            testMember.AddBoughtMembership(testBoughtMembership);
            Assert.That(testMember.BoughtMemberships.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void TestSaveAndLoad()
        {
            testMember.AddEntryRecord(testEntry);
            testMember.AddBoughtMembership(testBoughtMembership);

            Member.save(tempFile);

            Member.load(tempFile);

            var loadedMember = Member.Members[0];
            Assert.That(loadedMember.MemberID, Is.EqualTo(1));
            Assert.That(loadedMember.MembershipType, Is.EqualTo("Basic"));
            Assert.That(loadedMember._ReadOnlyEntryRecords.Count, Is.EqualTo(1));
            Assert.That(loadedMember.BoughtMemberships.Count, Is.EqualTo(1));
        }
        
        
        [TearDown]
        public void TearDown()
        {
            File.Delete(tempFile);
        }
    }
}