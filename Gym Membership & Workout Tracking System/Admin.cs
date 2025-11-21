namespace Gym_Membership___Workout_Tracking_System;

public class Admin
{
    public int AdminLevel { get; set; }

    private List<string> _permissions = new();

    public List<string> Permissions
    {
        get => new List<string>(_permissions);
        set
        {
            if (value == null || value.Count == 0)
                throw new ArgumentException("Permissions list must contain at least one value.");

            _permissions = new List<string>(value);
        }
    }

    public Admin(int adminLevel, List<string> permissions)
    {
        AdminLevel = adminLevel;
        Permissions = permissions;
    }

    public void ManagePermissions(string permission, bool add)
    {
        if (add)
        {
            if (!_permissions.Contains(permission))
                _permissions.Add(permission);
        }
        else
        {
            if (_permissions.Contains(permission))
                _permissions.Remove(permission);

            if (_permissions.Count == 0)
                throw new InvalidOperationException(
                    "Admin must have at least one permission.");
        }
    }
}