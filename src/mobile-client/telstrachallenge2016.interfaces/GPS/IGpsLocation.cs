using System;
using telstrachallenge2016.interfaces.geometry;

namespace telstrachallenge2016.interfaces.gps
{
	public interface IGpsLocation
	{
		I2DCoordinate Coordinates { get; }
		double Altitude { get; }
		double Heading { get; }
		double HorizontalAccuracy { get; }
		double Speed { get; }
		double VerticalAccuracy { get; }
		DateTime TimeStamp { get; }
	}
	
}
