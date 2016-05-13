using System;
using UIKit;
using CoreGraphics;

namespace TestTaskForSGS
{
	public class TableHeaderView: UIView
	{
		public TableHeaderView ()
		{
		}

		public static UIView CalculationSizeTableHeader(UITableView tableView, UILabel curAddresLabel, UILabel curCoordLabel, 
												UIView currentView,	UIButton clearBtn) {

			//HEADER VIEW
			var headerView = new UIView ();
			headerView.Frame = new CGRect (0, 
				0, 
				tableView.Frame.Width, 
				tableView.Frame.Height / 10);

			//CURRENT ADDRES VIEW
			currentView.Frame = new CGRect (0, 
				0, 
				headerView.Frame.Width, 
				headerView.Frame.Height);


			//CURRENT LABEL
			var curLabel = new UILabel ();
			curLabel.Text = "Текущее:";
			curLabel.Font = UIFont.FromName("HelveticaNeue", 25);
			curLabel.SizeToFit ();
			curLabel.Frame = new CGRect (currentView.Frame.Width / 2 - curLabel.Frame.Width/2,  
				8, 
				curLabel.Frame.Width, 
				curLabel.Frame.Height);

			currentView.AddSubview (curLabel);


			//CURRENT ADDRES LABEL
			curAddresLabel.Frame = new CGRect (currentView.Frame.Width / 2 - curAddresLabel.Frame.Width/2,  
				curLabel.Frame.Y + curLabel.Frame.Height + 8, 
				tableView.Frame.Width-16, 
				tableView.Frame.Height);
			curAddresLabel.TextAlignment = UITextAlignment.Center;
			curAddresLabel.LineBreakMode = UILineBreakMode.WordWrap;
			curAddresLabel.TextColor = UIColor.FromRGBA (0, 122, 255, 255);
			curAddresLabel.Lines = 0;
			curAddresLabel.SizeToFit ();
			curAddresLabel.Frame = new CGRect (currentView.Frame.Width / 2 - curAddresLabel.Frame.Width/2,  
				curLabel.Frame.Y + curLabel.Frame.Height + 8, 
				curAddresLabel.Frame.Width, 
				curAddresLabel.Frame.Height);

			currentView.AddSubview (curAddresLabel);


			//CURRENT COORDINATE LABEL
			curCoordLabel.Frame = new CGRect (currentView.Frame.Width / 2 - curCoordLabel.Frame.Width/2,  
				curAddresLabel.Frame.Y + curAddresLabel.Frame.Height + 8, 
				tableView.Frame.Width-16, 
				tableView.Frame.Height);
			curCoordLabel.TextAlignment = UITextAlignment.Center;
			curCoordLabel.LineBreakMode = UILineBreakMode.WordWrap;
			curCoordLabel.TextColor = UIColor.FromRGBA (0, 122, 255, 255);
			curCoordLabel.Lines = 0;
			curCoordLabel.SizeToFit ();
			curCoordLabel.Frame = new CGRect (currentView.Frame.Width / 2 - curCoordLabel.Frame.Width/2,  
				curAddresLabel.Frame.Y + curAddresLabel.Frame.Height + 8, 
				curCoordLabel.Frame.Width, 
				curCoordLabel.Frame.Height);

			currentView.AddSubview (curCoordLabel);

			//CURRENT VIEW FRAME
			currentView.Frame = new CGRect (0, 
				0, 
				headerView.Frame.Width, 
				curLabel.Frame.Height + curAddresLabel.Frame.Height + curCoordLabel.Frame.Height + 24);


			headerView.AddSubview (currentView);


			//PAST LABEL
			var pastLabel = new UILabel ();
			pastLabel.Text = "Прошлые:";
			pastLabel.Font = UIFont.FromName("HelveticaNeue", 25);
			pastLabel.SizeToFit ();
			pastLabel.Frame = new CGRect (12,  
				curCoordLabel.Frame.Y + curCoordLabel.Frame.Height + 8, 
				pastLabel.Frame.Width, 
				pastLabel.Frame.Height);

			headerView.AddSubview (pastLabel);


			//CLEAR BUTTON
			clearBtn.SetTitle("Очистить", UIControlState.Normal);
			clearBtn.Font = UIFont.FromName("HelveticaNeue", 25);
			clearBtn.SizeToFit ();
			clearBtn.Frame = new CGRect (headerView.Frame.Width-clearBtn.Frame.Width-8,  
				pastLabel.Frame.Y, 
				clearBtn.Frame.Width, 
				pastLabel.Frame.Height);


			headerView.AddSubview (clearBtn);


			//HEADER FRAME
			headerView.Frame = new CGRect (0, 
				0, 
				tableView.Frame.Width, 
				currentView.Frame.Height + pastLabel.Frame.Height + 16);

			//SEPARATE 
			var separateImage = new UIImage ("imgSeparate.png");
			var separateView = new UIImageView (separateImage);

			separateView.Frame = new CGRect (headerView.Frame.X+8, 
				pastLabel.Frame.Y-4, 
				headerView.Frame.Width-16, 
				1);

			headerView.AddSubview (separateView);


			return headerView;

		}

	}
}

