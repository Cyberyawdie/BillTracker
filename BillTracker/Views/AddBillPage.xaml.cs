

namespace BillTracker.Views;

public partial class AddBillPage : ContentPage
{
    readonly DataService dataService;


    public AddBillPage(DataService service)
    {
        InitializeComponent();
        dataService = service;
        LoadBillNames();
    }
    private List<string>? _allBillNames; // Store all bill names

    private async void LoadBillNames()
    {
        // Fetch distinct bill names from your database or data source
        _allBillNames = await dataService.GetDistinctBillNamesAsync();
    }

    private async void OnAddBillClicked(object sender, EventArgs e)
    {
        var billName = NameEntry.Text;
        var billAmount = decimal.Parse(AmountEntry.Text);
        var billDueDate = DueDatePicker.Date;

        var newBill = new Bill
        {
            Name = billName,
            Amount = billAmount,
            OriginalDueDate = billDueDate,
            CurrentDueDate = billDueDate,
            IsPaid = false
        };

        await dataService.AddBillAsync(newBill);
        await Shell.Current.GoToAsync(".."); // Navigate back
    }

    private void BillNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue.ToLower();
        if (_allBillNames != null)
        {
            // Filter bill names based on the user's input
            var filteredNames = _allBillNames.Where(name => name.ToLower().Contains(searchText)).ToList();

            // Update the CollectionView's ItemsSource with the filtered names
            BillNameSuggestions.ItemsSource = filteredNames;

            // Show or hide the suggestions based on whether there are any matches
            BillNameSuggestions.IsVisible = filteredNames.Any();
        }
    }

    private void BillNameSuggestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is string selectedName)
        {
            // Set the selected name to the Entry and hide suggestions
            NameEntry.Text = selectedName;
            BillNameSuggestions.IsVisible = false;
        }
    }
}