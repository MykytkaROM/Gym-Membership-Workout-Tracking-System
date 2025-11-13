using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gym_Membership___Workout_Tracking_System
{
    public class MembershipPlan
    {
        private class MembershipPlanDTO
        {
            public string Name { get; set; }
            public int DurationMonths { get; set; }
            public decimal Price { get; set; }
            public decimal? DiscountRate { get; set; }
            public string Benefits { get; set; }
        }

        private string _name;//name : string
        public string Name
        {
            get => _name;
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Name of membership can't be empty");
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
                _durationMonths = value;
            }
        }


        private decimal _price;//price : decimal

        public decimal Price { get=>_price; set 
            {
                if (value < 0) 
                {
                    throw new ArgumentException("Price can't be negative");
                }
                _price = value;
            } 
        }

        private decimal? _discountRate;//discountRate[0..1] : int

        public decimal? DiscountRate { get => _discountRate; set
            {
                
                _discountRate = value ?? 0;
 
                if (_discountRate < 0 || _discountRate > 1) {
                    throw new ArgumentOutOfRangeException("Discount should be a value between 0 and 1");
                }
                
            }
        }

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

        [JsonIgnore]
        public List<MembershipPlan> MembershipPlans { get
            {
                List<MembershipPlan> copy = new List<MembershipPlan>(_membershipPlans.Count);

                _membershipPlans.ForEach((item) =>
                {
                    copy.Add(new MembershipPlan(item));
                });
                return copy;
            }
        }
        
        public MembershipPlan(MembershipPlan other) 
        {
            Name = other.Name;
            DurationMonths = other.DurationMonths;
            Price = other.Price;
            DiscountRate = other.DiscountRate;
            Benefits = other.Benefits;
        }
        public MembershipPlan(string name, int durationMonths, decimal price, decimal? discountRate, string benefits)
        {
            Name = name;
            DurationMonths = durationMonths;
            Price = price;
            DiscountRate = discountRate;
            Benefits = benefits;
            addMembershipPlan(this);
        }
        private static void addMembershipPlan(MembershipPlan membershipPlan) 
        {
            if (_membershipPlans.Contains(membershipPlan) )
            {
                throw new ArgumentException("Value is already in the list");
            }
            if (membershipPlan == null)
            {
                throw new ArgumentNullException("Value must be specified");
            }
            _membershipPlans.Add(membershipPlan);
        }
        
        public static void save(string path = "membershipPlans.json")
        { 
            var dtoList = _membershipPlans
                .Select(m => new MembershipPlanDTO
                {
                    Name = m.Name,
                    DurationMonths = m.DurationMonths,
                    Price = m.Price,
                    DiscountRate = m.DiscountRate,
                    Benefits = m.Benefits
                })
                .ToList();

            string json = JsonSerializer.Serialize(dtoList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            Console.WriteLine("Membership plans saved to " + path);
        }
        
        public static void load(string path = "membershipPlans.json")
        {
            if (!File.Exists(path)) 
                throw new FileNotFoundException($"File not found: {path}"); 
            
            _membershipPlans.Clear();

            string json = File.ReadAllText(path);

            var dtoList = JsonSerializer.Deserialize<List<MembershipPlanDTO>>(json)
                          ?? throw new ArgumentNullException("No data in JSON file");

            foreach (var dto in dtoList)
            {
                new MembershipPlan(
                    dto.Name,
                    dto.DurationMonths,
                    dto.Price,
                    dto.DiscountRate,
                    dto.Benefits
                );
            }
        }
    }
}
