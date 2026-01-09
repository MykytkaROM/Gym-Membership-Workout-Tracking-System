namespace Gym_Membership___Workout_Tracking_System;

public class TrainingSession
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Name cannot be empty");
            _name = value;
        }
    }
    public DateTime Date { get; set; }

    private DateTime _startTime;
    public DateTime StartTime
    {
        get => _startTime;
        set
        {
            _startTime = value;
            ValidateTime();
        }
    }

    private DateTime _endTime;
    public DateTime EndTime
    {
        get => _endTime;
        set
        {
            _endTime = value;
            ValidateTime();
        }
    }
    
    public TimeSpan Duration => EndTime - StartTime;
    
    public TrainingGroup GroupDetails { get; private set; }
    public PersonalTraining PersonalDetails { get; private set; }
    
    public OnsiteSession OnsiteDetails { get; private set; }
    public OnlineSession OnlineDetails { get; private set; } 
    
    public TrainingSession(string name, DateTime date, DateTime startTime, DateTime endTime)
    {
        Name = name;
        Date = date;
        _startTime = startTime;
        _endTime = endTime;

        ValidateTime();
    }
    
    public TimeSpan GetDuration()
    {
        return Duration;
    }
    
    private void ValidateTime()
    { 
        if (_endTime != default && _startTime != default && _endTime <= _startTime) 
            throw new ArgumentException("EndTime must be later than StartTime.");
    }
    
    public void ValidateSession()
    {
        bool hasKind = (GroupDetails != null) ^ (PersonalDetails != null);
        if (!hasKind)
            throw new InvalidOperationException("Session must be exactly one kind: Group OR Personal.");

        if (OnsiteDetails == null && OnlineDetails == null)
            throw new InvalidOperationException("Session must be Onsite or Online (or both).");
    }
    
    public void ValidateDeliveryMode()
    {
        if (OnsiteDetails == null && OnlineDetails == null)
            throw new InvalidOperationException("Session must be Onsite or Online (or both).");
    }
    
    public void manageTrainingSessions()
    {
        // placeholder method for Trainer?
    }
    
    
}