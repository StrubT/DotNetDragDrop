using System;
using System.Collections.Generic;
using StrubT.BFH.DotNet.DragDrop.Data;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace StrubT.BFH.DotNet.DragDrop {

	public sealed partial class MainPage {

		public static string StorageFileName { get; } = "data.json";

		public static DependencyProperty AppointmentsProperty { get; } = DependencyProperty.Register(nameof(Appointments), typeof(ICollection<Appointment>), typeof(MainPage), new PropertyMetadata(new List<Appointment>()));

		public static DependencyProperty StoreProperty { get; } = DependencyProperty.Register(nameof(Store), typeof(Store), typeof(MainPage), new PropertyMetadata(new Store()));

		StorageFile StorageFile { get; set; }

		public Store Store {
			get { return (Store)GetValue(StoreProperty); }
			set { SetValue(StoreProperty, value); }
		}

		public ICollection<Appointment> Appointments => Store.Appointments;

		public MainPage() {

			InitializeComponent();
		}

		protected async override void OnNavigatedTo(NavigationEventArgs e) {
			base.OnNavigatedTo(e);

			StorageFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync(StorageFileName, CreationCollisionOption.OpenIfExists);
			Store = await Store.LoadAsync(StorageFile);
		}
	}
}
