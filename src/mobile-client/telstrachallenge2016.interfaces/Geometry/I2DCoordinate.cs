using System;

namespace telstrachallenge2016.interfaces.geometry
{
	public interface I2DCoordinate {
		bool IsValid { get; }
		double Latitude { get; }
		double Longitude { get; }
	}
	
}
