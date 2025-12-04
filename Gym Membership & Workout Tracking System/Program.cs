namespace Gym_Membership___Workout_Tracking_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //LOAD
            /*User.load();
            var list = User.Users;
            foreach (var user in list)
            {
                Console.WriteLine(user.Name);
            }*/

            //SAVE
            /*User user = 
                new User("Michael","michael@gmail.com","12345678",
                new Address("Warsaw","Koszykowa",10));
            User.save();*/
            
            //ADMIN
            /*var admin = new Admin(
                adminLevel: 5,
                permissions: new List<string> { "Read", "Write" }
            );
            
            foreach (var p in admin.Permissions)
                Console.WriteLine("- " + p);*/
            
            /*admin.ManagePermissions("Delete", add: true);
            
            Console.WriteLine("\n...");
            foreach (var p in admin.Permissions)
                Console.WriteLine("- " + p);*/
            
            /*admin.ManagePermissions("Write", add: false);
            
            Console.WriteLine("\n...");
            foreach (var p in admin.Permissions)
                Console.WriteLine("- " + p);*/
            
            //EntryRecord
            /*var start = new DateTime(2025, 11, 25, 10, 15, 0);
            var end   = new DateTime(2025, 11, 25, 11, 45, 0);
            
            var entry = new EntryRecord(start, end);

            Console.WriteLine($"Start time: {entry.StartTime}");
            Console.WriteLine($"End time:   {entry.EndTime}");
            Console.WriteLine($"Duration:   {entry.Duration}");*/
            
            
            // TestBoughtMembershipBag();
        }
        
        
        // static void TestBoughtMembershipBag()
        // {
        //     var plan = new MembershipPlan("Basic", 3, 100m, 0.2m, "Gym access");
        //
        //     var member = new Member();
        //     member.MemberID = 1;
        //     member.MembershipType = "Basic";
        //     member.TotalPoints = 0;
        //
        //     var bm1 = new BoughtMembership(member, plan, 0.2m, DateTime.Now, 30);
        //     member.AddBoughtMembership(bm1);
        //
        //     var bm2 = new BoughtMembership(member, plan, 0.2m, DateTime.Now.AddDays(30), 30);
        //     member.AddBoughtMembership(bm2);
        //
        //     Console.WriteLine("Bought memberships count: " + member.BoughtMemberships.Count);
        //     foreach (var b in member.BoughtMemberships)
        //     {
        //         Console.WriteLine($"{b.DateOfPurchase:yyyy-MM-dd} {b.Plan.Name} discount={b.Discount} expiresIn={b.Expires}");
        //     }
        //
        //     try
        //     {
        //         member.AddBoughtMembership(null);
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine("Null test: " + ex.GetType().Name);
        //     }
        //
        //     var another = new Member();
        //     another.MemberID = 2;
        //     another.MembershipType = "Basic";
        //     another.TotalPoints = 0;
        //
        //     var wrong = new BoughtMembership(another, plan, 0.1m, DateTime.Now, 30);
        //
        //     try
        //     {
        //         member.AddBoughtMembership(wrong);
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine("Wrong member test: " + ex.GetType().Name);
        //     }
        // }
    }
}
