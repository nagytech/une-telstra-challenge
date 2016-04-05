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
	/// Implementation of I2DCoordinate
	/// </summary>
	public class Coordinate2D : I2DCoordinate
	{
		public bool IsValid { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }
	}
	
}
