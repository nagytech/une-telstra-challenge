using System;
using CoreLocation;
using UIKit;

using Foundation;
using telstrachallenge2016.iOS;
using telstrachallenge2016.interfaces;
using telstrachallenge2016.interfaces.gps;

namespace telstrachallenge2016.iOS
{
	/// <summary>
	/// Implementation of ILocationUpdateEventArgs
	/// </summary>
	public class LocationUpdatedEventArgs : ILocationUpdateEventArgs
	{
		public IGpsLocation Location { get; set; }

		public LocationUpdatedEventArgs()
		{

		}
	}
	
}
