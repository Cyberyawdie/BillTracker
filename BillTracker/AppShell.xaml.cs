namespace BillTracker;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(BillDetailPage), typeof(BillDetailPage));
		Routing.RegisterRoute(nameof(AddBillPage), typeof(AddBillPage));
        Routing.RegisterRoute(nameof(BillsPage), typeof(BillsPage));
        Routing.RegisterRoute(nameof(BillHistoryPage), typeof(BillHistoryPage));
        Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
    }
}
