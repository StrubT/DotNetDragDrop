using System;

namespace StrubT.BFH.DotNet.DragDrop.Data {

	public class Appointment {

		TimeSpan duration;

		public Guid Guid { get; }

		public Priority Priority { get; set; }

		public Category Category { get; set; }

		public string Name { get; set; }

		public DateTime StartTime { get; set; }

		public TimeSpan Duration {
			get { return duration; }
			set {
				if (duration < TimeSpan.Zero)
					throw new ArgumentOutOfRangeException(nameof(Duration), "An appointment duration cannot be negative.");
				duration = value;
			}
		}

		public DateTime EndTime {
			get { return StartTime + Duration; }
			set { Duration = value - StartTime; }
		}

		Appointment(Guid guid) {

			Guid = guid;
		}

		public Appointment() : this(Guid.NewGuid()) { }

		public Appointment(Guid guid, Priority priority, Category category, string name, DateTime startTime, DateTime endTime) : this(guid, priority, category, name, startTime, endTime - startTime) { }

		public Appointment(Guid guid, Priority priority, Category category, string name, DateTime startTime, TimeSpan duration) : this(guid) {

			if (category == null)
				throw new ArgumentNullException(nameof(category));
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			Priority = priority;
			Category = category;
			Name = name;
			StartTime = startTime;
			Duration = duration;
		}
	}

	public enum Priority {

		Low = Normal - 1,
		Normal = 0,
		High,
		Critical
	}
}
