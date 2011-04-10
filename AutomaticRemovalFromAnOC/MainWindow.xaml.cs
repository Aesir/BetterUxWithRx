using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Concurrency;
using System.Linq;
using System.Windows;

namespace AutomaticRemovalFromAnOC
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
			  .Delay(TimeSpan.FromSeconds(5), Scheduler.Dispatcher)
			  .Subscribe(
				  e =>
				  {
					  foreach (int i in e.EventArgs.NewItems)
					  {
						  this.Numbers.Remove(i);
					  }
				  });
		}

	}
}
