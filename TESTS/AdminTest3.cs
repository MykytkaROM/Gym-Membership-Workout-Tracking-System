using Gym_Membership___Workout_Tracking_System;

namespace TESTS
{
    public class Admin_tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Admin_AssignsLevelAndPermissions()
        {
            var permissions = new List<string> { "ViewUsers", "EditUsers" };

            var admin = new Admin(2, permissions, new User());

            Assert.That(admin.AdminLevel, Is.EqualTo(2));
            CollectionAssert.AreEquivalent(permissions, admin.Permissions);
        }

        [Test]
        public void Admin_EmptyPermissions_ThrowsArgumentException()
        {
            var empty = new List<string>();

            Assert.Throws<ArgumentException>(() =>
                new Admin(1, empty, new User()));
        }

        [Test]
        public void Admin_PermissionsGetter_ReturnsCopy()
        {
            var admin = new Admin(1, new List<string> { "ViewUsers" }, new User());

            var perms = admin.Permissions;
            perms.Add("HackedPermission");

            var permsAgain = admin.Permissions;
            Assert.False(permsAgain.Contains("HackedPermission"));
            CollectionAssert.AreEquivalent(new[] { "ViewUsers" }, permsAgain);
        }

        [Test]
        public void Admin_AddNewPermission_AddsToList()
        {
            var admin = new Admin(1, new List<string> { "ViewUsers" }, new User());

            admin.ManagePermissions("EditUsers", true);

            CollectionAssert.AreEquivalent(
                new[] { "ViewUsers", "EditUsers" },
                admin.Permissions);
        }

        [Test]
        public void Admin_AddExistingPermission_DoesNotDuplicate()
        {
            var admin = new Admin(1, new List<string> { "ViewUsers" }, new User());

            admin.ManagePermissions("ViewUsers", true);

            CollectionAssert.AreEquivalent(
                new[] { "ViewUsers" },
                admin.Permissions);
        }

        [Test]
        public void Admin_RemovePermission_RemovesFromList()
        {
            var admin = new Admin(1, new List<string> { "ViewUsers", "EditUsers" }, new User());

            admin.ManagePermissions("EditUsers", false);

            CollectionAssert.AreEquivalent(
                new[] { "ViewUsers" },
                admin.Permissions);
        }

        [Test]
        public void Admin_RemoveLastPermission_ThrowsInvalidOperationException()
        {
            var admin = new Admin(1, new List<string> { "ViewUsers" }, new User());

            Assert.Throws<InvalidOperationException>(() =>
                admin.ManagePermissions("ViewUsers", false));
        }
    }
}