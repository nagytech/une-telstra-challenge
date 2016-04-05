using System;
using CoreLocation;
using UIKit;

using Foundation;
using telstrachallenge2016.iOS;
using telstrachallenge2016.interfaces;
using telstrachallenge2016.interfaces.gps;
using telstrachallenge2016.interfaces.geometry;

namespace telstrachallenge2016.iOS
{
	/// <summary>
	/// Implementation of IGpsLocation
	/// </summary>
	public class GpsLocation : IGpsLocation
	{
		public I2DCoordinate Coordinates { get; set; }

		public double Altitude { get; set; }

		public double Heading { get; set; }

		public double HorizontalAccuracy { get; set; }

		public double Speed { get; set; }

		public DateTime TimeStamp { get; set; }

		public double VerticalAccuracy { get; set; }

	}

	
}
