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

        private DateTime _dateOfPurchase;
        public DateTime DateOfPurchase
        {
            get => _dateOfPurchase;
            set => _dateOfPurchase = value;
        }

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
            set => _member = value ?? throw new ArgumentNullException(nameof(value));
        }

        private MembershipPlan _plan;
        public MembershipPlan Plan
        {
            get => _plan;
            set => _plan = value ?? throw new ArgumentNullException(nameof(value));
        }

        public BoughtMembership(Member member, MembershipPlan plan, decimal discount, DateTime dateOfPurchase, int expires)
        {
            Member = member;
            Plan = plan;
            Discount = discount;
            DateOfPurchase = dateOfPurchase;
            Expires = expires;
        }

        public BoughtMembership(BoughtMembership other)
        {
            Member = other.Member;
            Plan = other.Plan;
            Discount = other.Discount;
            DateOfPurchase = other.DateOfPurchase;
            Expires = other.Expires;
        }

        public BoughtMembership()
        {
        }
    }
}
