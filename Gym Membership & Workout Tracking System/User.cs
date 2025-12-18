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
        public User() { }
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
            
            AddUserEXT(this);
        }
        public User(string name, string email, string phoneNumber, Address address,Admin admin)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            AddAdmin(admin);
            AddUserEXT(this);
        }

        public User(string name, string email, string phoneNumber, Address address, Admin admin, Trainer trainer)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            AddAdmin(admin);
            AddTrainer(trainer);
            AddUserEXT(this);
        }
        public User(string name, string email, string phoneNumber, Address address, Admin admin, Member member)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            AddAdmin(admin);
            AddMember(member);  
            AddUserEXT(this);
        }
        public User(string name, string email, string phoneNumber, Address address, Admin admin, Member member, Trainer trainer)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            AddAdmin(admin);
            AddMember(member);
            AddTrainer(trainer);
            AddUserEXT(this);
        }
        public User(string name, string email, string phoneNumber, Address address, Trainer trainer)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            AddTrainer(trainer);
            AddUserEXT(this);
        }

        public User(string name, string email, string phoneNumber, Address address, Trainer trainer, Member member)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            AddTrainer(trainer);
            AddMember(member);
            AddUserEXT(this);
        }
        public User(string name, string email, string phoneNumber, Address address, Member member)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            AddMember(member);
            AddUserEXT(this);
        }



        public User(User other) 
        {
            Name = other.Name;
            Email = other.Email;
            PhoneNumber = other.PhoneNumber;
            Address = new Address(other.Address);

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

        private Admin? _admin;

        public Admin? Admin { 
            get
            {
                if(_admin == null) return null;
                return _admin;
            } 
        }

        public void AddAdmin(Admin admin) 
        {
            if (admin == null)
            {
                throw new ArgumentNullException("Admin cannot be null");
            }
            _admin = admin;

            if (admin.User == null)
            {
                admin.AddUser(this);
            }
            
        }

        public void RemoveAdmin(Admin admin) 
        {
            if (_admin == null) throw new ArgumentNullException("Admin should be added first");
            if (admin == null)
            {
                throw new ArgumentNullException("Admin cannot be null");
            }
            if (!_admin.Equals(admin)) throw new ArgumentException("Admin specified is different from admin in this user");
            _admin = null;
            if (admin.User.Equals(this))
            {
                admin.RemoveUser(this);
            }
            
            

        }

        private Member? _member;
        public Member? Member
        {
            get
            {
                if (_member == null) return null;
                return _member;
            }
        }
        public void AddMember(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("Member cannot be null");
            }
            _member = member;
            if (member.User == null)
            {
                member.AddUser(this);
            }
            
        }

        public void RemoveMember(Member member)
        {
            if (_member == null) throw new ArgumentNullException("Member should be added first");
            if (member == null)
            {
                throw new ArgumentNullException("Member cannot be null");
            }
            if (!_member.Equals(member)) throw new ArgumentException("Member specified is different from member in this user");
            _member = null;
            if (member.User.Equals(this))
            {
                member.RemoveUser(this);
            }
            
            
        }

        private Trainer? _trainer;
        public Trainer? Trainer
        {
            get
            {
                if (_trainer == null) return null;
                return _trainer;
            }
        }
        public void AddTrainer(Trainer trainer)
        {
            if (trainer == null)
            {
                throw new ArgumentNullException("Trainer cannot be null");
            }
            _trainer = trainer;
            if (trainer.User == null)
            {
                trainer.AddUser(this);
            }
            
        }

        public void RemoveTrainer(Trainer trainer)
        {
            if (_trainer == null) throw new ArgumentNullException("Trainer should be added first");
            if (trainer == null)
            {
                throw new ArgumentNullException("Trainer cannot be null");
            }
            if (!_trainer.Equals(trainer)) throw new ArgumentException("Trainer specified is different from trainer in this user");
            _trainer = null;
            if (trainer.User.Equals(this))
            {
                trainer.RemoveUser(this);
            }
            
        }
        private static void AddUserEXT(User user)
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
        public static void RemoveUserEXT(User user) 
        {
            if (user == null)
            {
                throw new ArgumentNullException("Value must be specified");
            }
            if (!_users.Contains(user))
            {
                throw new ArgumentException("Value is not in the list");
            }
            _users.Remove(user);
            if (user._admin != null) user._admin.RemoveUser(user);
            if (user._member != null) user._member.RemoveUser(user);
            if(user._trainer != null) user._trainer.RemoveUser(user);
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
