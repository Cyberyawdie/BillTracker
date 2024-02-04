using Microsoft.Maui.Controls;

namespace BillTracker.Views;

public partial class MainPage : ContentPage
{ 

    private readonly DataService _dataService;
	public MainPage(DataService service)
	{
        InitializeComponent();
      _dataService = service;
        LoadDataAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadDataAsync(); // Reload data every time the page appears
    }

    private async void LoadDataAsync()
    {
        var bills = await _dataService.GetItemsAsync(); // Fetch bills from the database
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;

        var totalAmountThisMonth = bills.Where(b => (b.CurrentDueDate.Month == currentMonth && b.CurrentDueDate.Year == currentYear) ||
        (b.CurrentDueDate < new DateTime(currentYear, currentMonth, 1)))
                                     .Sum(b => b.Amount);

        var totalDueThisMonth = bills.Where(b => (b.CurrentDueDate.Month == currentMonth && b.CurrentDueDate.Year == currentYear) && !b.IsPaid ||
        (b.CurrentDueDate < new DateTime(currentYear, currentMonth, 1) && !b.IsPaid))
                                     .Sum(b => b.Amount);
        var totalPaidThisMonth = bills.Where(b => ((b.CurrentDueDate.Month == currentMonth && b.CurrentDueDate.Year == currentYear && b.PaymentDate?.Month == currentMonth) && b.IsPaid) || (b.PaymentDate?.Month == currentMonth) && b.IsPaid)
                                    .Sum(b => b.Amount);
        var overdueBillsCount = bills.Count(b => b.CurrentDueDate < DateTime.Now && !b.IsPaid);
        var currentBillsCount = bills.Count(b => b.CurrentDueDate.Month == currentMonth && !b.IsPaid);
        var paidOffBillsCount = bills.Count(b => b.PaymentDate?.Month == currentMonth);

        TotalAmountLabel.Text = $"Total Amount This Month: ${totalAmountThisMonth:0.00}";
        TotalAmountDueLabel.Text = $"Remaining Amount Due This Month: ${totalDueThisMonth:0.00}";
        OverdueBillsLabel.Text = $"Overdue Bills: {overdueBillsCount}";
        CurrentBillsLabel.Text = $"Bills Due This Month: {currentBillsCount}";
        PaidOffBillsLabel.Text = $"Congratulations! You've paid off {paidOffBillsCount} bill(s) this month totaling: ${totalPaidThisMonth:0.00} .";
    }

}
