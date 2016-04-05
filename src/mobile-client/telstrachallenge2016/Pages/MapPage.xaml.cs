using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace telstrachallenge2016
{
	public partial class MapPage : ContentPage
	{
		public MapPage(MapPageViewModel vm)
		{
			InitializeComponent();
			BindingContext = vm;
		}
	}
}

