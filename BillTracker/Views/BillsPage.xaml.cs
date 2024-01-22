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
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;

        Items = new ObservableCollection<Bill>(await dataService.GetItemsAsync());
        var currentMonthBills = Items.Where(b => (b.CurrentDueDate.Month == currentMonth && b.CurrentDueDate.Year == currentYear) && !b.IsPaid ||
        (b.CurrentDueDate < new DateTime(currentYear, currentMonth, 1) && !b.IsPaid)).ToList();


        collectionview.ItemsSource = currentMonthBills;

        UpdateTotalAmount(currentMonthBills);
    }

	private async void ItemTapped(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(BillDetailPage), true, new Dictionary<string, object>
		{
			{ "Item", (sender as BindableObject).BindingContext as Bill }
		});
	}

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddBillPage));
    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var bill = swipeItem.BindingContext as Bill;

        if (bill != null)
        {
            bool confirmed = await DisplayAlert("Confirm", $"Delete {bill.Name}?", "Yes", "No");
            if (confirmed)
            {
                await dataService.DeleteBillByIdAsync(bill.Id);
                await LoadDataAsync(); // Reload the bills list
            }
        }
    }

    private void UpdateTotalAmount(IEnumerable<Bill> bills)
    {
        var currentMonth = DateTime.Now.Month;
        var total = bills.Where(b => b.CurrentDueDate.Month == currentMonth || !b.IsPaid)
                         .Sum(b => b.Amount);

        var currentMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
        TotalAmountLabel.Text = $"${total:0.00}";
        CurrentMonthLabel.Text = $"Total for {currentMonthName}:";
    }
}
