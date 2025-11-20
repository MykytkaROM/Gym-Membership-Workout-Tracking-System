namespace Gym_Membership___Workout_Tracking_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MembershipPlan.load();
            List < MembershipPlan > membershipPlans = MembershipPlan.MembershipPlans;
            membershipPlans.Add(new MembershipPlan("Gold", 2 , 179.99m,null, "Health"));
            foreach (var membershipPlan in membershipPlans) {
                Console.WriteLine(membershipPlan.Name);
            }
        }
    }
}
