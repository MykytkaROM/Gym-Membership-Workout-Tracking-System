namespace Gym_Membership___Workout_Tracking_System;

public class BoughtMembershipDTO
{
    public decimal Discount { get; set; }
    public DateTime DateOfPurchase { get; set; }
    public int Expires { get; set; }
    public string PlanName { get; set; }
}
