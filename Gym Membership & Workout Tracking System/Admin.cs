using System.Text.Json;

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

    private User _user;
    public User User { get { return _user; } set 
        {
            if (value == null) throw new ArgumentNullException("User must be specified");
            _user = value;
        } 
    }

    public void AddUser(User user) 
    {
        if (user == null)
        {
            throw new ArgumentNullException("User cannot be null");
        }
        _user = user;
        if (user.Admin == null)
        {
            user.AddAdmin(this);
        }
         
    }

    public void RemoveUser(User user) 
    {
        if (_user == null) throw new ArgumentNullException("User should be added first");
        if (user == null)
        {
            throw new ArgumentNullException("User cannot be null");
        }
        if (!_user.Equals(user)) throw new ArgumentException("User specified is different from user in this admin");
        _user = null;
        if (user.Admin != null && user.Admin.Equals(this))
        {
            user.RemoveAdmin(this);
        }
        DeleteAdminEXT(this);
    }

    public Admin(int adminLevel, List<string> permissions, User user)
    {
        AdminLevel = adminLevel;
        Permissions = permissions;
        User = user;
        AddAdminEXT(this);
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
        User = other.User;
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
                dto.AdminLevel, dto.Permissions, dto.User
            );
        }
    }
    private static void AddAdminEXT(Admin admin) 
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
    public static void DeleteAdminEXT(Admin admin) 
    {
        if (admin == null)
        {
            throw new ArgumentNullException("Value must be specified");
        }
        if (!_admins.Contains(admin))
        {
            throw new ArgumentException("Value is not in the list");
        }
        _admins.Remove(admin);
    }
}