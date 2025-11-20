using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }
    }
}
