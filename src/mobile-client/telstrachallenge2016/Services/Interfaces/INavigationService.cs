using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace telstrachallenge2016
{
	public interface INavigationService
	{
		Task DisplayAlert(string title, string message, string cancel);
		Task<bool> DisplayAlert(string title, string message, string ok, string cancel);
		void Pop();
		void Push<T>() where T : Page;
		void PushToRoot<T>() where T : Page;
		void SetRoot<T>() where T : Page;
	}
}

