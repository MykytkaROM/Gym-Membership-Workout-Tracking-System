using System.Text.Json;
using Gym_Membership___Workout_Tracking_System.DTO;

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
        AddAdmins(this);
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
    private static List<Admin> _admins = new List<Admin>();
    public static List<Admin> Admins { get 
        {
            List<Admin> copy = new List<Admin>(_admins.Count);

            _admins.ForEach((item) =>
            {
                copy.Add(new Admin(item));
            });
            return copy;
        } }
    public Admin(Admin other) 
    {
        AdminLevel = other.AdminLevel;
        Permissions = other.Permissions;

    }
    public static void save(string path = "admins.json")
    {
        var dtoList = _admins
            .Select(m => new AdminDTO
            {
                AdminLevel = m.AdminLevel,
                Permissions = m.Permissions
            })
            .ToList();

        string json = JsonSerializer.Serialize(dtoList, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        Console.WriteLine("Admins saved to " + path);
    }

    public static void load(string path = "admins.json")
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        _admins.Clear();

        string json = File.ReadAllText(path);

        var dtoList = JsonSerializer.Deserialize<List<AdminDTO>>(json)
                      ?? throw new ArgumentNullException("No data in JSON file");

        foreach (var dto in dtoList)
        {
            new Admin(
                dto.AdminLevel, dto.Permissions
            );
        }
    }
    private static void AddAdmins(Admin admin) 
    {
        if (_admins.Contains(admin))
        {
            throw new ArgumentException("Value is already in the list");
        }
        if (admin == null)
        {
            throw new ArgumentNullException("Value must be specified");
        }
        _admins.Add(admin);
    }
}