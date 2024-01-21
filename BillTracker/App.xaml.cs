namespace BillTracker;

public partial class App : Application
{
    public static DataService Database;
    public App()
	{
		InitializeComponent();

        MainPage = new AppShell();
	}
}
