using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using StrubT.BFH.DotNet.DragDrop.Data;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StrubT.BFH.DotNet.DragDrop {

	public sealed partial class AgendaWeek {

		ICollection<Appointment> Appointments { get; } = new List<Appointment>();

		public AgendaWeek() {

			InitializeData();

			InitializeComponent();
			InitializeAgenda();
		}

		void InitializeData() {

			var categoryBUAS = new Category() { Color = Colors.Blue, Name = "University" };
			var categoryGraded = new Category() { Color = Colors.Red, Name = "Exams" };
			var categoryPriv = new Category() { Color = Colors.Green, Name = "Private" };

			//Su
			var sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
			Appointments.Add(new Appointment() { Priority = Priority.Normal, Category = categoryPriv, Name = "Birthday Party", StartTime = sunday.AddHours(10), Duration = TimeSpan.FromMinutes(75) });

			//Tu
			var tuesday = sunday.AddDays(2);
			Appointments.Add(new Appointment() { Priority = Priority.High, Category = categoryGraded, Name = "AC++, HSM4, N.321", StartTime = tuesday.AddHours(10).AddMinutes(20), Duration = TimeSpan.FromMinutes(95) });
			Appointments.Add(new Appointment() { Priority = Priority.Normal, Category = categoryBUAS, Name = "ADMg, SSA1, M.106", StartTime = tuesday.AddHours(18).AddMinutes(10), Duration = TimeSpan.FromMinutes(95) });
			Appointments.Add(new Appointment() { Priority = Priority.Critical, Category = categoryBUAS, Name = "BaTh, Poster", StartTime = tuesday.AddHours(23), Duration = TimeSpan.Zero });

			//We
			var wednesday = sunday.AddDays(3);
			Appointments.Add(new Appointment() { Priority = Priority.High, Category = categoryBUAS, Name = "BaTh, HSM4 / LGM5, N.552", StartTime = wednesday.AddHours(10).AddMinutes(45), Duration = TimeSpan.FromMinutes(45) });
			Appointments.Add(new Appointment() { Priority = Priority.Normal, Category = categoryBUAS, Name = "SemW, BEO1, M.106", StartTime = wednesday.AddHours(19).AddMinutes(55), Duration = TimeSpan.FromMinutes(95) });

			//Th
			var thursday = sunday.AddDays(4);
			Appointments.Add(new Appointment() { Priority = Priority.High, Category = categoryGraded, Name = ".Net, PRM1, N.522", StartTime = thursday.AddHours(8).AddMinutes(20), Duration = TimeSpan.FromMinutes(95) });

			//Fr
			var friday = sunday.AddDays(5);
			Appointments.Add(new Appointment() { Priority = Priority.Low, Category = categoryBUAS, Name = "WBA3, JFR1 / GZR1 / STU1, N.422", StartTime = friday.AddHours(8).AddMinutes(20), Duration = TimeSpan.FromMinutes(215) });
			Appointments.Add(new Appointment() { Priority = Priority.Normal, Category = categoryBUAS, Name = "WBA3, JFR1 / GZR1 / STU1, N.422", StartTime = friday.AddHours(12).AddMinutes(45), Duration = TimeSpan.FromMinutes(205) });
			Appointments.Add(new Appointment() { Priority = Priority.Low, Category = categoryBUAS, Name = "WBA3, JFR1 / GZR1 / STU1, N.422", StartTime = friday.AddHours(16).AddMinutes(15), Duration = TimeSpan.FromMinutes(180) });

			//Sa
			var saturday = sunday.AddDays(6);
			Appointments.Add(new Appointment() { Priority = Priority.Normal, Category = categoryPriv, Name = "Swimming", StartTime = saturday.AddHours(9), Duration = TimeSpan.FromMinutes(120) });
			Appointments.Add(new Appointment() { Priority = Priority.Normal, Category = categoryPriv, Name = "Tutoring, cenga1", StartTime = saturday.AddHours(14), Duration = TimeSpan.FromMinutes(120) });
		}

		void InitializeAgenda() {

			var sunday = DateTime.Today.AddDays(((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)DateTime.Today.DayOfWeek + 7) % 7);

			var nofColumns = 7 + 1;
			var nofRows = 24 * 2 + 2;

			//set static element spans
			Grid.SetColumnSpan(Heading, nofColumns);
			Grid.SetColumnSpan(WeekBorder, nofColumns);
			Grid.SetRowSpan(WeekBorder, nofRows - 1);

			//create day columns, borders & headers
			for (var i = 0; i < 7; i++) {
				RootGrid.ColumnDefinitions.Add(new ColumnDefinition());

				var columnBorder = new Border();
				Grid.SetRow(columnBorder, 1);
				Grid.SetRowSpan(columnBorder, nofRows - 1);
				Grid.SetColumn(columnBorder, i + 1);
				RootGrid.Children.Add(columnBorder);

				var columnDate = sunday.AddDays(i);
				var columnHeader = new TextBlock() {
					Text = $"{columnDate:ddd}, {columnDate:d}"
				};
				Grid.SetRow(columnHeader, 1);
				Grid.SetColumn(columnHeader, i + 1);
				RootGrid.Children.Add(columnHeader);
			}

			//create rows and one header per hour
			for (var i = 0; i < 24 * 2; i++) {
				RootGrid.RowDefinitions.Add(new RowDefinition());

				var rowBorder = new Border();
				Grid.SetRow(rowBorder, i + 2);
				Grid.SetRowSpan(rowBorder, i % 2 == 0 ? 2 : 1);
				Grid.SetColumn(rowBorder, i % 2 == 0 ? 0 : 1);
				Grid.SetColumnSpan(rowBorder, nofColumns);
				RootGrid.Children.Add(rowBorder);

				if (i % 2 == 0) {
					var rowHeader = new TextBlock() {
						Text = DateTime.Today.Add(TimeSpan.FromHours((double)i / 2)).ToString("t")
					};
					Grid.SetRow(rowHeader, i + 2);
					Grid.SetRowSpan(rowHeader, 2);
					RootGrid.Children.Add(rowHeader);
				}
			}

			//create appointments
			foreach (var appointment in Appointments) {
				var element = new AgendaAppointment(appointment) {
					CanDrag = true
				};
				element.DragStarting += AgendaAppointment_DragStarting;
				RootGrid.Children.Add(element);
			}
		}

		void AgendaAppointment_DragStarting(UIElement sender, DragStartingEventArgs args) {
			
		}

		void RootGrid_DragOver(object sender, DragEventArgs e) {

		}
	}
}
