namespace BillTracker.Views;

public partial class BillsPage : ContentPage
{
	readonly DataService dataService;
	private ObservableCollection<Bill> Items { get; set; }

	public BillsPage(DataService service)
	{
		InitializeComponent();

		dataService = service;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		await LoadDataAsync();
	}
	private async void OnRefreshing(object sender, EventArgs e)
	{
		refreshview.IsRefreshing = true;

		try
		{
			await LoadDataAsync();
		}
		finally
		{
			refreshview.IsRefreshing = false;
		}
	}

	private async Task LoadDataAsync()
	{
		Items = new ObservableCollection<Bill>(await dataService.GetItemsAsync());

        collectionview.ItemsSource = Items;
	}

	private async void ItemTapped(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(BillDetailPage), true, new Dictionary<string, object>
		{
			{ "Item", (sender as BindableObject).BindingContext as Bill }
		});
	}
}
