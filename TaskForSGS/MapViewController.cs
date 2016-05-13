using System;
using UIKit;
using MapKit;
using CoreLocation;

namespace TestTaskForSGS
{
	public class MapViewController: UIViewController
	{

		double latitude, longitude;
		string textForAnnotation;

		public MapViewController (double lati, double longi, string text)
		{
			latitude = lati;
			longitude = longi;
			textForAnnotation = text;
		}

		public override void ViewDidLoad ()
		{

			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.

			Title = "Your location on map";

			var map = new MKMapView (UIScreen.MainScreen.Bounds);
			map.MapType = MKMapType.Standard;

			View.AddSubview(map);

			var annotation = new MKPointAnnotation () {
				Title=textForAnnotation,
				Coordinate = new CLLocationCoordinate2D (latitude, longitude)
			};

			var mapRegion = MKCoordinateRegion.FromDistance (annotation.Coordinate, 5000, 5000);

			map.AddAnnotations (annotation);

			map.CenterCoordinate = annotation.Coordinate;
			map.Region = mapRegion;


		}

	}
}

