using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StrubT.BFH.DotNet.DragDrop {

	public sealed partial class ToDo {

		public ToDo() {

			InitializeComponent();
		}

		void TextBlock_DragStarting(UIElement sender, DragStartingEventArgs args) {

			var textBlock = sender as TextBlock;
			if (textBlock != null) {
				((Panel)textBlock.Parent).Children.Remove(textBlock);
				args.Data.RequestedOperation = DataPackageOperation.Move;
				args.Data.SetText(textBlock.Text);
			}
		}

		void StackPanel_DragEnter(object sender, DragEventArgs args) {

			if (args.DataView.Contains(StandardDataFormats.Text))
				args.AcceptedOperation = DataPackageOperation.Copy | DataPackageOperation.Move;
			args.Handled = true;
		}

		async void StackPanel_Drop(object sender, DragEventArgs args) {

			var stackPanel = sender as StackPanel;
			if (stackPanel != null) {
				var textBlock = new TextBlock {
					Text = await args.DataView.GetTextAsync()
				};
				textBlock.DragStarting += TextBlock_DragStarting;
				stackPanel.Children.Add(textBlock);
			}
			args.Handled = true;
		}
	}
}
