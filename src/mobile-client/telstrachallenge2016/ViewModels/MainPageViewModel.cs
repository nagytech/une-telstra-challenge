using System;

namespace telstrachallenge2016
{
	public class MainPageViewModel : ViewModelBase
	{
		INavigationService navigationService;

		Xamarin.Forms.Command _handleMapNav;

		public Xamarin.Forms.Command HandleMapNav { 
			get {
				if (_handleMapNav == null)
					_handleMapNav = new Xamarin.Forms.Command((e) => {
						navigationService.Push<MapPage>();	
					});
				return _handleMapNav;
			}
		}

		public MainPageViewModel(INavigationService navigationService)
		{
			this.navigationService = navigationService;
		}
	}
}

