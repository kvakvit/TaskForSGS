using System;
using UIKit;
using Foundation;
using CoreLocation;
using System.Collections.Generic;
using CoreGraphics;
using Newtonsoft.Json;
using System.IO;
using Contacts;

namespace TestTaskForSGS
{
	public class GeoTableViewController: UITableViewController
	{
		static readonly string cellId = "mapItemCellId";
		public List<GeoPosition> tableItems { get; set; }

		CLLocationManager locationManager = new CLLocationManager();

		string addres = "Местоположение недоступно";
		string coordinate = "...";
		bool flagSaved = false;

		UILabel curAddresLabel, curCoordLabel;
		UIButton clearBtn;
		UIView currentView;


		public GeoTableViewController ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			tableItems = new List<GeoPosition> ();
			tableItems = GeoPosition.fetchLocations();

			TableView.RowHeight = UITableView.AutomaticDimension;
			TableView.EstimatedRowHeight = 60;

			curAddresLabel = new UILabel ();
			curCoordLabel = new UILabel ();

			currentView = new UIView();
			var tap = new UITapGestureRecognizer (handleTapOnCurrentAddres);
			currentView.AddGestureRecognizer (tap);

			clearBtn = UIButton.FromType(UIButtonType.RoundedRect);
			clearBtn.TouchUpInside += ClearBtnHandleTouchUpInside;  

			if (tableItems.Count == 0) {
				clearBtn.Enabled = false;
			}
	

			locationManager.AuthorizationChanged += DidChangeAuthorizationStatus; 

			if (CLLocationManager.Status == CLAuthorizationStatus.NotDetermined) {

				locationManager.RequestWhenInUseAuthorization ();
			}
		}

		bool checkAutorizationStatus() {
			
			if ((CLLocationManager.Status == CLAuthorizationStatus.Denied) ||
				(CLLocationManager.Status == CLAuthorizationStatus.Restricted)) {

				addres = "Местоположение недоступно";
				coordinate = "Разрешите доступ к геолокации";
				return false;

			} else {
				return true;
			} 			
		}

		void DidChangeAuthorizationStatus(object sender, CLAuthorizationChangedEventArgs e){

			if (checkAutorizationStatus ()) {

				locationManager.LocationsUpdated += delegate(object senderLoc, CLLocationsUpdatedEventArgs eLoc) {

					CLGeocoder geoCoder = new CLGeocoder ();
					geoCoder.ReverseGeocodeLocation (eLoc.Locations[0], ReverseGeocodeLocationHandle);
				};

				locationManager.StartUpdatingLocation ();
			}

			UpdateTableHeader();
		}

		void ClearBtnHandleTouchUpInside(object sender, EventArgs ea) {

			var alertController = UIAlertController.Create("Вы уверены?", 
				"Все местоположения будут стерты!", 
				UIAlertControllerStyle.Alert);


			alertController.AddAction(UIAlertAction.Create("Да", 
				UIAlertActionStyle.Default, 
				alert => 	
				{
					clearBtn.Enabled = false;
					GeoPosition.deleteLocations ();
					tableItems = GeoPosition.fetchLocations();
					TableView.ReloadData();
				}));

			alertController.AddAction(UIAlertAction.Create("Отмена", 
				UIAlertActionStyle.Cancel, 
				null));
			
			PresentViewController(alertController, true, null);

		}
			

		void ReverseGeocodeLocationHandle(CLPlacemark[] placemarks, NSError error) {

			if (error != null) {
			
				addres = "Местоположение недоступно";
				coordinate = "Дождитесь наличия соединения";

			} else {

				if ((placemarks != null) && (placemarks.Length > 0)) {

					locationManager.StopUpdatingLocation ();

					var currentGeoPos = new GeoPosition () {

						Addres = GeoPosition.formatAddressFromPlacemark (placemarks [0]),
						Latitude = placemarks[0].Location.Coordinate.Latitude,
						Longitude = placemarks[0].Location.Coordinate.Longitude
					};
							
					string coord = currentGeoPos.Latitude.ToString () + "; " + currentGeoPos.Longitude.ToString ();

					addres = currentGeoPos.Addres;
					coordinate = coord;

					if (!flagSaved) {
						GeoPosition.saveLocations (currentGeoPos);
						flagSaved = true;
					}
				}
			}

			UpdateTableHeader(); 

		}

		void UpdateTableHeader() {

			curAddresLabel.Text = addres;
			curCoordLabel.Text = coordinate;
			
			TableView.TableHeaderView = TableHeaderView.CalculationSizeTableHeader (TableView, curAddresLabel, curCoordLabel, 
																					currentView, clearBtn);
		}
								
		void handleTapOnCurrentAddres(UITapGestureRecognizer tap) {
				
			if (locationManager.Location != null) {
				var latitude = locationManager.Location.Coordinate.Latitude;
				var longitude = locationManager.Location.Coordinate.Longitude;
				var mapForLocation = new MapViewController (latitude, longitude, "Now you are here!");
				NavigationController.PushViewController (mapForLocation, true);
			}

		}
			

		public override nint RowsInSection (UITableView tableView, nint section)
		{
			return tableItems.Count;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var latitude = tableItems [indexPath.Row].Latitude;
			var longitude = tableItems [indexPath.Row].Longitude;
			var mapForLocation = new MapViewController (latitude, longitude, "You were here");
			NavigationController.PushViewController (mapForLocation, true);
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (cellId);
			GeoPosition item = tableItems[indexPath.Row];

			if (cell == null)
			{ cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellId); }

			string coord = item.Latitude.ToString () + ", " + item.Longitude.ToString ();

			cell.TextLabel.Text = item.Addres;
			cell.TextLabel.Lines = 0;
			cell.DetailTextLabel.Text = coord;

			return cell;
		}
			

	}
}

