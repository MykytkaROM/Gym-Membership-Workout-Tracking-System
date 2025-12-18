using Gym_Membership___Workout_Tracking_System;

namespace TESTS
{
    public class User_tests
    {
        
        [SetUp]
        public void Setup()
        {
            var temp = Path.GetTempFileName();
            File.WriteAllText(temp, "[]");
            User.Load(temp);
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

            Assert.Throws<FileNotFoundException>(() => User.Load(path));
        }

        [Test]
        public void User_Load_EmptyArray_LeavesExtentEmpty()
        {
            var temp = Path.GetTempFileName();
            File.WriteAllText(temp, "[]");

            User.Load(temp);

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

            User.Save(temp);
            User.Load(temp);

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
}