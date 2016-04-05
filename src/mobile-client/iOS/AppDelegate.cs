using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Google.Maps;

namespace telstrachallenge2016.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			MapServices.ProvideAPIKey("AIzaSyBeuJPP0QNaKxGZlLoHnaT9QCn5hEsTHb8");

			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}

