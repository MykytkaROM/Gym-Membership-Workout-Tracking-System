using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gym_Membership___Workout_Tracking_System
{
    public class Trainer
    {
        private int _trainerID;   //trainerID : int
        public int TrainerID { get => _trainerID; set 
            {
                if (value < 0)
                {
                    throw new ArgumentException("ID cannot be negative");
                }

                _trainerID = value;
            } 
        }
        private string _specialization; //specialization : string
        public string Specialization { get => _specialization; set 
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Specialization can't be empty or null");
                }
                _specialization = value;
            } 
        }
        private DateTime _hireDate;//hireDate : DateTime
        public DateTime HireDate => _hireDate;

        private decimal _baseSalary;//baseSalary : decimal
        public decimal BaseSalary { get => _baseSalary; set 
            {
                if (value < 0)
                {
                    throw new ArgumentException("Base salary can't be negative");
                }
                _baseSalary = value;
            } 
        }
        private int _yearsOfExperience;//yearsOfExperience : int
        public int YearOfExperience { get => _yearsOfExperience; set 
            {
                if (value < 0)
                {
                    throw new ArgumentException("ID cannot be negative");
                }
                _yearsOfExperience = value;
            } 
        }
        public decimal CurrentSalary { get { return _baseSalary * (1 + _yearsOfExperience); } }

        private static List<Trainer> _trainers = new List<Trainer>();
        public static List<Trainer> Trainers
        {
            get
            {
                List<Trainer> copy = new List<Trainer>(_trainers.Count);

                _trainers.ForEach((item) =>
                {
                    copy.Add(new Trainer(item));
                });
                return copy;
            }
        }
        private static void AddTrainerEXT(Trainer trainer) 
        {
            if (trainer == null)
            {
                throw new ArgumentNullException("Value must be specified");
            }
            if (_trainers.Contains(trainer))
            {
                throw new ArgumentException("Value is already in the list");
            }
            if (_trainers.Any(m => m.TrainerID == trainer.TrainerID))
                throw new ArgumentException("Duplicate trainer ID.");

            _trainers.Add(trainer);
        }
        public Trainer(Trainer trainer) 
        {
            TrainerID = trainer.TrainerID;
            Specialization = trainer.Specialization;
            _hireDate = trainer.HireDate;
            BaseSalary = trainer.BaseSalary;
            YearOfExperience = trainer.YearOfExperience;
            _mentor = trainer.Mentor;
            _trainees = trainer.Trainees;
        }
        public Trainer(int trainerID, string specialization, DateTime hireDate, decimal baseSalary, int yearOfExpirience) 
        {
            TrainerID= trainerID;
            Specialization = specialization;
            _hireDate = hireDate;
            BaseSalary = baseSalary;
            YearOfExperience= yearOfExpirience;
            AddTrainerEXT(this);
        }
        public static void save(string path = "Trainers.json")
        {
            var dtoList = _trainers
                .Select(m => new TrainersDTO
                {
                   TrainerID = m.TrainerID,
                    Specialization = m.Specialization,
                    HireDate = m.HireDate,
                    BaseSalary = m.BaseSalary,
                    YearOfExpirience = m.YearOfExperience
                })
                .ToList();

            string json = JsonSerializer.Serialize(dtoList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            Console.WriteLine("Trainers saved to " + path);
        }

        public static void load(string path = "Trainers.json")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            _trainers.Clear();

            string json = File.ReadAllText(path);

            var dtoList = JsonSerializer.Deserialize<List<TrainersDTO>>(json)
                          ?? throw new ArgumentNullException("No data in JSON file");

            foreach (var dto in dtoList)
            {
                new Trainer(
                    dto.TrainerID,
                    dto.Specialization,
                    dto.HireDate,
                    dto.BaseSalary,
                    dto.YearOfExpirience

                );
            }
        }

        private Trainer? _mentor;
        public Trainer Mentor => _mentor;

        private List<Trainer> _trainees;
        public List<Trainer> Trainees
        {
            get
            {
                List<Trainer> copy = new List<Trainer>(_trainees.Count);

                _trainees.ForEach((item) =>
                {
                    copy.Add(new Trainer(item));
                });
                return copy;
            }
        }

        public void AddMentor(Trainer mentor) 
        {
            if (_mentor != null) throw new InvalidOperationException("Mentor is already added. If you meant to modify it use EditMentor() instead");
            if (mentor == null) throw new ArgumentNullException("Mentor should be not null");
            if (mentor.Equals(this)) throw new ArgumentException("Trainer cannot mentor himself");
            _mentor = mentor;
            if (!mentor.Trainees.Contains(this)) 
            {
                mentor.AddTrainee(this);
            }
        }
        public void EditMentor(Trainer newMentor) 
        {
            if (newMentor == null) throw new ArgumentNullException("Mentor should be not null");
            if (newMentor.Equals(this)) throw new ArgumentException("Trainer cannot mentor himself");
            this.DeleteMentor(this.Mentor);
            this.AddMentor(newMentor);
        }
        public void DeleteMentor(Trainer mentor) 
        {
            if (_mentor == null) throw new ArgumentNullException("Mentor should be specified to delete it");
            if (mentor == null) throw new ArgumentNullException("Mentor should be not null");
            if (!_mentor.Equals(mentor)) throw new ArgumentException("Mentor specified is different from mentor in this trainee");
            _mentor = null;
            if (mentor.Trainees.Contains(this)) 
            {
                mentor.DeleteTrainee(this);
            }
            
        }
        public void AddTrainee(Trainer trainee) 
        {
            if (trainee == null) throw new ArgumentNullException("Trainee cannot be null");
            if (_trainees.Contains(trainee)) throw new ArgumentException("This trainee is already in the list");
            if (this.Equals(trainee)) throw new ArgumentException("Trainer cannot mentor himself");
            _trainees.Add(trainee);
            if (trainee.Mentor == null) 
            {
                trainee.AddMentor(this);
            }
            
        }
        public void EditTrainee(Trainer oldTrainee ,Trainer newTrainee) 
        {
            if (_trainees == null) throw new ArgumentNullException("List is empty");
            if (newTrainee == null) throw new ArgumentNullException("New trainee cannot be null");
            if(oldTrainee == null) throw new ArgumentNullException("Old trainee cannot be null");
            if (this.Equals(newTrainee) && this.Equals(oldTrainee)) throw new ArgumentException("Trainer cannot mentor himself");
            if (oldTrainee.Equals(newTrainee)) throw new ArgumentException("Trainee to edit cannot be equal to new trainee");
            this.DeleteTrainee(oldTrainee);
            this.AddTrainee(newTrainee);
        }
        public void DeleteTrainee(Trainer trainee) 
        {
            if (_trainees == null) throw new ArgumentNullException("List is empty");
            if (trainee == null) throw new ArgumentNullException("Trainee cannot be null");
            if (trainee.Mentor != this) throw new ArgumentException("Trainee have different mentor specified");
            _trainees.Remove(trainee);
            if (trainee.Mentor != null) 
            {
                trainee.DeleteMentor(this);
            }
        }
    }
}
