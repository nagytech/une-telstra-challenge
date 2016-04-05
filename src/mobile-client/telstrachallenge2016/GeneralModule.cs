using System;

using Xamarin.Forms;
using Autofac;

namespace telstrachallenge2016
{
	public class GeneralModule : Module {

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<MainPage>().SingleInstance();
			builder.RegisterType<MainPageViewModel>().SingleInstance();

			builder.RegisterType<MapPage>().SingleInstance();
			builder.RegisterType<MapPageViewModel>().SingleInstance();

			builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
		}

	}
	
}
