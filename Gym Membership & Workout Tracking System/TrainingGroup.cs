namespace Gym_Membership___Workout_Tracking_System;

public class TrainingGroup : TrainingSession
{
    public int GroupSize { get; } = 15;

    private decimal _price;
    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0) throw new ArgumentException("Price cannot be negative");
            _price = value;
        }
    }
    
    public TrainingGroup(string name, DateTime date, DateTime startTime, DateTime endTime, decimal price, bool addToExtent = true)
        : base(name,
            date,
            startTime,
            endTime)
    {
        Price = price;
        if (addToExtent)
            AddTrainingSessionEXT(this);
    }
}