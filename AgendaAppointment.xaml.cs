using StrubT.BFH.DotNet.DragDrop.Data;
using Windows.UI.Xaml;

namespace StrubT.BFH.DotNet.DragDrop {

	public sealed partial class AgendaAppointment {

		public Appointment Appointment { get; }

		public AgendaAppointment(Appointment appointment) {

			Appointment = appointment;

			InitializeComponent();
		}
	}
}
