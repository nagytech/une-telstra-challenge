using System;
using CoreLocation;
using UIKit;

using Foundation;
using telstrachallenge2016.iOS;
using telstrachallenge2016.interfaces;

// Dependency handles
[assembly: Xamarin.Forms.Dependency(typeof(LocationUpdateService))]
[assembly: Xamarin.Forms.Dependency(typeof(GpsLocation))]
[assembly: Xamarin.Forms.Dependency(typeof(LocationUpdatedEventArgs))]
[assembly: Xamarin.Forms.Dependency(typeof(Coordinate2D))]

namespace telstrachallenge2016.iOS
{
	/// <summary>
	/// Location update service.
	/// </summary>
	public class LocationUpdateService : ILocationUpdateService
	{
		/// <summary>
		/// The Location Manager.
		/// </summary>
		private readonly CLLocationManager _mgr;

		/// <summary>
		/// Gets the location manager.
		/// </summary>
		/// <value>The location manager.</value>
		public CLLocationManager LocationManager { get { return _mgr; } }

		/// <summary>
		/// Occurs when location updated.
		/// </summary>
		public event EventHandler<ILocationUpdateEventArgs> LocationUpdated = delegate { };

		/// <summary>
		/// Initializes a new instance of the <see cref="telstrachallenge2016.iOS.LocationUpdateService"/> class.
		/// </summary>
		public LocationUpdateService()
		{
			_mgr = new CLLocationManager();
			// Check for auth requirement in iOS
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
				_mgr.RequestAlwaysAuthorization();
		}

		/// <summary>
		/// Stops the location updates.
		/// </summary>
		public void StopLocationUpdates()
		{
			_mgr.StopUpdatingLocation();
			try {
				_mgr.LocationsUpdated -= _mgr_LocationsUpdated;
			} catch (Exception ex) {
				Console.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// Starts the location updates.
		/// </summary>
		public void StartLocationUpdates()
		{
			if (!CLLocationManager.LocationServicesEnabled) {
				// TODO: Should raise this.
				return;
			}
			_mgr.DesiredAccuracy = 1;
			_mgr.LocationsUpdated += _mgr_LocationsUpdated;
			_mgr.StartUpdatingLocation();

		}

		/// <summary>
		/// Handles location updated
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void _mgr_LocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
		{
			// Take the last known location
			var nativeLocation = e.Locations[e.Locations.Length - 1];

			// From object
			var coordinates = new Coordinate2D() {
				IsValid = nativeLocation.Coordinate.IsValid(),
				Latitude = nativeLocation.Coordinate.Latitude,
				Longitude = nativeLocation.Coordinate.Longitude
			};
			var gpsLocation = new GpsLocation() {
				Altitude = nativeLocation.Altitude,
				Coordinates = coordinates,
				Heading = nativeLocation.Course,
				HorizontalAccuracy = nativeLocation.HorizontalAccuracy,
				Speed = nativeLocation.Speed,
				TimeStamp = utils.Conversion.NSDateToDateTime(nativeLocation.Timestamp),
				VerticalAccuracy = nativeLocation.VerticalAccuracy
			};
			var eventArgs = new LocationUpdatedEventArgs() {
				Location = gpsLocation
			};
					
			// Trigger event
			LocationUpdated(this, eventArgs);	

		}

	}
}
