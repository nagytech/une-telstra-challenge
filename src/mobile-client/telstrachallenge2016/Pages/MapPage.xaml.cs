using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace telstrachallenge2016
{
	public partial class MapPage : ContentPage
	{
		public MapPage(MapPageViewModel vm)
		{
			InitializeComponent();
			BindingContext = vm;
		}

		public void MapFrameChanged(float bearing, float zoom, double lat, double lng, double maxx, double maxy, double minx, double miny, float tilt)
		{
			// TODO: Return some stuff

		}
	}
}

