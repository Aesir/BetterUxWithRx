using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Concurrency;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

namespace GroupEventsAndProcessTogether
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
		: Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = this;

			_Numbers = new ObservableCollection<int>();
			_NumberSets = new ObservableCollection<string>();

			SetupObserver();
		}

		private readonly ObservableCollection<int> _Numbers;
		public ObservableCollection<int> Numbers
		{
			get
			{
				return _Numbers;
			}
		}

		private readonly ObservableCollection<string> _NumberSets;
		public ObservableCollection<string> NumberSets
		{
			get
			{
				return _NumberSets;
			}
		}


		private void Button_Click(object sender, RoutedEventArgs e)
		{
			int num = int.Parse(txtNumberToAdd.Text);
			Numbers.Add(num);
			txtNumberToAdd.Text = (num + 1).ToString();
		}

		private void SetupObserver()
		{
			Observable
			  .FromEvent<NotifyCollectionChangedEventArgs>(this.Numbers, "CollectionChanged")
			  .Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Add)
			  .Select( //Changes from the event to the number that was added
				e => e.EventArgs
					.NewItems
					.OfType<int>()
					.Single())
			  .BufferWithTime(TimeSpan.FromSeconds(5), Scheduler.Dispatcher)
			  //.Where(nums => nums.Any())
			  .Subscribe(
					nums => 
					{
						var numberStr = nums.Aggregate("", (acc, num) => string.Format("{0} {1}", acc, num));
						this.NumberSets.Add(numberStr);
					});
		}

	}
}
