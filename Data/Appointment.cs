using System;

namespace StrubT.BFH.DotNet.DragDrop.Data {

	public class Appointment : IComparable<Appointment>, IComparable {

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

		public override string ToString() => $"{StartTime:g} - {EndTime:g}: {Name}";

		public override int GetHashCode() => (17 + Guid.GetHashCode()) * 31;

		public override bool Equals(object obj) => obj != null && GetType() == obj.GetType() && CompareTo((Appointment)obj) == 0;

		int IComparable.CompareTo(object obj) => CompareTo(obj as Appointment);

		public int CompareTo(Appointment other) {

			if (ReferenceEquals(other, null)) return -1;
			if (ReferenceEquals(this, other)) return 0;

			int cmp;
			if ((cmp = StartTime.CompareTo(other.StartTime)) != 0) return cmp;
			if ((cmp = EndTime.CompareTo(other.EndTime)) != 0) return cmp;
			if ((cmp = string.Compare(Name, other.Name, StringComparison.CurrentCulture)) != 0) return cmp;
			//if ((cmp = Guid.CompareTo(other.Guid)) != 0) return cmp;
			return 0;
		}

		public static bool operator ==(Appointment appointment1, Appointment appointment2) => !ReferenceEquals(appointment1, null) ? appointment1.CompareTo(appointment2) == 0 : ReferenceEquals(appointment2, null);

		public static bool operator !=(Appointment appointment1, Appointment appointment2) => !(appointment1 == appointment2);

		public static bool operator <(Appointment appointment1, Appointment appointment2) => !ReferenceEquals(appointment1, null) ? appointment1.CompareTo(appointment2) < 0 : !ReferenceEquals(appointment2, null);

		public static bool operator <=(Appointment appointment1, Appointment appointment2) => ReferenceEquals(appointment1, null) || appointment1.CompareTo(appointment2) <= 0;

		public static bool operator >(Appointment appointment1, Appointment appointment2) => !(appointment1 <= appointment2);

		public static bool operator >=(Appointment appointment1, Appointment appointment2) => !(appointment1 < appointment2);
	}

	public enum Priority {

		Low = Normal - 1,
		Normal = 0,
		High,
		Critical
	}
}
