using System;
using Autofac;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace telstrachallenge2016
{
	public class NavigationService : INavigationService
	{
		private readonly IComponentContext _componentContext;

		public NavigationService(IComponentContext componentContext) {
			_componentContext = componentContext;
		}

		public Task<bool> DisplayAlert(string title, string message, string ok, string cancel) {
			return telstrachallenge2016.App.Current.MainPage.DisplayAlert(title, message, ok, cancel);
		}

		public Task DisplayAlert(string title, string message, string cancel) {
			return telstrachallenge2016.App.Current.MainPage.DisplayAlert(title, message, cancel);
		}

		public async void Pop() {
			await telstrachallenge2016.App.Current.MainPage.Navigation.PopAsync();
		}

		public async void Push<T>() where T : Page {
			var page = Resolve<T>();
			await telstrachallenge2016.App.Current.MainPage.Navigation.PushAsync(page);
		}
		AttributeUsageAttribute
		public void PushToRoot<T>() where T : Page {
			var page = Resolve<T>();
			telstrachallenge2016.App.Current.MainPage = new NavigationPage(page);
		}

		public void SetRoot<T>() where T : Page {
			var page = Resolve<T>();
			telstrachallenge2016.App.Current.MainPage = page;
		}

		private T Resolve<T>() where T : Page {
			var page = _componentContext.Resolve<T>();
			return page;
		}
	}
}

