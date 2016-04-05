using System;

using Xamarin.Forms;

namespace telstrachallenge2016
{
	public class MapPage : ContentPage
	{
		public MapPage()
		{
			Content = new StackLayout { 
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}


