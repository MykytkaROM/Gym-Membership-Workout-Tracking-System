namespace Gym_Membership___Workout_Tracking_System

{
    public class BoughtMembership
    {
        private decimal _discount;
        public decimal Discount
        {
            get => _discount;
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException("Discount must be between 0 and 1.");
                }
                _discount = value;
            }
        }

        public DateTime DateOfPurchase { get; set; }

        private int _expires;
        public int Expires
        {
            get => _expires;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Expires must be positive.");
                }
                _expires = value;
            }
        }

        private Member _member;
        public Member Member
        {
            get => _member;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                if (_member != null && !ReferenceEquals(_member, value))
                    throw new InvalidOperationException("Member cannot be changed once set.");

                _member = value;
            }
        }

        private MembershipPlan _plan;
        public MembershipPlan Plan
        {
            get => _plan;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                if (_plan != null && !ReferenceEquals(_plan, value))
                    throw new InvalidOperationException("Plan cannot be changed once set.");

                _plan = value;
            }
        }

        public BoughtMembership(Member member, MembershipPlan plan, decimal discount, DateTime dateOfPurchase, int expires)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            if (plan == null) throw new ArgumentNullException(nameof(plan));

            Member = member;
            Plan = plan;

            Discount = discount;
            DateOfPurchase = dateOfPurchase;
            Expires = expires;

            member.AddBoughtMembership(this);
            plan.AddBoughtMembership(this);
        }
        
        public BoughtMembership(BoughtMembership other)
            : this(other.Member, other.Plan, other.Discount, other.DateOfPurchase, other.Expires)
        {
        }

        public void Delete()
        {
            if (_member != null) _member.RemoveBoughtMembership(this);
            if (_member != null) _plan.RemoveBoughtMembership(this);
        }
    }
}
