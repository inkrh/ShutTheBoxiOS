using UIKit;
using System;
using System.Collections.Generic;

namespace ShutTheBox
{
	public class ScoresTableSource : UITableViewSource
	{
		Dictionary<int, string> tableItems = new Dictionary<int, string>();
		string cellIdentifier = "TableCell";

		public ScoresTableSource(Dictionary<int, string> tableSource) {
			//tableItems = tableSource;
			int i = 1;
			int oi = tableSource.Count;
			foreach (var item in tableSource) {
				if (!tableItems.ContainsKey(i)) {
					tableItems.Add (i, tableSource [oi]);
				} else {
					tableItems [i] = tableSource [oi];
				}
				i++;
				oi--;
			}
		}

		public override nint RowsInSection(UITableView tableView, nint section) {
			return tableItems.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath) {
			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
			// if there are no cells to reuse, create a new one

			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
			cell.TextLabel.Lines = 4;
			cell.TextLabel.TextColor = UIColor.White;
			cell.TintColor = UIColor.Clear;
			cell.BackgroundColor = UIColor.Clear;
			cell.TextLabel.Font = UIFont.FromName ("appleberry", 14);

			//		cell.TextLabel.PreferredMaxLayoutWidth = 300;
			cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
			cell.TextLabel.AdjustsFontSizeToFitWidth = true;
			cell.TextLabel.Text = tableItems[indexPath.Row+1];
			return cell;
		}
	}
}

