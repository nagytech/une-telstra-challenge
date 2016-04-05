using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace telstrachallenge2016
{
	public partial class MainPage : ContentPage
	{
		public MainPage(MainPageViewModel vm)
		{
			InitializeComponent();
			BindingContext = vm;
		}
	}
}

