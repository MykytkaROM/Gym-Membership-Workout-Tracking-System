using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_Membership___Workout_Tracking_System
{
    public class MembershipPlan
    {
        private string _name;//name : string
            public string Name
        {
            get => _name; 
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name of membership can't be empty");
                }
                _name = value;

            }
        }
        private int _durationMonths;//durationMonths : int

        public int DurationMonths { get; set; }
       

        private decimal _price;//price : decimal

        public decimal Price { get; set; }

        private int _discountRate;//discountRate[0..1] : int

        public int DiscountRate { get; set; }

        private string _benefits;//benefits : string

        public string Benefits
        {
            get => _benefits;
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name of membership can't be empty");
                }
                _benefits = value;

            }
        }

        private static decimal _minPrice = 100;//minPrice = 100 : decimal
        public decimal MinPrice { get => _minPrice; }
    }
}
