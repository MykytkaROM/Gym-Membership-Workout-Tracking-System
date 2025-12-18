
using Gym_Membership___Workout_Tracking_System;
using System.Reflection;

namespace TESTS
{
    public class Member_tests
    {
        private Member testMember;
        private EntryRecord testEntry;
        private string tempFile;
        private BoughtMembership testBoughtMembership;
        private MembershipPlan testMembershipPlan;
        private User testUser;

        [SetUp]
        public void Setup()
        {
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "[]");
            User.Load(tempFile);
            Member.Load(tempFile);
            testUser = new User("John", "Pork@mail.com", "9871230", new Address("Porkvile", "Porkstreet", 67));
            testMember = new Member(1, new DateTime(2025, 1, 1), "Basic", 0, MembershipStatus.active, testUser);
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
            testMember.AddUser(testUser);

            Member.Save(tempFile);

            Member.Load(tempFile);

            var loadedMember = Member.Members[0];
            Assert.That(loadedMember.MemberID, Is.EqualTo(1));
            Assert.That(loadedMember.MembershipType, Is.EqualTo("Basic"));
            Assert.That(loadedMember._ReadOnlyEntryRecords.Count, Is.EqualTo(1));
            Assert.That(loadedMember.BoughtMemberships.Count, Is.EqualTo(1));
            Assert.That(loadedMember.User, Is.Not.Null);
            Assert.That(loadedMember.User.Name, Is.EqualTo(testUser.Name));
            Assert.That(loadedMember.User.Email, Is.EqualTo(testUser.Email));
            Assert.That(loadedMember.User.PhoneNumber, Is.EqualTo(testUser.PhoneNumber));
        }
        
        [Test]
        public void TestAddAndRemoveEntryRecord()
        {
            testMember.AddEntryRecord(testEntry);
            Assert.That(testMember._ReadOnlyEntryRecords.Count, Is.EqualTo(1));
            
            testMember.RemoveEntryRecord(testEntry);
            Assert.That(testMember._ReadOnlyEntryRecords.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void TestRemoveBoughtMembership()
        {
            testMember.AddBoughtMembership(testBoughtMembership);
            Assert.That(testMember.BoughtMemberships.Count, Is.EqualTo(1));
            
            testMember.RemoveBoughtMembership(testBoughtMembership);
            Assert.That(testMember.BoughtMemberships.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void TestSaveAndLoadEmptyData()
        {
            _ = new Member(2, DateTime.Now, "Basic", 0, MembershipStatus.active, testUser);
            
            Member.Save(tempFile);
            Member.Load(tempFile);

            var loadedMember = Member.Members.Single(m => m.MemberID == 2);
            Assert.That(loadedMember.MemberID, Is.EqualTo(2));
            Assert.That(loadedMember.MembershipType, Is.EqualTo("Basic"));
            Assert.That(loadedMember._ReadOnlyEntryRecords.Count, Is.EqualTo(0));
            Assert.That(loadedMember.BoughtMemberships.Count, Is.EqualTo(0));
            Assert.That(loadedMember.User, Is.Not.Null);
        }

        [Test]
        public void TestAddNullEntryRecord()
        {
            Assert.Throws<ArgumentNullException>(() => testMember.AddEntryRecord(null));
        }

        [Test]
        public void TestAddNullBoughtMembership()
        {
            Assert.Throws<ArgumentNullException>(() => testMember.AddBoughtMembership(null));
        }

        [Test]
        public void TestRemoveEntryRecordWithNonMatchingMember()
        {
            var differentMember = new Member(2, new DateTime(2025, 1, 1), "Silver", 0, MembershipStatus.active, new User());
            testEntry.AddMember(differentMember);

            Assert.Throws<ArgumentException>(() => testMember.RemoveEntryRecord(testEntry));
        }
        
        [Test]
        public void TestAddEntryRecordWithAssignedMember()
        {
            testEntry.AddMember(testMember);
            Assert.Throws<ArgumentException>(() => testMember.AddEntryRecord(testEntry));
        }
        
        [Test]
        public void TestSaveAndLoadMultipleMembers()
        {
            var secondMember = new Member(2, new DateTime(2025, 1, 2), "Gold", 100, MembershipStatus.active, testUser);
            secondMember.AddEntryRecord(testEntry);

            var planSnapshot = new MembershipPlan("Gold", 1, 150, 0.1m, "Gym access", false);
            _ = new BoughtMembership(secondMember, planSnapshot, 0.1m, DateTime.Now, 3);

            Member.Save(tempFile);
            Member.Load(tempFile);

            var loadedMembers = Member.Members;

            Assert.That(loadedMembers.Count, Is.EqualTo(2));
            Assert.That(loadedMembers.Any(m => m.MemberID == 2), Is.True);
        }
        
        [TearDown]
        public void TearDown()
        {
            File.Delete(tempFile);
        }
    }
}