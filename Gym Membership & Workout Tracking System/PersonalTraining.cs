namespace Gym_Membership___Workout_Tracking_System;

public class PersonalTraining : TrainingSession
{
    private decimal _pricePerHour;
    public decimal PricePerHour
    {
        get => _pricePerHour;
        set
        {
            if (value < 0) throw new ArgumentException("PricePerHour cannot be negative");
            _pricePerHour = value;
        }
    }
    
    public PersonalTraining(string name, DateTime date, DateTime startTime, DateTime endTime, decimal pricePerHour)
        : base(name,
            date,
            startTime,
            endTime)
    {
        PricePerHour = pricePerHour;
    }
    
    public decimal CalculateTotalPrice()
    {
        return (decimal)GetDuration().TotalHours * PricePerHour;
    }
}