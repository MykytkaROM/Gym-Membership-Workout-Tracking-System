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
            User.load(temp);
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
    
    public class User_tests
    {
        
        [SetUp]
        public void Setup()
        {
            var temp = Path.GetTempFileName();
            File.WriteAllText(temp, "[]");
            User.load(temp);
        }
        
        [Test]
        public void User_AssignsAllPropertiesCorrectly()
        {
            var address = new Address("Warsaw", "Zlota 44", 10);
            var user = new User("John Pork", "john@gmail.com", "88005553525", address);

            Assert.That(user.Name, Is.EqualTo("John Pork"));
            Assert.That(user.Email, Is.EqualTo("john@gmail.com"));
            Assert.That(user.PhoneNumber, Is.EqualTo("88005553525"));
            Assert.That(user.Address.City, Is.EqualTo("Warsaw"));
            Assert.That(user.Address.Street, Is.EqualTo("Zlota 44"));
            Assert.That(user.Address.Building, Is.EqualTo(10));
        }
        
        [Test]
        public void User_EmptyName_ThrowsArgumentNullException()
        {
            var address = new Address("Warsaw", "Zlota 44", 1);

            Assert.Throws<ArgumentNullException>(() =>
                new User("", "john@gmail.com", "88005553525", address));
        }
        
        [Test]
        public void User_NameWithInvalidCharacters_ThrowsArgumentException()
        {
            var address = new Address("Warsaw", "Zlota 44", 1);

            Assert.Throws<ArgumentException>(() =>
                new User("John123", "john@gmail.com", "1234567", address));
        }
        
        [Test]
        public void User_EmptyEmail_ThrowsArgumentNullException()
        {
            var address = new Address("Warsaw", "Zlota 44", 1);

            Assert.Throws<ArgumentNullException>(() =>
                new User("John Pork", "", "1234567", address));
        }
        
        [Test]
        public void User_InvalidEmailFormat_ThrowsArgumentException()
        {
            var address = new Address("Warsaw", "Zlota 44", 1);

            Assert.Throws<ArgumentException>(() =>
                new User("John Pork", "not-an-email", "1234567", address));
        }
        
        [Test]
        public void User_EmptyPhoneNumber_ThrowsArgumentNullException()
        {
            var address = new Address("Warsaw", "Zlota 44", 1);

            Assert.Throws<ArgumentNullException>(() =>
                new User("John Pork", "john@gmail.com", "", address));
        }
        
        [Test]
        public void User_PhoneWithNonDigits_ThrowsArgumentException()
        {
            var address = new Address("Warsaw", "Zlota 44", 1);

            Assert.Throws<ArgumentException>(() =>
                new User("John Pork", "john@gmail.com", "1234abc", address));
        }
        
        [Test]
        public void User_PhoneTooShortOrTooLong_ThrowsArgumentException()
        {
            var address = new Address("Warsaw", "Main", 1);

            Assert.Throws<ArgumentException>(() =>
                new User("John Pork", "john@gmail.com", "123456", address));

            Assert.Throws<ArgumentException>(() =>
                new User("John Pork", "john@gmail.com", "1234567890123456", address));
        }
        
        [Test]
        public void User_CopyConstructor_CreatesDeepCopy()
        {
            var address = new Address("Warsaw", "Main", 1);
            var original = new User("John Pork", "john@gmail.com", "1234567", address);

            var copy = new User(original);

            Assert.That(copy.Name, Is.EqualTo(original.Name));
            Assert.That(copy.Email, Is.EqualTo(original.Email));
            Assert.That(copy.PhoneNumber, Is.EqualTo(original.PhoneNumber));
            Assert.That(copy.Address.City, Is.EqualTo(original.Address.City));

            copy.Address.City = "Krakow";
            Assert.That(original.Address.City, Is.EqualTo("Warsaw"));
        }

        [Test]
        public void User_UsersExtent_ContainsAllCreatedUsers()
        {
            
            
            var addr1 = new Address("Warsaw", "First", 1);
            var addr2 = new Address("Krakow", "Second", 2);

            var u1 = new User("Mykyta Romanchuk", "mykyta@gmail.com", "1111111", addr1);
            var u2 = new User("Denys Babenko", "denys@gmail.com", "2222222", addr2);

            var extent = User.Users;
            
            Assert.That(extent.Any(u => u.Email == "mykyta@gmail.com"));
            Assert.That(extent.Any(u => u.Email == "denys@gmail.com"));
        }
        
        [Test]
        public void User_UsersExtent_ReturnedListIsDeepCopy()
        {
            var addr = new Address("Warsaw", "First", 1);
            var user = new User("Sasha Zasteba", "sasha@example.com", "1111111", addr);

            var extent1 = User.Users;
            extent1[0].Name = "Changed Name";

            var extent2 = User.Users;

            Assert.That(extent2[0].Name, Is.EqualTo("Sasha Zasteba"));
        }

        [Test]
        public void User_Load_NonExistingFile_ThrowsFileNotFoundException()
        {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            Assert.False(File.Exists(path));

            Assert.Throws<FileNotFoundException>(() => User.load(path));
        }

        [Test]
        public void User_Load_EmptyArray_LeavesExtentEmpty()
        {
            var temp = Path.GetTempFileName();
            File.WriteAllText(temp, "[]");

            User.load(temp);

            var extent = User.Users;
            Assert.That(extent.Count, Is.EqualTo(0));
        }

        [Test]
        public void User_SaveAndLoad_PersistsUsersAndAddresses()
        {
            var temp = Path.GetTempFileName();

            var addr1 = new Address("Warsaw", "First", 1);
            var addr2 = new Address("Krakow", "Second", 2);

            var u1 = new User("Mykyta Romanchuk", "mykyta@example.com", "1111111", addr1);
            var u2 = new User("Denys Babenko", "denys@example.com", "2222222", addr2);

            User.save(temp);
            User.load(temp);

            var users = User.Users;

            Assert.That(users.Count, Is.EqualTo(2));

            var alice = users.Single(u => u.Email == "mykyta@example.com");
            Assert.That(alice.Name, Is.EqualTo("Mykyta Romanchuk"));
            Assert.That(alice.Address.City, Is.EqualTo("Warsaw"));
            Assert.That(alice.Address.Street, Is.EqualTo("First"));
            Assert.That(alice.Address.Building, Is.EqualTo(1));

            var bob = users.Single(u => u.Email == "denys@example.com");
            Assert.That(bob.Name, Is.EqualTo("Denys Babenko"));
            Assert.That(bob.Address.City, Is.EqualTo("Krakow"));
            Assert.That(bob.Address.Street, Is.EqualTo("Second"));
            Assert.That(bob.Address.Building, Is.EqualTo(2));
        }
    }
    public class Admin_tests
{
    [SetUp]
    public void Setup() {}

    [Test]
    public void Admin_AssignsLevelAndPermissions()
    {
        var permissions = new List<string> { "ViewUsers", "EditUsers" };

        var admin = new Admin(2, permissions);

        Assert.That(admin.AdminLevel, Is.EqualTo(2));
        CollectionAssert.AreEquivalent(permissions, admin.Permissions);
    }

    [Test]
    public void Admin_EmptyPermissions_ThrowsArgumentException()
    {
        var empty = new List<string>();

        Assert.Throws<ArgumentException>(() =>
            new Admin(1, empty));
    }

    [Test]
    public void Admin_PermissionsGetter_ReturnsCopy()
    {
        var admin = new Admin(1, new List<string> { "ViewUsers" });

        var perms = admin.Permissions;
        perms.Add("HackedPermission");

        var permsAgain = admin.Permissions;
        Assert.False(permsAgain.Contains("HackedPermission"));
        CollectionAssert.AreEquivalent(new[] { "ViewUsers" }, permsAgain);
    }

    [Test]
    public void Admin_AddNewPermission_AddsToList()
    {
        var admin = new Admin(1, new List<string> { "ViewUsers" });

        admin.ManagePermissions("EditUsers", true);

        CollectionAssert.AreEquivalent(
            new[] { "ViewUsers", "EditUsers" },
            admin.Permissions);
    }

    [Test]
    public void Admin_AddExistingPermission_DoesNotDuplicate()
    {
        var admin = new Admin(1, new List<string> { "ViewUsers" });

        admin.ManagePermissions("ViewUsers", true);

        CollectionAssert.AreEquivalent(
            new[] { "ViewUsers" },
            admin.Permissions);
    }

    [Test]
    public void Admin_RemovePermission_RemovesFromList()
    {
        var admin = new Admin(1, new List<string> { "ViewUsers", "EditUsers" });

        admin.ManagePermissions("EditUsers", false);

        CollectionAssert.AreEquivalent(
            new[] { "ViewUsers" },
            admin.Permissions);
    }

    [Test]
    public void Admin_RemoveLastPermission_ThrowsInvalidOperationException()
    {
        var admin = new Admin(1, new List<string> { "ViewUsers" });

        Assert.Throws<InvalidOperationException>(() =>
            admin.ManagePermissions("ViewUsers", false));
    }
}
    public class EntryRecord_tests
    {
        [SetUp]
        public void Setup() {}

        [Test]
        public void EntryRecord_Duration_ReturnsCorrectTimeSpan()
        {
            var start = new DateTime(2025, 1, 1, 10, 0, 0);
            var end = start.AddHours(1.5);

            var record = new EntryRecord(start, end);

            Assert.That(record.Duration, Is.EqualTo(TimeSpan.FromMinutes(90)));
        }

        
        //TODO: SASHA: change the logic and rewrite the unit test for EntryRecord
        [Test]
        public void EntryRecord_Duration_StartEqualsEnd_ReturnsZero()
        {
            var time = new DateTime(2025, 1, 1, 10, 0, 0);

            var record = new EntryRecord(time, time);

            Assert.That(record.Duration, Is.EqualTo(TimeSpan.Zero));
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