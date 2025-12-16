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
        public Member Member => _member;

        private MembershipPlan _plan;
        public MembershipPlan Plan => _plan;

        public void AddBoughtMembership(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            if (_member != null && !ReferenceEquals(_member, member))
                throw new InvalidOperationException("BoughtMembership already linked to another Member.");

            _member = member;

            if (!member.HasBoughtMembership(this))
                member.LinkBoughtMembership(this);

            if (_plan != null && !_plan.HasBoughtMembership(this))
                _plan.LinkBoughtMembership(this);
        }

        public void AddBoughtMembership(MembershipPlan plan)
        {
            if (plan == null) throw new ArgumentNullException(nameof(plan));

            if (_plan != null && !ReferenceEquals(_plan, plan))
                throw new InvalidOperationException("BoughtMembership already linked to another MembershipPlan.");

            _plan = plan;

            if (!plan.HasBoughtMembership(this))
                plan.LinkBoughtMembership(this);

            if (_member != null && !_member.HasBoughtMembership(this))
                _member.LinkBoughtMembership(this);
        }

        public void RemoveBoughtMembership(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            if (!ReferenceEquals(_member, member))
                throw new InvalidOperationException("This BoughtMembership is not linked to this Member.");

            if (!member.HasBoughtMembership(this))
                throw new InvalidOperationException("Inconsistent state: Member does not contain this BoughtMembership.");

            member.UnlinkBoughtMembership(this);
            _member = null;
        }

        public void RemoveBoughtMembership(MembershipPlan plan)
        {
            if (plan == null) throw new ArgumentNullException(nameof(plan));
            if (!ReferenceEquals(_plan, plan))
                throw new InvalidOperationException("This BoughtMembership is not linked to this MembershipPlan.");

            if (!plan.HasBoughtMembership(this))
                throw new InvalidOperationException("Inconsistent state: Plan does not contain this BoughtMembership.");

            plan.UnlinkBoughtMembership(this);
            _plan = null;
        }
        
        public BoughtMembership(Member member, MembershipPlan plan, decimal discount, DateTime dateOfPurchase, int expires)
        {

            Discount = discount;
            DateOfPurchase = dateOfPurchase;
            Expires = expires;

            AddBoughtMembership(member);
            AddBoughtMembership(plan);
        }
        
        public BoughtMembership(BoughtMembership other)
            : this(other.Member, other.Plan, other.Discount, other.DateOfPurchase, other.Expires)
        {
        }

        public void Delete()
        {
            if (_member != null) RemoveBoughtMembership(_member);
            if (_plan != null) RemoveBoughtMembership(_plan);
        }
    }
}
