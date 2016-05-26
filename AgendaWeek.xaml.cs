using System;
using System.Collections.Generic;
using System.Globalization;
using StrubT.BFH.DotNet.DragDrop.Data;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace StrubT.BFH.DotNet.DragDrop {

	public sealed partial class AgendaWeek : IAgendaControl {

		public static string StorageFileName { get; } = "data.json";

		public static DependencyProperty RowsPerHourProperty { get; } = DependencyProperty.Register(nameof(RowsPerHour), typeof(int), typeof(AgendaWeek), new PropertyMetadata(2));

		public static DependencyProperty StartDateProperty { get; } = DependencyProperty.Register(nameof(StartDate), typeof(DateTime), typeof(AgendaWeek), new PropertyMetadata(GetFirstDayOfWeek(DateTime.Today)));

		public static DependencyProperty StoreProperty { get; } = DependencyProperty.Register(nameof(Store), typeof(Store), typeof(MainPage), new PropertyMetadata(new Store()));

		public static DependencyProperty AppointmentsProperty { get; } = DependencyProperty.Register(nameof(Appointments), typeof(ICollection<Appointment>), typeof(MainPage), new PropertyMetadata(new List<Appointment>()));

		StorageFile StorageFile { get; set; }

		public Store Store {
			get { return (Store)GetValue(StoreProperty); }
			set { SetValue(StoreProperty, value); }
		}

		public int RowsPerHour {
			get { return (int)GetValue(RowsPerHourProperty); }
			set { SetValue(RowsPerHourProperty, value); }
		}

		TimeSpan IAgendaControl.DateRange => TimeSpan.FromDays(7);

		public DateTime StartDate {
			get { return (DateTime)GetValue(StartDateProperty); }
			set { SetValue(StartDateProperty, GetFirstDayOfWeek(value)); }
		}

		public DateTime EndDate => StartDate.AddDays(7);

		public ICollection<Appointment> Appointments => Store.Appointments;

		public AgendaWeek() {

			InitializeStore(); //data

			InitializeComponent(); //GUI
			InitializeAgenda();
		}

		static DateTime GetFirstDayOfWeek(DateTime date) => date.AddDays((date.DayOfWeek - CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7);

		async void InitializeStore() {

			StorageFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync(StorageFileName, CreationCollisionOption.OpenIfExists);
			Store = await Store.LoadAsync(StorageFile);
		}

		void InitializeAgenda() {

			var culture = CultureInfo.CurrentCulture;

			//create day columns, borders & headers
			for (var i = 0; i < 7; i++) {
				RootGrid.ColumnDefinitions.Add(new ColumnDefinition());

				var columnBorder = new Border() {
					BorderThickness = new Thickness(1.0),
					BorderBrush = new SolidColorBrush(Colors.White)
				};
				SetColumn(columnBorder, i + 1);
				SetRowSpan(columnBorder, 24 * RowsPerHour + 1);
				RootGrid.Children.Add(columnBorder);

				var columnHeader = new TextBlock() {
					Text = culture.DateTimeFormat.ShortestDayNames[((int)culture.DateTimeFormat.FirstDayOfWeek + i) % 7]
				};
				SetColumn(columnHeader, i + 1);
				RootGrid.Children.Add(columnHeader);
			}

			//create rows and one header per hour
			for (var i = 0; i < 24 * RowsPerHour; i++) {
				RootGrid.RowDefinitions.Add(new RowDefinition());

				var rowBorder = new Border() {
					BorderThickness = new Thickness(1.0),
					BorderBrush = new SolidColorBrush(Colors.White)
				};
				SetRow(rowBorder, i + 1);
				SetColumnSpan(rowBorder, 8);
				RootGrid.Children.Add(rowBorder);

				if (i % RowsPerHour == 0) {
					var rowHeader = new TextBlock() {
						Text = DateTime.Today.Add(TimeSpan.FromHours((double)i / RowsPerHour)).ToString("t")
					};
					SetRow(rowHeader, i + 1);
					SetRowSpan(rowHeader, RowsPerHour);
					RootGrid.Children.Add(rowHeader);
				}
			}
		}


		//void TextBlock_DragStarting(UIElement sender, DragStartingEventArgs args) {

		//	args.Data.RequestedOperation = DataPackageOperation.Move;
		//	args.Data.SetApplicationLink(new Uri("xaml-elment://"));
		//}

		//void Grid_DragOver(object sender, DragEventArgs e) {

		//	if (e.DataView.Contains(StandardDataFormats.ApplicationLink))
		//		e.AcceptedOperation = DataPackageOperation.Move;
		//}

		//void Grid_Drop(object sender, DragEventArgs e) {

		//}

		//<Grid Grid.Row="1" AllowDrop="True" DragOver="Grid_DragOver" Drop="Grid_Drop">
		//	<Grid.ColumnDefinitions>
		//		<ColumnDefinition />
		//		<ColumnDefinition />
		//		<ColumnDefinition />
		//	</Grid.ColumnDefinitions>
		//	<Grid.RowDefinitions>
		//		<RowDefinition />
		//		<RowDefinition />
		//		<RowDefinition />
		//	</Grid.RowDefinitions>
		//	<StackPanel Grid.Row="1">
		//		<TextBlock Text="asdf" CanDrag="True" DragStarting="TextBlock_DragStarting" />
		//		<TextBlock Text="temp" CanDrag="True" DragStarting="TextBlock_DragStarting" />
		//	</StackPanel>
		//	<StackPanel Grid.Row="1" Grid.Column="2">
		//		<TextBlock Text="qwertz" CanDrag="True" DragStarting="TextBlock_DragStarting" />
		//	</StackPanel>
		//</Grid>
	}
}
