using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gym_Membership___Workout_Tracking_System
{
    public class Address
    {
        private string _city;
        public string City
        {
            get => _city;
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("City name can't be empty");
                }
                _city = value;

            }
        }

        private string _street;
        public string Street
        {
            get => _street;
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Street name can't be empty");
                }
                _street = value;

            }
        }

        private int _building;
        public int Building
        {
            get => _building; set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Building number can't be negative");
                }
                _building = value;
            }
        }

        public Address(string city, string street, int building)
        {
            City = city;
            Street = street;
            Building = building;

        }
        public Address(Address other) 
        {
            City = other.City;
            Street = other.Street;
            Building = other.Building;
        }
        public Address() { }
    }
}
