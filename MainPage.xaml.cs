using Windows.UI.Xaml;

namespace StrubT.BFH.DotNet.DragDrop {

	public sealed partial class MainPage {

		public MainPage() {

			InitializeComponent();
		}

		void NavigateTo_ToDo(object sender, RoutedEventArgs e) {

			Frame.Navigate(typeof(ToDo));
		}

		void NavigateTo_Agenda(object sender, RoutedEventArgs e) {

			Frame.Navigate(typeof(AgendaWeek));
		}
	}
}
