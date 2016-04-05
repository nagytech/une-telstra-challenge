using System;
using telstrachallenge2016.interfaces.gps;

namespace telstrachallenge2016.interfaces
{
	public interface ILocationUpdateEventArgs
	{
		IGpsLocation Location { get; }
	}
	
}
