

namespace BillTracker.Views;

public partial class AddBillPage : ContentPage
{
    readonly DataService dataService;

    public AddBillPage(DataService service)
	{
		InitializeComponent();
        dataService = service;
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
}