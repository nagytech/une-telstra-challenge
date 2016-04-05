using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace telstrachallenge2016
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		private bool _isBusy;
		public bool IsBusy 
		{ 
			get {
				return _isBusy;
			} 

			set {
				if (value == _isBusy)
					return;
				_isBusy = value;
				OnPropertyChanged();
				OnPropertyChanged("IsNotBusy");
			}
		}

		public bool IsNotBusy { get { return !IsBusy; }}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

	}
}

