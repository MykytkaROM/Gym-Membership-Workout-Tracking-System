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
        }
    }
}
