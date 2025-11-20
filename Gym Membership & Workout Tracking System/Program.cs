namespace Gym_Membership___Workout_Tracking_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MembershipPlan.load();
            List < MembershipPlan > membershipPlans = MembershipPlan.MembershipPlans;
            foreach (var membershipPlan in membershipPlans) {
                Console.WriteLine(membershipPlan.Name);
            }
        }
    }
}
