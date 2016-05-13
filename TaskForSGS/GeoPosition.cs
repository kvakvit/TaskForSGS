using System;
using System.Collections.Generic;
using Foundation;
using System.IO;
using Newtonsoft.Json;
using CoreLocation;


namespace TestTaskForSGS
{
	
	public class GeoPosition
	{	

		static string documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
		static string library = Path.Combine (documents, "..", "Library");
		static string filename = Path.Combine (library, "locations.json");
		
		public string Addres { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }



		public GeoPosition ()
		{
		}


		public static List<GeoPosition> fetchLocations() {

			var deserial = new List<GeoPosition>();

			if (File.Exists (filename)) {

				var text = File.ReadAllText (filename);
				if (JsonConvert.DeserializeObject<List<GeoPosition>> (text) != null) {
					deserial = JsonConvert.DeserializeObject<List<GeoPosition>> (text);
				}
			} 

			return deserial;

		}

		public static void saveLocations(GeoPosition currentLoc) {
		
			List<GeoPosition> pastLocations = fetchLocations ();
			pastLocations.Add (currentLoc);

			var json = JsonConvert.SerializeObject(pastLocations, Newtonsoft.Json.Formatting.Indented);

			File.WriteAllText(filename, json);
		}


		public static string formatAddressFromPlacemark(CLPlacemark placemark) {

			string fullAddres = "";

			NSArray addressArray = (placemark.AddressDictionary["FormattedAddressLines"]) as NSArray;

			for (nuint i = 0; i < addressArray.Count-1; i++) {

				fullAddres += addressArray.GetItem<NSString> (addressArray.Count - i - 1) + ", ";

			}
			fullAddres += addressArray.GetItem<NSString> (0);

			return fullAddres;
		}

		public static void deleteLocations(){
			File.WriteAllText (filename, "");
		}

	}
		
}

