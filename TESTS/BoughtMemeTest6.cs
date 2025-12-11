using System.Reflection;
using Gym_Membership___Workout_Tracking_System;

namespace TESTS{
    
    public class BoughtMembership_tests
    {
        private Member testMember;
        private MembershipPlan testPlan;

        [SetUp]
        public void Setup()
        {
            typeof(Member).GetField("_members", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new List<Member>());
            testMember = new Member(1, DateTime.Now, "Basic", 0, MembershipStatus.active);
            testPlan = new MembershipPlan("Gold", 3, 150, 0.1m, "Gym access");
        }

        [Test]
        public void TestBoughtMembershipCreation()
        {
            var date = new DateTime(2025, 1, 1);
            var bm = new BoughtMembership(testMember, testPlan, 0.2m, date, 3);

            Assert.That(bm.Member, Is.EqualTo(testMember));
            Assert.That(bm.Plan, Is.EqualTo(testPlan));
            Assert.That(bm.Discount, Is.EqualTo(0.2m));
            Assert.That(bm.DateOfPurchase, Is.EqualTo(date));
            Assert.That(bm.Expires, Is.EqualTo(3));
        }

        [Test]
        public void TestDiscountValidation()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new BoughtMembership(testMember, testPlan, -0.1m, DateTime.Now, 3));

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new BoughtMembership(testMember, testPlan, 1.5m, DateTime.Now, 3));
        }

        [Test]
        public void TestExpirationValidation()
        {
            Assert.Throws<ArgumentException>(() =>
                new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 0));

            Assert.Throws<ArgumentException>(() =>
                new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, -5));
        }

        [Test]
        public void TestNullMember()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new BoughtMembership(null, testPlan, 0.1m, DateTime.Now, 3));
        }

        [Test]
        public void TestNullPlan()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new BoughtMembership(testMember, null, 0.1m, DateTime.Now, 3));
        }

        [Test]
        public void TestCopyConstructor()
        {
            var original = new BoughtMembership(testMember, testPlan, 0.5m,
                                                new DateTime(2024, 5, 10), 6);

            var copy = new BoughtMembership(original);

            Assert.That(copy.Member, Is.EqualTo(original.Member));
            Assert.That(copy.Plan, Is.EqualTo(original.Plan));
            Assert.That(copy.Discount, Is.EqualTo(original.Discount));
            Assert.That(copy.DateOfPurchase, Is.EqualTo(original.DateOfPurchase));
            Assert.That(copy.Expires, Is.EqualTo(original.Expires));
        }

        [Test]
        public void TestPropertySetters()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 3);

            bm.Discount = 0.5m;
            bm.Expires = 10;

            Assert.That(bm.Discount, Is.EqualTo(0.5m));
            Assert.That(bm.Expires, Is.EqualTo(10));
        }

        [Test]
        public void TestMemberSetterNull()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.3m, DateTime.Now, 3);

            Assert.Throws<ArgumentNullException>(() => bm.Member = null);
        }

        [Test]
        public void TestPlanSetterNull()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.3m, DateTime.Now, 3);

            Assert.Throws<ArgumentNullException>(() => bm.Plan = null);
        }
    }
}