namespace BillTracker.Views;

[QueryProperty(nameof(Item), "Item")]
public partial class BillDetailPage : ContentPage
{
    readonly DataService dataService;
    private Bill _bill;

	public Bill Item
	{
		get { return _bill; }
		set
		{
           
			_bill = value;
            SetBillDetails();
        }
	}

	public BillDetailPage(DataService service)
	{
		InitializeComponent();
        dataService = service;
    }

    private void SetBillDetails()
    {
        NameLabel.Text = _bill.Name;
        AmountLabel.Text = $"${_bill.Amount}";
        DueDateLabel.Text = _bill.CurrentDueDate.ToString("d");
        IsPaidLabel.Text = _bill.IsPaid ? "Yes" : "No";

        MarkAsPaidButton.IsEnabled = !_bill.IsPaid;
    }

    private async void OnMarkAsPaidClicked(object sender, EventArgs e)
    {
        _bill.IsPaid = true;
        _bill.PaymentDate = DateTime.Now;

        await dataService.UpdateBillAsync(_bill); // Update bill in database
        SetBillDetails();

        // Optionally navigate back or update UI further
    }
}
