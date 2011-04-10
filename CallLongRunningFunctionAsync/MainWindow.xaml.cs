using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Concurrency;
using System.Linq;
using System.Windows;
using System.Threading;
using System.ComponentModel;

namespace CallLongRunningFunctionAsync
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

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Observable
			 .ToAsync<int>(LifeTheUniverseEverything)()
			 .ObserveOnDispatcher()
			 .Subscribe(theAnswer => this.Output = theAnswer.ToString());
		}

		private int LifeTheUniverseEverything()
		{
			Thread.Sleep(5000);
			return 42;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
