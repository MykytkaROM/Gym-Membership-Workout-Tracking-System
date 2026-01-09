namespace Gym_Membership___Workout_Tracking_System;

public class OnsiteSession
{
    public int RoomNumber { get; set; }
    public int FloorLevel { get; set; }

    public OnsiteSession(int roomNumber, int floorLevel)
    {
        RoomNumber = roomNumber;
        FloorLevel = floorLevel;
    }
}