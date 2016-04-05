using System;

namespace telstrachallenge2016.interfaces
{
	public interface ILocationUpdateService
	{
		event EventHandler<ILocationUpdateEventArgs> LocationUpdated;
		void StartLocationUpdates();
		void StopLocationUpdates();
	}
}

