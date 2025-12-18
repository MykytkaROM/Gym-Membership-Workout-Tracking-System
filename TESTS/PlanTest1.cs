using Gym_Membership___Workout_Tracking_System;

namespace TESTS
{
    public class MembershipPlan_tests
    {
        [SetUp]
        public void Setup()
        {
            var temp = Path.GetTempFileName();
            File.WriteAllText(temp, "[]");
            MembershipPlan.load(temp);
            User.Load(temp);
        }

        [Test]
        public void MembershipPlan_AssignsPropertiesCorrectly()
        {
            var plan = new MembershipPlan("Gold", 3, 150, 0.1m, "Gym access");

            Assert.That(plan.Name, Is.EqualTo("Gold"));
            Assert.That(plan.DurationMonths, Is.EqualTo(3));
            Assert.That(plan.Price, Is.EqualTo(150));
            Assert.That(plan.DiscountRate, Is.EqualTo(0.1m));
            Assert.That(plan.Benefits, Is.EqualTo("Gym access"));
        }

        [Test]
        public void MembershipPlan_EmptyName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new MembershipPlan("", 2, 150, 0.1m, "Some Stuff"));
        }

        [Test]
        public void MembershipPlan_NegativeDuration_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new MembershipPlan("Basic", -1, 100, 0.1m, "Some Stuff"));
        }

        [Test]
        public void MembershipPlan_NegativePrice_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new MembershipPlan("Basic", 1, -1, 0.1m, "Benefits"));
        }

        [Test]
        public void MembershipPlan_DiscountBelowZero_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new MembershipPlan("Basic", 2, 100, -0.5m, "Benefits"));
        }

        [Test]
        public void MembershipPlan_DiscountAboveOne_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new MembershipPlan("Basic", 2, 100, 1.5m, "Benefits"));
        }

        [Test]
        public void MembershipPlan_NullDiscount_SetsDiscountToZero()
        {
            var plan = new MembershipPlan("Basic", 2, 100, null, "Benefits");
            Assert.That(plan.DiscountRate, Is.EqualTo(0));
        }

        [Test]
        public void MembershipPlan_EmptyBenefits_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new MembershipPlan("Basic", 2, 100, 0.2m, ""));
        }

        [Test]
        public void MembershipPlan_CopyConstructor_CopiesAllFieldsCorrectly()
        {
            var original = new MembershipPlan("Basic", 2, 100, 0.2m, "All access");
            var copy = new MembershipPlan(original);

            Assert.That(copy.Name, Is.EqualTo(original.Name));
            Assert.That(copy.Price, Is.EqualTo(original.Price));
            Assert.That(copy.DurationMonths, Is.EqualTo(original.DurationMonths));
            Assert.That(copy.DiscountRate, Is.EqualTo(original.DiscountRate));
        }

        [Test]
        public void MembershipPlan_SaveAndLoadIn_KeepsSystemWorkingForNewPlans()
        {
            var plan = new MembershipPlan("Silver", 3, 120, 0.15m, "Gym");

            var temp = Path.GetTempFileName();
            
            MembershipPlan.save(temp);
            MembershipPlan.load(temp);
            

            var loadedPlans = MembershipPlan.MembershipPlans;

            Assert.That(loadedPlans.Count, Is.EqualTo(1));
            var loaded = loadedPlans[0];

            Assert.That(loaded.Name, Is.EqualTo("Silver"));
            Assert.That(loaded.DurationMonths, Is.EqualTo(3));
            Assert.That(loaded.Price, Is.EqualTo(120));
            Assert.That(loaded.DiscountRate, Is.EqualTo(0.15m));
            Assert.That(loaded.Benefits, Is.EqualTo("Gym"));
        }
        
        
    }
}