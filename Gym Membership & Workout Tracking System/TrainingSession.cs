using System.Text.Json;

namespace Gym_Membership___Workout_Tracking_System;

public abstract class TrainingSession
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

        public OnsiteSession OnsiteDetails { get; private set; }
        public OnlineSession OnlineDetails { get; private set; }

        protected TrainingSession(string name, DateTime date, DateTime startTime, DateTime endTime)
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
        
        public void SetOnsite(int roomNumber, int floorLevel)
        {
            if (OnsiteDetails != null) throw new InvalidOperationException("OnsiteSession is already set.");
            OnsiteDetails = new OnsiteSession(roomNumber, floorLevel);
        }

        public void SetOnline(string meetingLink, string platformName)
        {
            if (OnlineDetails != null) throw new InvalidOperationException("OnlineSession is already set.");
            OnlineDetails = new OnlineSession(meetingLink, platformName);
        }

        public void RemoveOnsite()
        {
            if (OnsiteDetails == null) throw new ArgumentNullException("OnsiteSession is not set.");
            OnsiteDetails = null;
        }

        public void RemoveOnline()
        {
            if (OnlineDetails == null) throw new ArgumentNullException("OnlineSession is not set.");
            OnlineDetails = null;
        }

        public void ValidateDeliveryMode()
        {
            if (OnsiteDetails == null && OnlineDetails == null)
                throw new InvalidOperationException("Session must be Onsite or Online (or both).");
        }

        private void ValidateTime()
        {
            if (_endTime != default && _startTime != default && _endTime <= _startTime)
                throw new ArgumentException("EndTime must be later than StartTime.");
        }

        public void manageTrainingSessions()
        {
            // placeholder from UML
        }
        
        private static readonly List<TrainingSession> _trainingSessions = new List<TrainingSession>();

        public static List<TrainingSession> TrainingSessions
        {
            get { return new List<TrainingSession>(_trainingSessions); }
        }

        protected static void AddTrainingSessionEXT(TrainingSession session)
        {
            if (session == null) throw new ArgumentNullException("Value must be specified");
            if (_trainingSessions.Contains(session)) throw new ArgumentException("Value is already in the list");
            _trainingSessions.Add(session);
        }
        
        public static void Save(string path = "TrainingSessions.json")
        {
            var dtoList = _trainingSessions.Select(s =>
            {
                var dto = new TrainingSessionDTO
                {
                    Name = s.Name,
                    Date = s.Date,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,

                    Onsite = s.OnsiteDetails == null ? null : new OnsiteSessionDTO
                    {
                        RoomNumber = s.OnsiteDetails.RoomNumber,
                        FloorLevel = s.OnsiteDetails.FloorLevel
                    },

                    Online = s.OnlineDetails == null ? null : new OnlineSessionDTO
                    {
                        MeetingLink = s.OnlineDetails.MeetingLink,
                        PlatformName = s.OnlineDetails.PlatformName
                    }
                };

                if (s is TrainingGroup g)
                {
                    dto.SessionType = "Group";
                    dto.Price = g.Price;
                    dto.GroupSize = g.GroupSize;
                }
                else if (s is PersonalTraining p)
                {
                    dto.SessionType = "Personal";
                    dto.PricePerHour = p.PricePerHour;
                }
                else
                {
                    throw new InvalidOperationException("Unknown TrainingSession subclass.");
                }

                return dto;
            }).ToList();

            string json = JsonSerializer.Serialize(dtoList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            Console.WriteLine("TrainingSessions saved to " + path);
        }

        public static void Load(string path = "TrainingSessions.json")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            _trainingSessions.Clear();

            string json = File.ReadAllText(path);
            var dtoList = JsonSerializer.Deserialize<List<TrainingSessionDTO>>(json)
                          ?? throw new ArgumentNullException("No data in JSON file");

            foreach (var dto in dtoList)
            {
                if (string.IsNullOrWhiteSpace(dto.SessionType))
                    throw new ArgumentNullException("SessionType is missing in JSON");

                TrainingSession session;

                if (dto.SessionType == "Group")
                {
                    if (dto.Price == null) throw new ArgumentNullException("Group session must have Price");
                    session = new TrainingGroup(dto.Name, dto.Date, dto.StartTime, dto.EndTime, dto.Price.Value, true);
                }
                else if (dto.SessionType == "Personal")
                {
                    if (dto.PricePerHour == null) throw new ArgumentNullException("Personal session must have PricePerHour");
                    session = new PersonalTraining(dto.Name, dto.Date, dto.StartTime, dto.EndTime, dto.PricePerHour.Value, true);
                }
                else
                {
                    throw new ArgumentException("Unknown SessionType: " + dto.SessionType);
                }

                if (dto.Onsite != null)
                    session.SetOnsite(dto.Onsite.RoomNumber, dto.Onsite.FloorLevel);

                if (dto.Online != null)
                    session.SetOnline(dto.Online.MeetingLink, dto.Online.PlatformName);
            }
        }
    }