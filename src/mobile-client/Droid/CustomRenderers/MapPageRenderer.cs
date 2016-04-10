using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Gms.Maps;
using telstrachallenge2016;
using telstrachallenge2016.Droid;

[assembly:ExportRenderer(typeof(MapPage), typeof(MapPageRenderer))]
namespace telstrachallenge2016.Droid
{
	public class MapPageRenderer : PageRenderer, TextureView.ISurfaceTextureListener, IOnMapReadyCallback, Android.Gms.Maps.GoogleMap.IOnCameraChangeListener
	{
		Activity activity;
		global::Android.Views.View view;
		MapFragment myMapFragment;

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null) {
				return;
			}

			try {
				SetupUserInterface();
				SetupNativeHooks();
				AddView(view);
			} catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);

			var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
			var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

			view.Measure(msw, msh);
			view.Layout(0, 0, r - l, b - t);
		}

		public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int width, int height)
		{

		}

		public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
		{
			return true;
		}

		public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height)
		{

		}

		public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface)
		{

		}

		void SetupUserInterface()
		{

			activity = this.Context as Activity;
			view = activity.LayoutInflater.Inflate(Resource.Layout.MapFrameLayout, this, false);
			myMapFragment = MapFragment.NewInstance();
			FragmentTransaction tx = activity.FragmentManager.BeginTransaction();
			tx.Add(Resource.Id.map_frame, myMapFragment);
			tx.Commit();

			myMapFragment.GetMapAsync(this);

		}

		void SetupNativeHooks()
		{
			MapPage xMapView = (MapPage)Element;
		}

		private GoogleMap googleMap;

		public void OnMapReady(GoogleMap googleMap)
		{
			this.googleMap = googleMap;
			googleMap.MyLocationEnabled = true;
			googleMap.BuildingsEnabled = true;
			googleMap.MapClick += (object sender, GoogleMap.MapClickEventArgs e) => {
				// TODO: Navigation stuff.
			};
			// TODO: if map is hidden, we don't want to spend time moving
			// TODO: Get any routes, friends, etc in progress
			googleMap.SetOnCameraChangeListener(this);
		}

		public async void OnCameraChange(Android.Gms.Maps.Model.CameraPosition position)
		{
			MapPage xMapView = (MapPage)Element;
			var fr = googleMap.Projection.VisibleRegion.FarRight;
			var nl = googleMap.Projection.VisibleRegion.NearLeft;
		}
	}
}



