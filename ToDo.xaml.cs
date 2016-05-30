using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.DragDrop;
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
				args.Data.RequestedOperation = DataPackageOperation.Copy | DataPackageOperation.Move;
				args.Data.SetText(textBlock.Text);
				args.Data.Properties.ApplicationName = TitleBlock.Text;
				args.Data.Properties.Title = textBlock.Text;
				args.Data.Properties.Description = "An activity that can either be remebered to do or marked as done.";
			}
		}

		void TextBlock_DropCompleted(UIElement sender, DropCompletedEventArgs args) {

			var textBlock = sender as TextBlock;
			if (args.DropResult == DataPackageOperation.Move)
				((Panel)textBlock.Parent).Children.Remove(textBlock);
		}

		void StackPanel_DragEnter(object sender, DragEventArgs args) {

			var stackPanel = sender as StackPanel;
			string stackPanelLabel;

			if (stackPanel == ToDoPanel)
				stackPanelLabel = ToDoLabelBlock.Text;
			else if (stackPanel == DonePanel)
				stackPanelLabel = DoneLabelBlock.Text;
			else
				return;

			if (args.DataView.Contains(StandardDataFormats.Text)) {
				args.DragUIOverride.Caption = $"Add to '{stackPanelLabel}'";
				args.DragUIOverride.IsCaptionVisible = true;
				args.DragUIOverride.IsContentVisible = true;
				args.DragUIOverride.IsGlyphVisible = true;

				if (args.Modifiers.HasFlag(DragDropModifiers.Control))
					args.AcceptedOperation = DataPackageOperation.Copy;
				else if (args.Modifiers.HasFlag(DragDropModifiers.Shift) || args.DataView.RequestedOperation.HasFlag(DataPackageOperation.Move))
					args.AcceptedOperation = DataPackageOperation.Move;
				else
					args.AcceptedOperation = DataPackageOperation.Copy;
			}

			args.Handled = true;
		}

		async void StackPanel_Drop(object sender, DragEventArgs args) {

			var stackPanel = sender as StackPanel;
			if (stackPanel != null) {
				var deferral = args.GetDeferral();

				var textBlock = new TextBlock {
					Text = await args.DataView.GetTextAsync()
				};
				textBlock.DragStarting += TextBlock_DragStarting;
				textBlock.DropCompleted += TextBlock_DropCompleted;
				stackPanel.Children.Add(textBlock);

				deferral.Complete();
			}

			args.Handled = true;
		}
	}
}
