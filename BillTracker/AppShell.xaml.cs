namespace BillTracker;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(BillDetailPage), typeof(BillDetailPage));
		Routing.RegisterRoute(nameof(BillsPage), typeof(BillsPage));
    }
}
