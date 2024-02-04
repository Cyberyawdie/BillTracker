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

    private async void OnPartialPaymentClicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(PartialPaymentEntry.Text, out decimal partialPayment) && partialPayment > 0)
        {
            if (partialPayment > _bill.Amount)
            {
                await DisplayAlert("Error", "Payment exceeds bill amount.", "OK");
                return;
            }

            // Update the bill amount
            _bill.Amount -= partialPayment;

            // Optionally, record the payment transaction

            // Update the bill in the database
            await dataService.UpdateBillAsync(_bill);

            // Update UI to reflect new bill amount
            // For example: BillAmountLabel.Text = $"Amount Due: {_bill.Amount:C}";

            // Clear the partial payment entry field
            PartialPaymentEntry.Text = string.Empty;

            // Display a confirmation message
            await DisplayAlert("Payment Successful", $"Partial payment of {partialPayment:C} applied.", "OK");
            SetBillDetails();

        }
        else
        {
            await DisplayAlert("Error", "Invalid payment amount.", "OK");
            SetBillDetails();

        }
    }
}
