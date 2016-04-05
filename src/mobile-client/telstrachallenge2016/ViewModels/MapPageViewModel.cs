using System;

namespace telstrachallenge2016
{
	public class MapPageViewModel
	{
		public event EventHandler MapPageStatusChanged;

		public class MapPageStatusChangedEventArgs : EventArgs {
			// TODO: Routes, Friends, etc.
		}

		public MapPageViewModel()
		{
		}
	}
}

