using System;

namespace StrubT.BFH.DotNet.DragDrop {

	public interface IAgendaControl {

		TimeSpan DateRange { get; }

		DateTime StartDate { get; set; }

		DateTime EndDate { get; }
	}
}
