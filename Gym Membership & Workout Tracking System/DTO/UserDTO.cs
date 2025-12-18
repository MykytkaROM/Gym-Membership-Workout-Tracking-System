using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_Membership___Workout_Tracking_System.DTO;
public class UserDTO
{
    public string name { get; set; }
    public string email { get; set; }
    public string phoneNumber { get; set; }

    
    public AddressDTO address { get; set; }
}

public class AddressDTO
{
    public string City { get; set; }
    public string Street { get; set; }
    public int Building { get; set; }
}
