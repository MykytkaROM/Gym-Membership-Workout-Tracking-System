namespace Gym_Membership___Workout_Tracking_System;

public class OnlineSession
{
    private string _meetingLink;
    public string MeetingLink
    {
        get => _meetingLink;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("MeetingLink cannot be empty");
            _meetingLink = value;
        }
    }
    
    private string _platformName;
    public string PlatformName
    {
        get => _platformName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("PlatformName cannot be empty");
            _platformName = value;
        }
    }
    
    public OnlineSession(string meetingLink, string platformName)
    {
        MeetingLink = meetingLink;
        PlatformName = platformName;
    }
}