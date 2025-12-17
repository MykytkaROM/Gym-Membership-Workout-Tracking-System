using System;
using System.Collections.Generic;

namespace Gym_Membership___Workout_Tracking_System
{
    public class BoughtMembership
    {
        private static readonly List<BoughtMembership> _boughtMemberships = new List<BoughtMembership>();
        public static List<BoughtMembership> BoughtMemberships => new List<BoughtMembership>(_boughtMemberships);

        private decimal _discount;
        public decimal Discount
        {
            get => _discount;
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("Discount must be between 0 and 1.");
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
                    throw new ArgumentException("Expires must be positive.");
                _expires = value;
            }
        }

        private Member _member;
        public Member Member => _member;

        private MembershipPlan _plan;
        public MembershipPlan Plan => _plan;
        
        private MembershipPlan _catalogPlan;

        public void AddBoughtMembership(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            if (_member != null)
            {
                if (ReferenceEquals(_member, member)) return;
                throw new InvalidOperationException("BoughtMembership already linked to another Member.");
            }

            _member = member;

            if (!member.HasBoughtMembership(this))
                member.LinkBoughtMembership(this);
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

        public void AddBoughtMembership(MembershipPlan plan)
        {
            if (plan == null) throw new ArgumentNullException(nameof(plan));

            if (_plan != null)
            {
                if (ReferenceEquals(_plan, plan)) return;
                throw new InvalidOperationException("BoughtMembership already linked to another MembershipPlan.");
            }

            _plan = plan;

            if (!plan.HasBoughtMembership(this))
                plan.LinkBoughtMembership(this);
        }

        public void RemoveBoughtMembership(MembershipPlan plan)
        {
            if (plan == null) throw new ArgumentNullException(nameof(plan));

            if (!ReferenceEquals(_plan, plan))
                throw new InvalidOperationException("This BoughtMembership is not linked to this MembershipPlan.");

            if (!plan.HasBoughtMembership(this))
                throw new InvalidOperationException("Inconsistent state: Plan is not linked to this BoughtMembership.");

            plan.UnlinkBoughtMembership(this);
            _plan = null;
        }

        public BoughtMembership(Member member, MembershipPlan plan, decimal discount, DateTime dateOfPurchase, int expires)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            if (plan == null) throw new ArgumentNullException(nameof(plan));

            Discount = discount;
            DateOfPurchase = dateOfPurchase;
            Expires = expires;

            _catalogPlan = plan;

            var planSnapshot = new MembershipPlan(
                plan.Name,
                plan.DurationMonths,
                plan.Price,
                plan.DiscountRate,
                plan.Benefits,
                false
            );

            AddBoughtMembership(member);
            AddBoughtMembership(planSnapshot);

            _catalogPlan.LinkBoughtMembership(this);

            _boughtMemberships.Add(this);
        }

        public BoughtMembership(BoughtMembership other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (other.Member == null) throw new ArgumentNullException(nameof(other.Member));
            if (other.Plan == null) throw new ArgumentNullException(nameof(other.Plan));

            var planTemplate = new MembershipPlan(
                other.Plan.Name,
                other.Plan.DurationMonths,
                other.Plan.Price,
                other.Plan.DiscountRate,
                other.Plan.Benefits,
                false
            );

            Discount = other.Discount;
            DateOfPurchase = other.DateOfPurchase;
            Expires = other.Expires;

            AddBoughtMembership(other.Member);
            AddBoughtMembership(planTemplate);

            if (_boughtMemberships.Contains(this))
                throw new ArgumentException("BoughtMembership already exists in extent.");

            _boughtMemberships.Add(this);
        }

        public void Delete()
        {
            if (_member != null) RemoveBoughtMembership(_member);
            if (_plan != null) RemoveBoughtMembership(_plan);
            _boughtMemberships.Remove(this);
            
            if (_catalogPlan != null)
            {
                _catalogPlan.UnlinkBoughtMembership(this);
                _catalogPlan = null;
            }
        }
    }
}
