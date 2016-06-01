using System;
using System.Globalization;
using StrubT.BFH.DotNet.DragDrop.Data;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace StrubT.BFH.DotNet.DragDrop {

	public abstract class ValueConverterBase<S, T> : IValueConverter {

		public abstract T Convert(S value, object parameter, string language);

		public virtual S ConvertBack(T value, object parameter, string language) {
			throw new NotImplementedException();
		}

		object IValueConverter.Convert(object value, Type targetType, object parameter, string language) {

			if (value != null && value.GetType() != typeof(S)) throw new ArgumentException("Passed value of wrong source type.", nameof(value));
			if (targetType != typeof(T)) throw new ArgumentException("Specified wrong target type.", nameof(targetType));

			return Convert((S)value, parameter, language);
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language) {

			if (value != null && value.GetType() != typeof(T)) throw new ArgumentException("Passed value of wrong source type.", nameof(value));
			if (targetType != typeof(S)) throw new ArgumentException("Specified wrong target type.", nameof(targetType));

			return ConvertBack((T)value, parameter, language);
		}
	}

	public class DateTimeWeekGridColumnConverter : ValueConverterBase<DateTime, int> {

		public override int Convert(DateTime value, object parameter, string language) => ((int)value.DayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7 + 1;
	}

	public class DateTimeWeekGridRowConverter : ValueConverterBase<DateTime, int> {

		public override int Convert(DateTime value, object parameter, string language) => (int)(value.TimeOfDay.TotalHours * 2) + 2;
	}

	public class DateTimeWeekGridRowSpanConverter : ValueConverterBase<TimeSpan, int> {

		public override int Convert(TimeSpan value, object parameter, string language) => value.TotalHours > 0.5 ? -(int)(-value.TotalHours * 2) : 1;
	}

	public class ColorBrushConverter : ValueConverterBase<Color, Brush> {

		public override Brush Convert(Color value, object parameter, string language) => new SolidColorBrush(value);
	}

	public class AppointmentTimeConverter : ValueConverterBase<Appointment, string> {

		public override string Convert(Appointment value, object parameter, string language) => value.StartTime.Date != value.EndTime.Date ? $"{value.StartTime:g} - {value.EndTime:g}" : $"{value.StartTime:g} - {value.EndTime:t}";
	}

	public class PriorityVisibilityConverter : ValueConverterBase<Priority, Visibility> {

		public override Visibility Convert(Priority value, object parameter, string language) => value >= Priority.High ? Visibility.Visible : Visibility.Collapsed;
	}

	public class PrioritySymbolConverter : ValueConverterBase<Priority, Symbol> {

		public Symbol DefaultSymbol { get; set; }

		public override Symbol Convert(Priority value, object parameter, string language) {

			switch (value) {
				//case Priority.Low: return Symbol.MapPin;
				//case Priority.Normal:
				case Priority.High: return Symbol.Important;
				case Priority.Critical: return Symbol.ReportHacked;
				default: return DefaultSymbol;
			}
		}
	}

	public class PriorityBrushConverter : ValueConverterBase<Priority, Brush> {

		public Brush DefaultBrush { get; set; }

		public override Brush Convert(Priority value, object parameter, string language) => value == Priority.Critical ? new SolidColorBrush(Colors.Red) : DefaultBrush;
	}
}
