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

        public int DurationMonths
        {
            get => _durationMonths; set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Duration of membership can't be negative");
                }
            }
        }


        private decimal _price;//price : decimal

        public decimal Price { get=>_price; set 
            {
                if (value < 0) 
                {
                    throw new ArgumentException("Price can't be negative");
                }
            } }

        private decimal? _discountRate;//discountRate[0..1] : int

        public decimal? DiscountRate { get => _discountRate; set
            {
                if (value == null)
                {
                    _discountRate = 0;
                }
                if (value < 0 || value > 1) {
                    throw new ArgumentException("Discount should be a value between 0 and 1");
                }
                _discountRate = value;


            } }

        private string _benefits;//benefits : string

        public string Benefits
        {
            get => _benefits;
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Benefits can't be empty");
                }
                _benefits = value;

            }
        }

        private static decimal _minPrice = 100;//minPrice = 100 : decimal
        public decimal MinPrice { get => _minPrice; }

        private static List<MembershipPlan> _membershipPlans = new List<MembershipPlan>();

        

        public MembershipPlan(string name, int durationMonths, decimal price, decimal? discountRate, string benefits)
        {
            Name = name;
            DurationMonths = durationMonths;
            Price = price;
            DiscountRate = discountRate;
            Benefits = benefits;
            if (!_membershipPlans.Contains(this))
            {
                _membershipPlans.Add(this);
            }
        }
    }
}
