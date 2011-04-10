using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Concurrency;
using System.Linq;
using System.Windows;
using System.Threading;
using System.ComponentModel;
using System.Windows.Controls;

namespace DoSomethingWhenTheUserStopsTyping
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = this;

			Observable
				.FromEvent<TextChangedEventArgs>(txtUser, "TextChanged")
				.Throttle(TimeSpan.FromMilliseconds(500), Scheduler.Dispatcher)
				.Subscribe(_ => ReverseText(txtUser.Text));
		}

		private string _Output;
		public string Output
		{
			get
			{
				return _Output;
			}
			set
			{
				if (!ReferenceEquals(_Output, value))
				{
					_Output = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new PropertyChangedEventArgs("Output"));
				}
			}
		}

		private void ReverseText(string text)
		{
			this.Output = new string(text.Reverse().ToArray());
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
