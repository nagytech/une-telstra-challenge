using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Google.Maps;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using telstrachallenge2016;
using telstrachallenge2016.interfaces.geometry;
using telstrachallenge2016.iOS;

[assembly:ExportRenderer(typeof(MapPage), typeof(MapViewRenderer))]

namespace telstrachallenge2016.iOS
{
	public class MapViewRenderer : PageRenderer
	{
		private MapPage xMapPage;

		public MapViewRenderer()
		{

		}

		/// <summary>
		/// Raises the element changed event (when page loads)
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			xMapPage = (MapPage)e.NewElement;

			var vm = (xMapPage.BindingContext as MapPageViewModel);
			vm.MapPageStatusChanged += MapPage_StatusChanged;

			// Ignore unmounted events
			if (e.OldElement != null || Element == null)
				return;

			try {

				// TODO: Persist center / zoom, or get from user location
				var camera = CameraPosition.FromCamera(-34, 151, 12);
				var mapView = MapView.FromCamera(RectangleF.Empty, camera);

				mapView.MyLocationEnabled = true;
				//mapView.Settings.MyLocationButton = true; // TODO: On click
				//mapView.Settings.CompassButton = true;
				mapView.Settings.SetAllGesturesEnabled(true);

				mapView.CameraPositionIdle += MapPage_CameraPositionIdle;

				View = mapView;

			} catch (Exception ex) {

			}

		}

		/// <summary>
		/// Handles the event where the map page status has changed
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		async void MapPage_StatusChanged(object sender, EventArgs _e)
		{
			var e = (telstrachallenge2016.MapPageViewModel.MapPageStatusChangedEventArgs)_e;
			// TODO: display other users, update route - displauy alerts etc
		}

		/// <summary>
		/// Handles the event where the map page is idle
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e2">E2.</param>
		async void MapPage_CameraPositionIdle(object sender, GMSCameraEventArgs e)
		{
			var mapView = View as MapView;

			var fr = mapView.Projection.VisibleRegion.FarRight;
			var nl = mapView.Projection.VisibleRegion.NearLeft;

			/* 
			 * Position information can be sent back tot he server:
			 * e.Position.Bearing, 
			 * e.Position.Zoom, 
			 * e.Position.Target.Latitude, 
			 * e.Position.Target.Longitude, 
			 * fr.Longitude, 
			 * fr.Latitude, 
			 * nl.Longitude, 
			 * nl.Latitude, 
			 * e.Position.ViewingAngle
			*/

		}

	}
}

