using System;

using Xamarin.Forms;
using Autofac;

namespace telstrachallenge2016
{
	public class AppLoader {

		private static bool _hasRun = false;

		public static void Run() {
			if (_hasRun) return;

			var builder = new ContainerBuilder();
			builder.RegisterModule<GeneralModule>();

			var container = builder.Build();

			var page = container.Resolve<MainPage>();
			telstrachallenge2016.App.Current.MainPage = new NavigationPage(page);

			_hasRun = true;
		}

	}
	
}
