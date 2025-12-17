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

            Assert.That(bm.Plan, Is.Not.Null);
            Assert.That(ReferenceEquals(bm.Plan, testPlan), Is.False);

            Assert.That(bm.Plan.Name, Is.EqualTo(testPlan.Name));
            Assert.That(bm.Plan.DurationMonths, Is.EqualTo(testPlan.DurationMonths));
            Assert.That(bm.Plan.Price, Is.EqualTo(testPlan.Price));
            Assert.That(bm.Plan.DiscountRate, Is.EqualTo(testPlan.DiscountRate));
            Assert.That(bm.Plan.Benefits, Is.EqualTo(testPlan.Benefits));

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

            Assert.That(copy.Plan, Is.Not.Null);
            Assert.That(original.Plan, Is.Not.Null);
            Assert.That(ReferenceEquals(copy.Plan, original.Plan), Is.False);

            Assert.That(copy.Plan.Name, Is.EqualTo(original.Plan.Name));
            Assert.That(copy.Plan.DurationMonths, Is.EqualTo(original.Plan.DurationMonths));
            Assert.That(copy.Plan.Price, Is.EqualTo(original.Plan.Price));
            Assert.That(copy.Plan.DiscountRate, Is.EqualTo(original.Plan.DiscountRate));
            Assert.That(copy.Plan.Benefits, Is.EqualTo(original.Plan.Benefits));

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

        /*[Test]
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
        }*/
        private static bool MemberContains(Member m, BoughtMembership bm) =>
            m.BoughtMemberships.Any(x => ReferenceEquals(x, bm));

        [Test]
        public void BoughtMembership_Creation_LinksMemberAndPlan()
        {
            var m = new Member(101, DateTime.Now, "Basic", 0, MembershipStatus.active);
            var p = new MembershipPlan("Gold", 1, 150, 0.1m, "Gym access", false);

            var bm = new BoughtMembership(m, p, 0.2m, new DateTime(2025, 1, 1), 1);

            Assert.That(bm.Member, Is.SameAs(m));

            // If you keep snapshot plans inside BoughtMembership, this is expected:
            // bm.Plan is NOT the same instance as p.
            // If you DON'T use snapshots, replace Not.SameAs with SameAs.
            Assert.That(bm.Plan, Is.Not.SameAs(p));

            Assert.That(MemberContains(m, bm), Is.True);
            Assert.That(m.BoughtMemberships.Count(x => ReferenceEquals(x, bm)), Is.EqualTo(1));

            Assert.That(p.BoughtMembership, Is.SameAs(bm));
        }

        [Test]
        public void BoughtMembership_Delete_UnlinksMemberAndPlan()
        {
            var m = new Member(102, DateTime.Now, "Basic", 0, MembershipStatus.active);
            var p = new MembershipPlan("Silver", 1, 120, null, "Gym access", false);

            var bm = new BoughtMembership(m, p, 0.0m, DateTime.Now, 1);

            bm.Delete();

            Assert.That(bm.Member, Is.Null);
            Assert.That(bm.Plan, Is.Null);

            Assert.That(MemberContains(m, bm), Is.False);
            Assert.That(p.BoughtMembership, Is.Null);
        }

        [Test]
        public void Member_CanHaveMultipleBoughtMemberships()
        {
            var m = new Member(103, DateTime.Now, "Basic", 0, MembershipStatus.active);

            var p1 = new MembershipPlan("Gold-1", 1, 150, null, "Gym access", false);
            var p2 = new MembershipPlan("Gold-2", 1, 150, null, "Gym access", false);

            var bm1 = new BoughtMembership(m, p1, 0.1m, DateTime.Now.AddDays(-10), 1);
            var bm2 = new BoughtMembership(m, p2, 0.0m, DateTime.Now, 1);

            Assert.That(m.BoughtMemberships.Count, Is.EqualTo(2));
            Assert.That(MemberContains(m, bm1), Is.True);
            Assert.That(MemberContains(m, bm2), Is.True);

            Assert.That(p1.BoughtMembership, Is.SameAs(bm1));
            Assert.That(p2.BoughtMembership, Is.SameAs(bm2));
        }

        [Test]
        public void MembershipPlan_CannotBeLinkedToTwoBoughtMemberships()
        {
            var m1 = new Member(104, DateTime.Now, "Basic", 0, MembershipStatus.active);
            var m2 = new Member(105, DateTime.Now, "Basic", 0, MembershipStatus.active);

            var p = new MembershipPlan("UniquePlan", 1, 150, null, "Gym access", false);

            _ = new BoughtMembership(m1, p, 0.1m, DateTime.Now, 1);

            Assert.Throws<InvalidOperationException>(() =>
                _ = new BoughtMembership(m2, p, 0.2m, DateTime.Now, 1)
            );
        }
        
                [Test]
        public void BoughtMembership_Extent_IncludesCreatedInstance()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);
            Assert.That(BoughtMembership.BoughtMemberships.Any(x => ReferenceEquals(x, bm)), Is.True);
        }

        [Test]
        public void BoughtMembership_Extent_RemovesOnDelete()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);
            bm.Delete();
            Assert.That(BoughtMembership.BoughtMemberships.Any(x => ReferenceEquals(x, bm)), Is.False);
        }

        [Test]
        public void BoughtMembership_Delete_CanBeCalledTwice()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);
            bm.Delete();
            Assert.DoesNotThrow(() => bm.Delete());
        }

        [Test]
        public void BoughtMembership_Discount_Allows0And1()
        {
            var bm0 = new BoughtMembership(testMember, testPlan, 0.0m, DateTime.Now, 1);
            Assert.That(bm0.Discount, Is.EqualTo(0.0m));

            var m2 = new Member(2, DateTime.Now, "Basic", 0, MembershipStatus.active);
            var p2 = new MembershipPlan("P2", 1, 150, null, "Gym", false);
            var bm1 = new BoughtMembership(m2, p2, 1.0m, DateTime.Now, 1);
            Assert.That(bm1.Discount, Is.EqualTo(1.0m));
        }

        [Test]
        public void BoughtMembership_Expires_AllowsMinimumOne()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);
            Assert.That(bm.Expires, Is.EqualTo(1));
        }

        [Test]
        public void BoughtMembership_Discount_SetterRejectsInvalid()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => bm.Discount = -0.01m);
            Assert.Throws<ArgumentOutOfRangeException>(() => bm.Discount = 1.01m);
        }

        [Test]
        public void BoughtMembership_Expires_SetterRejectsInvalid()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);
            Assert.Throws<ArgumentException>(() => bm.Expires = 0);
            Assert.Throws<ArgumentException>(() => bm.Expires = -1);
        }

        [Test]
        public void BoughtMembership_AddMember_Twice_DoesNotDuplicate()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);

            bm.AddBoughtMembership(testMember);

            Assert.That(testMember.BoughtMemberships.Count(x => ReferenceEquals(x, bm)), Is.EqualTo(1));
        }

        [Test]
        public void BoughtMembership_RemoveMember_AllowsRelinkSameMember()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);

            bm.RemoveBoughtMembership(testMember);
            Assert.That(bm.Member, Is.Null);
            Assert.That(MemberContains(testMember, bm), Is.False);

            bm.AddBoughtMembership(testMember);
            Assert.That(bm.Member, Is.SameAs(testMember));
            Assert.That(MemberContains(testMember, bm), Is.True);
        }

        [Test]
        public void BoughtMembership_AddMember_DifferentMember_Throws()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);
            var other = new Member(99, DateTime.Now, "Basic", 0, MembershipStatus.active);

            Assert.Throws<InvalidOperationException>(() => bm.AddBoughtMembership(other));
        }

        [Test]
        public void BoughtMembership_RemoveMember_WrongMember_Throws()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);
            var other = new Member(98, DateTime.Now, "Basic", 0, MembershipStatus.active);

            Assert.Throws<InvalidOperationException>(() => bm.RemoveBoughtMembership(other));
        }

        [Test]
        public void ReuseSamePlanInstance_AfterDelete_IsAllowed()
        {
            var m1 = new Member(201, DateTime.Now, "Basic", 0, MembershipStatus.active);
            var m2 = new Member(202, DateTime.Now, "Basic", 0, MembershipStatus.active);
            var p = new MembershipPlan("Reusable", 1, 150, null, "Gym", false);

            var bm1 = new BoughtMembership(m1, p, 0.1m, DateTime.Now, 1);
            bm1.Delete();

            Assert.DoesNotThrow(() => _ = new BoughtMembership(m2, p, 0.2m, DateTime.Now, 1));
        }

        [Test]
        public void Member_BoughtMemberships_ReturnsCopy()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);

            var list = testMember.BoughtMemberships;
            list.Clear();

            Assert.That(MemberContains(testMember, bm), Is.True);
            Assert.That(testMember.BoughtMemberships.Count, Is.EqualTo(1));
        }

        [Test]
        public void BoughtMemberships_StaticProperty_ReturnsCopy()
        {
            var bm = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);

            var list = BoughtMembership.BoughtMemberships;
            list.Clear();

            Assert.That(BoughtMembership.BoughtMemberships.Any(x => ReferenceEquals(x, bm)), Is.True);
        }

        [Test]
        public void BoughtMembership_Construction_DoesNotAddSnapshotToPlanExtent()
        {
            var before = MembershipPlan.MembershipPlans.Count;
            _ = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);
            var after = MembershipPlan.MembershipPlans.Count;

            Assert.That(after, Is.EqualTo(before));
        }

        [Test]
        public void CopyConstructor_DoesNotBreakOriginalPlanBackLink()
        {
            var original = new BoughtMembership(testMember, testPlan, 0.1m, DateTime.Now, 1);
            var copy = new BoughtMembership(original);

            Assert.That(original.Plan, Is.Not.Null);
            Assert.That(copy.Plan, Is.Not.Null);

            Assert.That(original.Plan.BoughtMembership, Is.SameAs(original));
            Assert.That(copy.Plan.BoughtMembership, Is.SameAs(copy));
        }
    }
}