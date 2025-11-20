using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gym_Membership___Workout_Tracking_System
{
    public class User
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Username can't be empty");
                }
                if (!value.All(c=> char.IsLetter(c) || c == ' ')) 
                {
                    throw new ArgumentException("Name can only contain letters and spaces");
                }
                _name = value;

            }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Email can't be empty");
                }

                try
                {
                    var mail = new MailAddress(value);
                }
                catch 
                {
                    throw new ArgumentException("Invalid email format");
                }
                _email = value;
                
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Phonenumber can't be empty");
                }
                if (!value.All(char.IsDigit)) 
                {
                    throw new ArgumentException("Phone number must contain only digits");
                }
                if (value.Length < 7 || value.Length > 15) 
                {
                    throw new ArgumentException("Phone number should be 7-15 digits long");
                }
                _phoneNumber = value;

            }
        }

        public Address Address { get; set; }
        public User(string name, string email, string phoneNumber, Address address) 
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            addUser(this);
        }

        public User(User other) 
        {
            Name = other.Name;
            Email = other.Email;
            PhoneNumber = other.PhoneNumber;
            Address = other.Address;

        }

        private static List<User> _users = new List<User>();

        [JsonIgnore]
        public static List<User> Users
        {
            get
            {
                List<User> copy = new List<User>(_users.Count);

                _users.ForEach((item) =>
                {
                    copy.Add(new User(item));
                });
                return copy;
            }
        }
        private static void addUser(User user)
        {
            if (_users.Contains(user))
            {
                throw new ArgumentException("Value is already in the list");
            }
            if (user == null)
            {
                throw new ArgumentNullException("Value must be specified");
            }
            _users.Add(user);
        }

        

        public static void save(string path = "users.json")
        {
            var dtoList = _users
                .Select(m => new UserDTO
                {
                    name = m.Name,
                    email = m.Email,
                    phoneNumber = m.PhoneNumber,
                    address = m.Address,
                })
                .ToList();

            string json = JsonSerializer.Serialize(dtoList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            Console.WriteLine("Users saved to " + path);
        }

        public static void load(string path = "users.json")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            _users.Clear();

            string json = File.ReadAllText(path);

            var dtoList = JsonSerializer.Deserialize<List<UserDTO>>(json)
                          ?? throw new ArgumentNullException("No data in JSON file");

            foreach (var dto in dtoList)
            {
                new User(
                    dto.name, 
                    dto.email, 
                    dto.phoneNumber,
                    dto.address
                );
            }
        }
    }
}
